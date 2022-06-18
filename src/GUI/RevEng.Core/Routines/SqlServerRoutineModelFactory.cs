using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RevEng.Core.Procedures
{
    public abstract class SqlServerRoutineModelFactory
    {
        public string RoutineType { get; set; }

        protected SqlServerRoutineModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _)
        {
        }

        protected RoutineModel GetRoutines(string connectionString, ModuleModelFactoryOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var result = new List<Routine>();
            var found = new List<Tuple<string, string, string, bool>>();
            var errors = new List<string>();

            var filter = options.Modules.ToHashSet();

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
#pragma warning disable CA1305 // Specify IFormatProvider
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
AND ROUTINE_TYPE = N'{RoutineType}'");
#pragma warning restore CA1305 // Specify IFormatProvider

#if !CORE50 && !CORE60
                // Filters out table-valued functions without filtering out stored procedures
                sql.AppendLine("AND COALESCE(DATA_TYPE, '') != 'TABLE'");
#endif

                sql.AppendLine("ORDER BY ROUTINE_NAME;");

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
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
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

                foreach (var foundModule in found)
                {
                    var key = $"[{foundModule.Item1}].[{foundModule.Item2}]";

                    if (filter.Count == 0 || filter.Contains(key))
                    {
                        var isScalar = foundModule.Item4;

                        var module = RoutineType == "PROCEDURE"
                            ? (Routine)new Procedure()
                            : new Function { IsScalar = isScalar };

                        module.Schema = foundModule.Item1;
                        module.Name = foundModule.Item2;
                        module.HasValidResultSet = true;

                        if (options.MappedModules?.ContainsKey(key) ?? false)
                        {
                            module.MappedType = options.MappedModules[key];
                        }

                        if (options.FullModel)
                        {
                            module.Parameters = GetParameters(connection, module.Schema, module.Name);

                            if (!isScalar)
                            {
#pragma warning disable CA1031 // Do not catch general exception types
                                try
                                {
                                    var forceLegacy = options.UseLegacyResultSetDiscovery;
                                    if (!forceLegacy)
                                    {
                                        forceLegacy = options.ModulesUsingLegacyDiscovery?.Contains($"[{module.Schema}].[{module.Name}]") ?? false;
                                    }
                                    
                                    module.Results.AddRange(GetResultElementLists(connection, module, options.DiscoverMultipleResultSets, forceLegacy));
                                }
                                catch (Exception ex)
                                {
                                    module.HasValidResultSet = false;
                                    module.Results = new List<List<ModuleResultElement>>
                                    {
                                        new List<ModuleResultElement>()
                                    };
                                    errors.Add($"Unable to get result set shape for {RoutineType} '{module.Schema}.{module.Name} - see the wiki for solutions.'{Environment.NewLine}{ex}{Environment.NewLine}");
                                }
#pragma warning restore CA1031 // Do not catch general exception types
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
            using var dtResult = new DataTable();
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
    'TypeName' = QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(TYPE_NAME(p.user_type_id)),
	'TypeSchema' = t.schema_id,
	'TypeId' = p.user_type_id
    from sys.parameters p
	LEFT JOIN sys.table_types t ON t.user_type_id = p.user_type_id
    where object_id = object_id('{schema}.{name}')
    ORDER BY parameter_id;";

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            using var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection)
            };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

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
                    Length = (par["Length"]is DBNull) ? (int?)null : int.Parse(par["Length"].ToString(), CultureInfo.InvariantCulture),
                    Precision = (par["Precision"] is DBNull) ? (int?)null : int.Parse(par["Precision"].ToString(), CultureInfo.InvariantCulture),
                    Scale = (par["Scale"] is DBNull) ? (int?)null : int.Parse(par["Scale"].ToString(), CultureInfo.InvariantCulture),
                    Output = (bool)par["output"],
                    Nullable = true,
                    TypeName = (par["TypeName"] is DBNull) ? par["Type"].ToString() : par["TypeName"].ToString(),
                    TypeId = (par["TypeId"] is DBNull) ? (int?)null : int.Parse(par["TypeId"].ToString(), CultureInfo.InvariantCulture),
                    TypeSchema = (par["TypeSchema"] is DBNull) ? (int?)null : int.Parse(par["TypeSchema"].ToString(), CultureInfo.InvariantCulture),
                };  

                result.Add(parameter);
            }

            return result;
        }

#pragma warning disable CA1716 // Identifiers should not match keywords
        protected abstract List<List<ModuleResultElement>> GetResultElementLists(SqlConnection connection,  Routine module, bool multipleResults, bool useLegacyResultSetDiscovery);
#pragma warning restore CA1716 // Identifiers should not match keywords
    }
}
