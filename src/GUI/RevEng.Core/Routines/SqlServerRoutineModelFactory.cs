using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RevEng.Core.Procedures
{
    public abstract class SqlServerRoutineModelFactory
    {
        protected readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        protected SqlServerRoutineModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
        }

        protected RoutineModel GetRoutines(string connectionString, ModuleModelFactoryOptions options)
        {
            var routineType = this switch
            {
                SqlServerStoredProcedureModelFactory _ => "PROCEDURE",
                SqlServerFunctionModelFactory _ => "FUNCTION",
                _ => throw new InvalidOperationException($"Unknown type '{GetType().Name}'"),
            };

            var result = new List<Routine>();
            var found = new List<Tuple<string, string, string, bool>>();
            var errors = new List<string>();

            var filter = options.Modules.ToHashSet();

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.AppendLine($@"
SELECT
    ROUTINE_SCHEMA,
    ROUTINE_NAME,
    ROUTINE_TYPE,
    CAST(CASE WHEN (ROUTINE_TYPE = 'FUNCTION' AND DATA_TYPE != 'TABLE') THEN 1 ELSE 0 END AS bit) AS IS_SCALAR
FROM INFORMATION_SCHEMA.ROUTINES
WHERE NULLIF(ROUTINE_NAME, '') IS NOT NULL
AND OBJECTPROPERTY(OBJECT_ID(QUOTENAME(ROUTINE_SCHEMA) + '.' + QUOTENAME(ROUTINE_NAME)), 'IsMSShipped') = 0
AND (
            select 
                major_id 
            from 
                sys.extended_properties 
            where 
                major_id = object_id(QUOTENAME(ROUTINE_SCHEMA) + '.' + QUOTENAME(ROUTINE_NAME)) and 
                minor_id = 0 and 
                class = 1 and 
                name = N'microsoft_database_tools_support'
        ) IS NULL 
AND ROUTINE_TYPE = N'{routineType}'");

#if !CORE50 && !CORE60
                // Filters out table-valued functions without filtering out stored procedures
                sql.AppendLine("AND COALESCE(DATA_TYPE, '') != 'TABLE'");
#endif

                sql.AppendLine("ORDER BY ROUTINE_NAME;");

                using (var command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Schema, Name, Type, IsScalar
                            found.Add(new Tuple<string, string, string, bool>(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3)));
                        }
                    }
                }

                foreach (var foundModule in found)
                {
                    if (filter.Count == 0 || filter.Contains($"[{foundModule.Item1}].[{foundModule.Item2}]"))
                    {
                        var isScalar = foundModule.Item4;

                        var module = routineType == "procedure"
                            ? (Routine)new Procedure()
                            : new Function { IsScalar = isScalar };

                        module.Schema = foundModule.Item1;
                        module.Name = foundModule.Item2;
                        module.HasValidResultSet = true;

                        if (options.FullModel)
                        {
                            module.Parameters = GetParameters(connection, module.Schema, module.Name);

                            if (!isScalar)
                            {
                                try
                                {
                                    module.Results.AddRange(GetResultElementLists(connection, module, options.DiscoverMultipleResultSets));
                                }
                                catch (Exception ex)
                                {
                                    module.HasValidResultSet = false;
                                    errors.Add($"Unable to get result set shape for {routineType} '{module.Schema}.{module.Name}'{Environment.NewLine}{ex.Message}{Environment.NewLine}");
                                    _logger?.Logger.LogWarning(ex, $"Unable to scaffold {module.Schema}.{module.Name}");
                                }
                            }
                        }

                        result.Add(module);
                    }
                }
            }

            return new RoutineModel
            {
                Routines = result,
                Errors = errors,
            };
        }

        protected virtual List<ModuleParameter> GetParameters(SqlConnection connection, string schema, string name)
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

            return result;
        }

        protected abstract List<List<ModuleResultElement>> GetResultElementLists(SqlConnection connection,  Routine module, bool multipleResults = false);
    }
}
