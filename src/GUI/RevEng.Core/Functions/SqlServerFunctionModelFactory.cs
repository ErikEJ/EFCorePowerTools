using Microsoft.Data.SqlClient;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RevEng.Core.Procedures
{
    public class SqlServerFunctionModelFactory : IFunctionModelFactory
    {
        public FunctionModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetFunctions(connectionString, options);
        }

        private FunctionModel GetFunctions(string connectionString, ModuleModelFactoryOptions options)
        {
            var result = new List<Function>();
            var found = new List<Tuple<string, string, int>>();
            var errors = new List<string>();

            var filter = options.Modules.ToHashSet();

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = $@"
SELECT SCHEMA_NAME(schema_id) AS [Schema], name AS [Name], object_id
FROM sys.objects 
WHERE objectproperty(OBJECT_ID,'IsScalarFunction') = 1
AND NULLIF([name], '') IS NOT NULL;";

                using (var command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            found.Add(new Tuple<string, string, int>(reader.GetString(0), reader.GetString(1), reader.GetInt32(2)));
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
                            IsScalar = true,
                        };

                        if (options.FullModel)
                        {
                            function.Parameters = GetFunctionParameters(connection, foundFunction.Item3);
                        }

                        result.Add(function);
                    }
                }
            }

            return new FunctionModel
            {
                Functions = result,
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
    }
}