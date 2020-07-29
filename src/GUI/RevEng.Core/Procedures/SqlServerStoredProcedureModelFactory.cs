using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using RevEng.Core.Procedures.Model;
using RevEng.Core.Procedures.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Data;

namespace RevEng.Core.Procedures
{
    class SqlServerStoredProcedureModelFactory : IProcedureModelFactory
    {
        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public SqlServerStoredProcedureModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
        }

        public ProcedureModel Create(string connectionString)
        {
            var model = new ProcedureModel
            {
                Procedures = GetStoredProcedures(connectionString)
            };

            return model;
        }


        private List<Procedure> GetStoredProcedures(string connectionString)
        {
            var dtResult = new DataTable();
            var result = new List<Procedure>();

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

                foreach (DataRow row in dtResult.Rows)
                {
                    try
                    {
                        var procedure = new Procedure
                        {
                            Schema = row["ROUTINE_SCHEMA"].ToString(),
                            Name = row["ROUTINE_NAME"].ToString()
                        };

                        procedure.Parameters = GetStoredProcedureParameters(connection, procedure.Schema, procedure.Name);

                        procedure.ResultElements = GetStoredProcedureResultElements(connection, procedure.Schema, procedure.Name);

                        result.Add(procedure);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex);
                        System.Diagnostics.Trace.WriteLine($"Unable to scaffold {row["ROUTINE_NAME"]}");
                        _logger?.Logger.LogWarning(ex, $"Unable to scaffold {row["ROUTINE_NAME"]}");
                    }
                }
            }

            return result;
        }

        private List<ProcedureParameter> GetStoredProcedureParameters(SqlConnection connection, string schema, string name)
        {
            var dtResult = new DataTable();
            var result = new List<ProcedureParameter>();

            // Validate this - based on https://stackoverflow.com/questions/20115881/how-to-get-stored-procedure-parameters-details/41330791

            var sql = $@"
SELECT  
    'Parameter' = name,  
    'Type'   = type_name(system_type_id),  
    'Length'   = CAST(max_length AS INT),  
    'Precision'   = CAST(case when type_name(system_type_id) = 'uniqueidentifier' 
                then precision  
                else OdbcPrec(system_type_id, max_length, precision) end AS INT),  
    'Scale'   = CAST(OdbcScale(system_type_id, scale) AS INT),  
    'Order'  = CAST(parameter_id AS INT),  
    'Collation'   = convert(sysname, 
                    case when system_type_id in (35, 99, 167, 175, 231, 239)  
                    then ServerProperty('collation') end),
    is_output AS output,
	is_nullable AS nullable
    from sys.parameters where object_id = object_id('{schema}.{name}')
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
                    Precision = par["Precision"].GetType() == typeof(DBNull) ? (byte?)null : byte.Parse(par["Precision"].ToString()),
                    Scale = par["Scale"].GetType() == typeof(DBNull) ? (byte?)null : byte.Parse(par["Scale"].ToString()),
                    Ordinal = int.Parse(par["Order"].ToString()),
                    Output = (bool)par["output"],
                    Nullable = (bool)par["nullable"],
                };

                result.Add(parameter);
            }

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