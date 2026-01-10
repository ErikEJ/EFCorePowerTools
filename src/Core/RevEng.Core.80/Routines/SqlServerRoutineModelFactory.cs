using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Extensions;

namespace RevEng.Core.Routines
{
    public abstract class SqlServerRoutineModelFactory
    {
        public string RoutineSql { get; set; }

        public string RoutineType { get; set; }

#pragma warning disable CA1716 // Identifiers should not match keywords
        protected abstract List<List<ModuleResultElement>> GetResultElementLists(SqlConnection connection, Routine module, bool multipleResults, bool useLegacyResultSetDiscovery);
#pragma warning restore CA1716 // Identifiers should not match keywords

        protected RoutineModel GetRoutines(string connectionString, ModuleModelFactoryOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            SqlServerSqlTypeExtensions.UseDateOnlyTimeOnly = options.UseDateOnlyTimeOnly;

            var result = new List<Routine>();
            var found = new List<Tuple<string, string, bool>>();
            var errors = new List<string>();

            var filter = options.Modules.ToHashSet();

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.AppendLine(RoutineSql);

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                using (var command = new SqlCommand(sql.ToString(), connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Schema, Name, IsScalar
                            found.Add(new Tuple<string, string, bool>(reader.GetString(0), reader.GetString(1), reader.GetBoolean(2)));
                        }
                    }
                }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

                var allParameters = options.FullModel && found.Count != 0 ? GetParameters(connection) : new Dictionary<string, List<ModuleParameter>>();

                foreach (var foundModule in found)
                {
                    var key = $"[{foundModule.Item1}].[{foundModule.Item2}]";

                    var alternateKey = $"{foundModule.Item1}.{foundModule.Item2}";

                    if (filter.Count == 0
                        || filter.Contains(key)
                        || filter.Contains(alternateKey))
                    {
                        var isScalar = foundModule.Item3;

                        var module = RoutineType == "PROCEDURE"
                            ? (Routine)new Procedure()
                            : new Function { IsScalar = isScalar };

                        module.Schema = foundModule.Item1;
                        module.Name = foundModule.Item2;

                        if (options.FullModel)
                        {
                            module.HasValidResultSet = true;

                            if (options.MappedModules?.ContainsKey(key) ?? false)
                            {
                                module.MappedType = options.MappedModules[key];
                            }

                            if (allParameters.TryGetValue($"[{module.Schema}].[{module.Name}]", out var moduleParameters))
                            {
                                module.Parameters = moduleParameters;
                            }

                            if (RoutineType == "PROCEDURE")
                            {
                                module.Parameters.Add(GetReturnParameter());
                            }

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
                                        new List<ModuleResultElement>(),
                                    };
#pragma warning disable CA1308 // Normalize strings to uppercase
                                    errors.Add($"Unable to get result set shape for {RoutineType.ToLower(CultureInfo.InvariantCulture)} '{module.Schema}.{module.Name}'. {ex.Message}.");
#pragma warning restore CA1308 // Normalize strings to uppercase
                                }
#pragma warning restore CA1031 // Do not catch general exception types
                            }

                            if (module is Function func
                                && func.IsScalar
                                && func.Parameters.Count > 0
                                && func.Parameters.Exists(p => p.StoreType == "table type"))
                            {
                                errors.Add($"Unable to scaffold {RoutineType} '{module.Schema}.{module.Name}' as it has TVP parameters.");
                                continue;
                            }

                            bool dupesFound = false;
                            foreach (var resultElement in module.Results)
                            {
                                var duplicates = resultElement.GroupBy(x => x.Name)
                                    .Where(g => g.Count() > 1)
                                    .Select(y => y.Key)
                                    .ToList();

                                if (duplicates.Count != 0)
                                {
                                    dupesFound = true;
                                    errors.Add($"Unable to scaffold {RoutineType} '{module.Schema}.{module.Name}' as it has duplicate result column names: '{duplicates[0]}'.");
                                }
                            }

                            if (module.UnnamedColumnCount > 0)
                            {
                                errors.Add($"{RoutineType} '{module.Schema}.{module.Name}' has {module.UnnamedColumnCount} un-named columns.");
                            }

                            if (dupesFound)
                            {
                                continue;
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

        private static string GetVersion(SqlConnection connection)
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT @@VERSION;";
            var result = command.ExecuteScalar();
            return result as string;
        }

        private static Dictionary<string, List<ModuleParameter>> GetParameters(SqlConnection connection)
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
	'TypeId' = p.user_type_id,
    'RoutineName' = OBJECT_NAME(p.object_id),
    'RoutineSchema' = OBJECT_SCHEMA_NAME(p.object_id)
    from sys.parameters p
	LEFT JOIN sys.table_types t ON t.user_type_id = p.user_type_id
    ORDER BY p.object_id, p.parameter_id;";

            if (GetVersion(connection) == "Microsoft SQL Kusto")
            {
                sql = $@"
    SELECT  
        'Parameter' = p.name,  
        'Type'   = COALESCE(ts.name, tu.name),
        'Length'   = CAST(p.max_length AS INT),  
        'Precision'   = CASE 
                  WHEN ts.name = 'uniqueidentifier' THEN p.precision  
                  WHEN ts.name IN ('decimal', 'numeric') THEN p.precision
                  WHEN ts.name IN ('varchar', 'nvarchar') THEN p.max_length
                  ELSE NULL
                END, 
        'Scale'   = CAST(p.scale AS INT),  
        'Order'  = CAST(p.parameter_id AS INT),  
        p.is_output AS output,
        'TypeName' = QUOTENAME(s.name) + '.' + QUOTENAME(tu.name),
	    'TypeSchema' = tu.schema_id,
	    'TypeId' = p.user_type_id,
        'RoutineName' = o.name,
        'RoutineSchema' = s.name
        from sys.parameters p
        inner join sys.objects AS o on o.object_id = p.object_id
        inner JOIN sys.schemas AS s ON o.schema_id = s.schema_id
        inner join sys.types tu ON p.user_type_id = tu.user_type_id 
        LEFT JOIN sys.types ts ON tu.system_type_id = ts.user_type_id
        ORDER BY p.object_id, p.parameter_id;
    ";
            }

            using var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection),
            };

            adapter.Fill(dtResult);

            foreach (DataRow par in dtResult.Rows)
            {
                if (par != null)
                {
                    var parameterName = par["Parameter"].ToString();
                    if (parameterName!.StartsWith('@'))
                    {
                        parameterName = parameterName.Substring(1);
                    }

                    var parameter = new ModuleParameter()
                    {
                        Name = parameterName,
                        RoutineName = par["RoutineName"].ToString(),
                        RoutineSchema = par["RoutineSchema"].ToString(),
                        StoreType = par["Type"].ToString(),
                        Length = (par["Length"] is DBNull) ? (int?)null : int.Parse(par["Length"].ToString()!, CultureInfo.InvariantCulture),
                        Precision = (par["Precision"] is DBNull) ? (int?)null : int.Parse(par["Precision"].ToString()!, CultureInfo.InvariantCulture),
                        Scale = (par["Scale"] is DBNull) ? (int?)null : int.Parse(par["Scale"].ToString()!, CultureInfo.InvariantCulture),
                        Output = (bool)par["output"],
                        Nullable = true,
                        TypeName = (par["TypeName"] is DBNull) ? par["Type"].ToString() : par["TypeName"].ToString(),
                        TypeId = (par["TypeId"] is DBNull) ? (int?)null : int.Parse(par["TypeId"].ToString()!, CultureInfo.InvariantCulture),
                        TypeSchema = (par["TypeSchema"] is DBNull) ? (int?)null : int.Parse(par["TypeSchema"].ToString()!, CultureInfo.InvariantCulture),
                    };

                    result.Add(parameter);
                }
            }

            // Populate TVP column information for table-valued parameters
            PopulateTvpColumns(connection, result);

            return result.GroupBy(x => $"[{x.RoutineSchema}].[{x.RoutineName}]").ToDictionary(g => g.Key, g => g.ToList(), StringComparer.InvariantCulture);
        }

        private static void PopulateTvpColumns(SqlConnection connection, List<ModuleParameter> parameters)
        {
            // Based on https://stackoverflow.com/a/46079868/183934
            var tvpColumnsSql = @"
SELECT 
    SC.name AS ColumnName, 
    ST.name AS DataType,
    TT.user_type_id AS TypeId,
    TT.schema_id AS SchemaId
FROM sys.columns SC
INNER JOIN sys.types ST ON ST.system_type_id = SC.system_type_id AND ST.is_user_defined = 0
INNER JOIN sys.table_types TT ON TT.type_table_object_id = SC.object_id
WHERE ST.name <> 'sysname'
ORDER BY TT.user_type_id, SC.column_id;";

            using var dtTvpColumns = new DataTable();
            using var tvpAdapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(tvpColumnsSql, connection),
            };

            tvpAdapter.Fill(dtTvpColumns);

            // Group TVP columns by TypeId and SchemaId
            var tvpColumnsDict = new Dictionary<(int TypeId, int SchemaId), List<ModuleParameterTvpColumn>>();

            foreach (DataRow row in dtTvpColumns.Rows)
            {
                if (row != null)
                {
                    var typeId = int.Parse(row["TypeId"].ToString()!, CultureInfo.InvariantCulture);
                    var schemaId = int.Parse(row["SchemaId"].ToString()!, CultureInfo.InvariantCulture);
                    var key = (typeId, schemaId);

                    if (!tvpColumnsDict.ContainsKey(key))
                    {
                        tvpColumnsDict[key] = new List<ModuleParameterTvpColumn>();
                    }

                    tvpColumnsDict[key].Add(new ModuleParameterTvpColumn
                    {
                        Name = row["ColumnName"].ToString(),
                        DataType = row["DataType"].ToString(),
                    });
                }
            }

            // Assign TVP columns to parameters
            foreach (var parameter in parameters)
            {
                if (parameter.TypeId.HasValue && parameter.TypeSchema.HasValue)
                {
                    var key = (parameter.TypeId.Value, parameter.TypeSchema.Value);
                    if (tvpColumnsDict.TryGetValue(key, out var columns))
                    {
                        parameter.TvpColumns = columns;
                    }
                }
            }
        }

        private static ModuleParameter GetReturnParameter()
        {
            // Add parameter to hold the standard return value
            return new ModuleParameter()
            {
                Name = "returnValue",
                StoreType = "int",
                Output = true,
                Nullable = false,
                IsReturnValue = true,
            };
        }
    }
}