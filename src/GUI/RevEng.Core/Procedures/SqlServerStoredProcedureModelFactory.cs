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
    public class SqlServerStoredProcedureModelFactory : IProcedureModelFactory
    {
        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public SqlServerStoredProcedureModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
        }

        public ProcedureModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetStoredProcedures(connectionString, options);
        }

        private ProcedureModel GetStoredProcedures(string connectionString, ModuleModelFactoryOptions options)
        {
            var result = new List<Procedure>();
            var found = new List<Tuple<string, string>>();
            var errors = new List<string>();

            var filter = options.Modules.ToHashSet();

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = $@"
SELECT ROUTINE_SCHEMA, ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_TYPE = 'PROCEDURE'
AND NULLIF(ROUTINE_NAME, '') IS NOT NULL
ORDER BY ROUTINE_NAME;";

                using (var command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            found.Add(new Tuple<string, string>(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                }

                foreach (var foundProcedure in found)
                {
                    if (filter.Count == 0 || filter.Contains($"[{foundProcedure.Item1}].[{foundProcedure.Item2}]"))
                    {
                        var procedure = new Procedure
                        {
                            Schema = foundProcedure.Item1,
                            Name = foundProcedure.Item2,
                            HasValidResultSet = true,
                        };

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
                                errors.Add($"Unable to get result set shape for procedure '{procedure.Schema}.{procedure.Name}'{Environment.NewLine}{ex.Message}{Environment.NewLine}");
                                _logger?.Logger.LogWarning(ex, $"Unable to scaffold {procedure.Schema}.{procedure.Name}");
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

        private List<ModuleParameter> GetStoredProcedureParameters(SqlConnection connection, string schema, string name)
        {
            var dtResult = new DataTable();
            var result = new List<ModuleParameter>();

            // Validate this - based on https://stackoverflow.com/questions/20115881/how-to-get-stored-procedure-parameters-details/41330791

            var sql = $@"
SELECT  
    'Parameter' = p.name,  
    'Type'   = COALESCE(type_name(p.system_type_id), type_name(p.user_type_id)),  
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
                var parameterName = par["Parameter"].ToString();
                if (parameterName.StartsWith("@", StringComparison.Ordinal))
                {
                    parameterName = parameterName.Substring(1);
                }

                var parameter = new ModuleParameter()
                {
                    Name = parameterName,
                    StoreType = par["Type"].ToString(),
                    Length = par["Length"].GetType() == typeof(DBNull) ? (int?)null : int.Parse(par["Length"].ToString()),
                    Precision = par["Precision"].GetType() == typeof(DBNull) ? (int?)null : int.Parse(par["Precision"].ToString()),
                    Scale = par["Scale"].GetType() == typeof(DBNull) ? (int?)null : int.Parse(par["Scale"].ToString()),
                    Output = (bool)par["output"],
                    Nullable = true,
                    TypeName = par["TypeName"].ToString(),
                };

                result.Add(parameter);
            }

            // Add parameter to hold the standard return value
            result.Add(new ModuleParameter()
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
                    StoreType = string.IsNullOrEmpty(res["system_type_name"].ToString()) ? res["user_type_name"].ToString() : res["system_type_name"].ToString(),
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