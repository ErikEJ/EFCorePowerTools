using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace RevEng.Core.Procedures
{
    public class SqlServerFunctionModelFactory : IFunctionModelFactory
    {
        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public SqlServerFunctionModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
        }

        public FunctionModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetFunctions(connectionString, options);
        }

        private FunctionModel GetFunctions(string connectionString, ModuleModelFactoryOptions options)
        {
            var result = new List<Function>();
            var found = new List<Tuple<string, string, int, bool>>();
            var errors = new List<string>();

            var filter = options.Modules.ToHashSet();

            using (var connection = new SqlConnection(connectionString))
            {
#if CORE50 || CORE60

                var sql = @"
SELECT SCHEMA_NAME(schema_id) AS [Schema], name AS [Name], object_id, CAST(objectproperty(OBJECT_ID,'IsScalarFunction') AS bit) AS IsScalar
FROM sys.objects 
WHERE (objectproperty(OBJECT_ID,'IsScalarFunction') = 1 OR objectproperty(OBJECT_ID,'IsTableFunction') = 1)
AND NULLIF([name], '') IS NOT NULL;";

#else

                var sql = @"
SELECT SCHEMA_NAME(schema_id) AS [Schema], name AS [Name], object_id, CAST(objectproperty(OBJECT_ID,'IsScalarFunction') AS bit) AS IsScalar
FROM sys.objects 
WHERE objectproperty(OBJECT_ID,'IsScalarFunction') = 1
AND NULLIF([name], '') IS NOT NULL;";

#endif

                using (var command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            found.Add(new Tuple<string, string, int, bool>(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetBoolean(3)));
                        }
                    }
                }

                foreach (var foundFunction in found)
                {
                    if (filter.Count == 0 || filter.Contains($"[{foundFunction.Item1}].[{foundFunction.Item2}]"))
                    {
                        var function = new Function
                        {
                            Schema = foundFunction.Item1,
                            Name = foundFunction.Item2,
                            IsScalar = foundFunction.Item4
                        };

                        if (options.FullModel)
                        {
                            function.Parameters = GetFunctionParameters(connection, foundFunction.Item3);

                            if (!function.IsScalar)
                            {
                                try
                                {
                                    function.ResultElements = GetTableFunctionResultElements(connection, foundFunction.Item3).Cast<ModuleResultElement>().ToList();
                                }
                                catch (Exception ex)
                                {
                                    function.HasValidResultSet = false;
                                    errors.Add($"Unable to get result set shape for function '{function.Schema}.{function.Name}'{Environment.NewLine}{ex.Message}{Environment.NewLine}");
                                    _logger?.Logger.LogWarning(ex, $"Unable to scaffold {function.Schema}.{function.Name}");
                                }
                            }
                        }

                        result.Add(function);
                    }
                }
            }

            return new FunctionModel
            {
                Routines = result.Cast<ModuleBase>().ToList(),
                Errors = errors,
            };
        }

        private List<ModuleParameter> GetFunctionParameters(SqlConnection connection, long objectId)
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
    p.is_output AS Output
    from sys.parameters p
    where object_id = {objectId}
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
                };

                result.Add(parameter);
            }

            return result;
        }

        private List<TableFunctionResultElement> GetTableFunctionResultElements(SqlConnection connection, int objectId)
        {
            var dtResult = new DataTable();
            var result = new List<TableFunctionResultElement>();

            var sql = $@"
SELECT 
    c.name,
    COALESCE(type_name(c.system_type_id), type_name(c.user_type_id)) AS type_name,
    c.column_id AS column_ordinal,
    c.is_nullable
FROM sys.columns c
WHERE object_id = {objectId};";

            var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection)
            };

            adapter.Fill(dtResult);

            int rCounter = 0;

            foreach (DataRow res in dtResult.Rows)
            {
                var parameter = new TableFunctionResultElement()
                {
                    Name = string.IsNullOrEmpty(res["name"].ToString()) ? $"Col{rCounter}" : res["name"].ToString(),
                    StoreType = res["type_name"].ToString(),
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
