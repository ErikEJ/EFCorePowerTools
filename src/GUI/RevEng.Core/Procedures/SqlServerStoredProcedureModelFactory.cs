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

        public ModuleModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetStoredProcedures(connectionString, options);
        }

        private ModuleModel GetStoredProcedures(string connectionString, ModuleModelFactoryOptions options)
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

                foreach (var foundModule in found)
                {
                    if (filter.Count == 0 || filter.Contains($"[{foundModule.Item1}].[{foundModule.Item2}]"))
                    {
                        var module = new Procedure
                        {
                            Schema = foundModule.Item1,
                            Name = foundModule.Item2,
                            HasValidResultSet = true,
                        };

                        if (options.FullModel)
                        {
                            module.Parameters = GetParameters(connection, module.Schema, module.Name);
                            try
                            {
                                module.ResultElements = GetResultElements(connection, module.Schema, module.Name);
                            }
                            catch (Exception ex)
                            {
                                module.HasValidResultSet = false;
                                errors.Add($"Unable to get result set shape for procedure '{module.Schema}.{module.Name}'{Environment.NewLine}{ex.Message}{Environment.NewLine}");
                                _logger?.Logger.LogWarning(ex, $"Unable to scaffold {module.Schema}.{module.Name}");
                            }
                        }

                        result.Add(module);
                    }
                }
            }

            return new ModuleModel
            {
                Routines = result.Cast<Module>().ToList(),
                Errors = errors,
            };
        }

        private List<ModuleParameter> GetParameters(SqlConnection connection, string schema, string name)
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

        private List<ModuleResultElement> GetResultElements(SqlConnection connection, string schema, string name)
        {
            var dtResult = new DataTable();
            var result = new List<ModuleResultElement>();

            var sql = $"exec dbo.sp_describe_first_result_set N'[{schema}].[{name}]';";

            var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection)
            };

            adapter.Fill(dtResult);

            int rCounter = 0;

            foreach (DataRow res in dtResult.Rows)
            {
                var parameter = new ModuleResultElement()
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
