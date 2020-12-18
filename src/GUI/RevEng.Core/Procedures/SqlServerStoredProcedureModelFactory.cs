using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RevEng.Core.Procedures
{
    class SqlServerStoredProcedureModelFactory : IProcedureModelFactory
    {
        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public SqlServerStoredProcedureModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
        }

        public ProcedureModel Create(string connectionString, ProcedureModelFactoryOptions options)
        {
            return GetStoredProcedures(connectionString, options);
        }

        private ProcedureModel GetStoredProcedures(string connectionString, ProcedureModelFactoryOptions options)
        {
            var dtResult = new DataTable();
            var result = new List<Procedure>();
            var errors = new List<string>();

            if (options.FullModel && !options.Procedures.Any())
            {
                return new ProcedureModel
                {
                    Procedures = result,
                    Errors = errors,
                };
            }

            using (var connection = new SqlConnection(connectionString))
            {
                string sql = $@"
SELECT * FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_TYPE = 'PROCEDURE'
ORDER BY ROUTINE_NAME";

                connection.Open();

                var adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(sql, connection)
                };

                adapter.Fill(dtResult);

                var filter = options.Procedures.ToHashSet();

                foreach (DataRow row in dtResult.Rows)
                {
                    var procedure = new Procedure
                    {
                        Schema = row["ROUTINE_SCHEMA"].ToString(),
                        Name = row["ROUTINE_NAME"].ToString(),
                        HasValidResultSet = true,
                    };

                    if (filter.Count == 0 || filter.Contains($"[{procedure.Schema}].[{procedure.Name}]"))
                    {
                        if (options.FullModel)
                        {
                            procedure.Parameters = GetStoredProcedureParameters(connection, procedure.Schema, procedure.Name);
                            try
                            {
                                procedure.ResultElements = GetStoredProcedureResultElements(connection, procedure.Schema, procedure.Name);
                            }
                            catch (Exception ex)
                            {
                                procedure.HasValidResultSet = false;
                                errors.Add($"Unable to get result set shape for {procedure.Schema}.{procedure.Name}" + Environment.NewLine + ex.Message);
                                _logger?.Logger.LogWarning(ex, $"Unable to scaffold {row["ROUTINE_NAME"]}");
                            }
                        }

                        result.Add(procedure);
                    }
                }
            }

            return new ProcedureModel
            {
                Procedures = result,
                Errors = errors,
            };
        }

        private List<ProcedureParameter> GetStoredProcedureParameters(SqlConnection connection, string schema, string name)
        {
            var dtResult = new DataTable();
            var result = new List<ProcedureParameter>();

            // Validate this - based on https://stackoverflow.com/questions/20115881/how-to-get-stored-procedure-parameters-details/41330791

            var sql = $@"
SELECT  
    'Parameter' = p.name,  
    'Type'   = type_name(p.system_type_id),  
    'Length'   = CAST(p.max_length AS INT),  
    'Precision'   = CAST(case when type_name(p.system_type_id) = 'uniqueidentifier' 
                then p.precision  
                else OdbcPrec(p.system_type_id, p.max_length, p.precision) end AS INT),  
    'Scale'   = CAST(OdbcScale(p.system_type_id, p.scale) AS INT),  
    'Order'  = CAST(parameter_id AS INT),  
    'Collation'   = convert(sysname, 
                    case when p.system_type_id in (35, 99, 167, 175, 231, 239)  
                    then ServerProperty('collation') end),
    p.is_output AS output,
	p.is_nullable AS nullable,
    'TypeName' = QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(TYPE_NAME(p.user_type_id))
    from sys.parameters p
	LEFT JOIN sys.table_types t ON t.user_type_id = p.user_type_id
    where object_id = object_id('{schema}.{name}')
    ORDER BY parameter_id;";

            var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection)
            };

            adapter.Fill(dtResult);

            foreach (DataRow par in dtResult.Rows)
            {
                var parameter = new ProcedureParameter()
                {
                    Name = par["Parameter"].ToString().Replace("@", ""),
                    StoreType = par["Type"].ToString(),
                    Length = par["Length"].GetType() == typeof(DBNull) ? (int?)null : int.Parse(par["Length"].ToString()),
                    Precision = par["Precision"].GetType() == typeof(DBNull) ? (int?)null : int.Parse(par["Precision"].ToString()),
                    Scale = par["Scale"].GetType() == typeof(DBNull) ? (int?)null : int.Parse(par["Scale"].ToString()),
                    Output = (bool)par["output"],
                    Nullable = (bool)par["nullable"],
                    TypeName = par["TypeName"].ToString(),
                };

                result.Add(parameter);
            }

            // Add parameter to hold the standard return value
            result.Add(new ProcedureParameter()
            {
                Name = "returnValue",
                StoreType = "int",
                Output = true,
                Nullable = false,
            });

            return result;
        }

        private List<ProcedureResultElement> GetStoredProcedureResultElements(SqlConnection connection, string schema, string name)
        {
            var dtResult = new DataTable();
            var result = new List<ProcedureResultElement>();

            var sql = $@"exec dbo.sp_describe_first_result_set N'[{schema}].[{name}]';";

            var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection)
            };

            adapter.Fill(dtResult);

            int rCounter = 0;

            foreach (DataRow res in dtResult.Rows)
            {
                var parameter = new ProcedureResultElement()
                {
                    Name = string.IsNullOrEmpty(res["name"].ToString()) ? $"Col{rCounter}" : res["name"].ToString(),
                    StoreType = res["system_type_name"].ToString(),
                    Ordinal = int.Parse(res["column_ordinal"].ToString()),
                    Nullable = (bool)res["is_nullable"],
                };

                result.Add(parameter);

                rCounter++;
            }

            return result;
        }
    }
}