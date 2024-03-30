using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using RevEng.Common.Cli.Configuration;

namespace RevEng.Common.Cli
{
    public static class CliConfigMapper
    {
        public static ReverseEngineerCommandOptions ToOptions(this CliConfig config, string connectionString, DatabaseType databaseType, string projectPath, bool isDacpac, string configPath)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(projectPath))
            {
                throw new ArgumentNullException(nameof(projectPath));
            }

            if (string.IsNullOrEmpty(configPath))
            {
                throw new ArgumentNullException(nameof(configPath));
            }

            var selectedToBeGenerated = config.CodeGeneration.Type.ToUpperInvariant() switch
            {
                "DBCONTEXT" => 1,
                "ENTITIES" => 2,
                _ => 0, // "all"
            };

            var replacements = config.Replacements ?? new Replacements();
            var typeMappings = config.TypeMappings ?? new TypeMappings();
            var names = config.Names ?? new Names();
            var dbContextDefaultName = names.DbContextName
                                       ?? GetDbContextNameSuggestion(connectionString, databaseType);

            var options = new ReverseEngineerCommandOptions
            {
                ConnectionString = connectionString,
                DatabaseType = databaseType,
                ProjectPath = projectPath,
                ModelNamespace = names.ModelNamespace,
                ContextNamespace = names.DbContextNamespace,
                UseFluentApiOnly = !config.CodeGeneration.UseDataAnnotations,
                ContextClassName = dbContextDefaultName,
                Tables = BuildObjectList(config),
                UseDatabaseNames = config.CodeGeneration.UseDatabaseNames,
                UseInflector = config.CodeGeneration.UseInflector,
                UseT4 = config.CodeGeneration.UseT4,
                T4TemplatePath = config.CodeGeneration.T4TemplatePath != null ? PathHelper.GetAbsPath(config.CodeGeneration.T4TemplatePath, projectPath) : null,
                IncludeConnectionString = !isDacpac && config.CodeGeneration.EnableOnConfiguring,
                SelectedToBeGenerated = selectedToBeGenerated,
                Dacpac = isDacpac ? connectionString : null,
                CustomReplacers = GetNamingOptions(configPath),
                UseLegacyPluralizer = config.CodeGeneration.UseLegacyInflector,
                UncountableWords = replacements.UncountableWords?.ToList(),
                UseSpatial = typeMappings.UseSpatial,
                UseHierarchyId = typeMappings.UseHierarchyId,
                UseNodaTime = typeMappings.UseNodaTime,
                UseBoolPropertiesWithoutDefaultSql = config.CodeGeneration.RemoveDefaultSqlFromBoolProperties,
                UseNoNavigations = config.CodeGeneration.UseNoNavigationsPreview,
                RunCleanup = config.CodeGeneration.SoftDeleteObsoleteFiles,
                UseManyToManyEntity = config.CodeGeneration.UseManyToManyEntity,
                UseMultipleSprocResultSets = config.CodeGeneration.DiscoverMultipleStoredProcedureResultsetsPreview,
                UseLegacyResultSetDiscovery = config.CodeGeneration.UseAlternateStoredProcedureResultsetDiscovery,
                PreserveCasingWithRegex = replacements.PreserveCasingWithRegex,
                UseDateOnlyTimeOnly = typeMappings.UseDateOnlyTimeOnly,
                UseNullableReferences = config.CodeGeneration.UseNullableReferenceTypes,
                ProjectRootNamespace = names.RootNamespace,
                MergeDacpacs = config.CodeGeneration.MergeDacpacs,
                UseDecimalDataAnnotation = config.CodeGeneration.UseDecimalDataAnnotation,
                UsePrefixNavigationNaming = config.CodeGeneration.UsePrefixNavigationNaming,

                // Not supported:
                UseHandleBars = false,
                SelectedHandlebarsLanguage = 0, // handlebars support, will not support it
                OptionsPath = null, // handlebars support, will not support it

                // Not implemented:
                UseNoDefaultConstructor = false, // not implemented, will consider if asked for
                DefaultDacpacSchema = null, // not implemented, will consider if asked for
                UseNoObjectFilter = false, // will always add all objects and use exclusions to filter list (for now)
                UseAsyncCalls = true, // not implemented
                FilterSchemas = false, // not implemented
                Schemas = null, // not implemented
            };

            if (config.FileLayout is null)
            {
                return options;
            }

            options.OutputPath = config.FileLayout.OutputPath;
            options.OutputContextPath = config.FileLayout.OutputDbContextPath;
            options.UseSchemaFolders = config.FileLayout.UseSchemaFoldersPreview;
            options.UseSchemaNamespaces = config.FileLayout.UseSchemaNamespacesPreview;
            options.UseDbContextSplitting = config.FileLayout.SplitDbContextPreview;

            return options;
        }

        public static bool TryGetCliConfig(string fullPath, string connectionString, DatabaseType databaseType, List<TableModel> objects, CodeGenerationMode codeGenerationMode, out CliConfig config, out List<string> warnings)
        {
            var cliConfigExists = File.Exists(fullPath);
            if (cliConfigExists)
            {
                config = JsonSerializer.Deserialize<CliConfig>(File.ReadAllText(fullPath, Encoding.UTF8));
            }
            else
            {
                config = new CliConfig
                {
                    Names =
                    {
                        DbContextName = GetDbContextNameSuggestion(connectionString, databaseType),
                        RootNamespace = GetRootNamespaceSuggestion(fullPath),
                    },
                };

                if (codeGenerationMode == CodeGenerationMode.EFCore8)
                {
                    config.TypeMappings = new TypeMappings
                    {
                        UseDateOnlyTimeOnly = true,
                    };
                }
            }

            if (config.CodeGeneration.RefreshObjectLists)
            {
                config.Tables = Add(objects, config.Tables);
                config.Views = Add(objects, config.Views);
                config.StoredProcedures = Add(objects, config.StoredProcedures);
                config.Functions = Add(objects, config.Functions);
            }

            warnings = ValidateExcludedColumns(config, objects);

            if (!cliConfigExists || config.CodeGeneration.RefreshObjectLists)
            {
#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
                var options = new JsonSerializerOptions { WriteIndented = true };
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
                File.WriteAllText(fullPath, JsonSerializer.Serialize(config, options), Encoding.UTF8);
            }

            return true;
        }

        public static List<SerializationTableModel> BuildObjectList(CliConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var objects = new List<SerializationTableModel>();

            ToSerializationModel(config.Tables, objects.AddRange);
            ToSerializationModel(config.Views, objects.AddRange);
            ToSerializationModel(config.StoredProcedures, objects.AddRange);
            ToSerializationModel(config.Functions, objects.AddRange);

            return objects;
        }

        /// <summary>
        /// Ensures that any excluded columns for tables are not required. Removes the invalid columns from the list.
        /// </summary>
        /// <param name="config">Configuration to Validate.</param>
        /// <param name="objects">Table Models to check against.</param>
        private static List<string> ValidateExcludedColumns(CliConfig config, List<TableModel> objects)
        {
            List<string> warnings = new();

            var tables = config.Tables ?? new List<Table>();
            var views = config.Views ?? new List<View>();

            var objectsToCheck = tables.Where(x => x.ExcludedColumns?.Count > 0)
                .Select(table => table as IEntity)
                .Union(views.Where(x => x.ExcludedColumns?.Count > 0));

            foreach (var table in objectsToCheck)
            {
                var dbTable = objects.Single(x => x.DisplayName == table.Name);
                var columnsThatCannotBeExcluded = dbTable.Columns.Where(x => x.IsForeignKey || x.IsPrimaryKey).Select(x => x.Name);

                var badExclusions = columnsThatCannotBeExcluded.Intersect(table.ExcludedColumns);

                foreach (var column in badExclusions)
                {
                    var originalColumnString = table.ExcludedColumns.Single(x => string.Equals(x, column, StringComparison.Ordinal));
                    warnings.Add($"{table.Name}.{originalColumnString} cannot be excluded because it is either a Primary Key or Foreign Key of another Mapped Column.  This entry has been removed from the config file.");
                    table.ExcludedColumns.Remove(originalColumnString);
                }
            }

            return warnings;
        }

        private static void ToSerializationModel<T>(IEnumerable<T> entities, Action<IEnumerable<SerializationTableModel>> addRange)
            where T : IEntity, new()
        {
            if (entities is null)
            {
                return;
            }

            var objectType = DefineObjectType<T>();

            var excludeAll = entities.Any(t => t.ExclusionWildcard == "*");

            var filters = GetFilters(entities);

            var serializationTableModels = entities.Where<T>(entity => ExclusionFilter(entity, excludeAll, filters)
                && !string.IsNullOrEmpty(entity.Name))
                .Select(entity => new SerializationTableModel(entity.Name, objectType, entity.ExcludedColumns));
            addRange(serializationTableModels);
        }

        private static List<ExclusionFilter> GetFilters<T>(IEnumerable<T> entities)
            where T : IEntity, new()
        {
            var filters = new List<ExclusionFilter>();

            var candidates = entities.Where(t => !string.IsNullOrEmpty(t.ExclusionWildcard)
                && t.ExclusionWildcard != "*"
                && t.ExclusionWildcard.Contains('*')).ToList();

            foreach (var candiate in candidates)
            {
                if (candiate.ExclusionWildcard.StartsWith("*", StringComparison.OrdinalIgnoreCase)
                    && candiate.ExclusionWildcard.EndsWith("*", StringComparison.OrdinalIgnoreCase)
                    && candiate.ExclusionWildcard.Length > 2)
                {
                    filters.Add(new ExclusionFilter
                    {
                        Filter = candiate.ExclusionWildcard.Substring(0, candiate.ExclusionWildcard.Length - 1).Substring(1),
                        FilterType = ExclusionFilterType.Contains,
                    });
                    break;
                }

                if (candiate.ExclusionWildcard.StartsWith("*", StringComparison.OrdinalIgnoreCase))
                {
                    filters.Add(new ExclusionFilter
                    {
                        Filter = candiate.ExclusionWildcard.Substring(1),
                        FilterType = ExclusionFilterType.EndsWith,
                    });
                    break;
                }

                if (candiate.ExclusionWildcard.EndsWith("*", StringComparison.OrdinalIgnoreCase))
                {
                    filters.Add(new ExclusionFilter
                    {
                        Filter = candiate.ExclusionWildcard.Substring(0, candiate.ExclusionWildcard.Length - 1),
                        FilterType = ExclusionFilterType.StartsWith,
                    });
                    break;
                }
            }

            return filters;
        }

        private static bool ExclusionFilter<T>(T entity, bool excludeAll, List<ExclusionFilter> filters)
            where T : IEntity, new()
        {
            if (excludeAll)
            {
                return entity.Exclude.HasValue && !entity.Exclude.Value;
            }

            foreach (var filter in filters)
            {
                if (entity.Exclude.HasValue && !entity.Exclude.Value)
                {
                    return true;
                }

                if (filter.FilterType == ExclusionFilterType.StartsWith
                    && !string.IsNullOrEmpty(entity.Name)
                    && string.IsNullOrEmpty(entity.ExclusionWildcard)
                    && entity.Name.StartsWith(filter.Filter, StringComparison.Ordinal))
                {
                    return false;
                }

                if (filter.FilterType == ExclusionFilterType.EndsWith
                    && !string.IsNullOrEmpty(entity.Name)
                    && string.IsNullOrEmpty(entity.ExclusionWildcard)
                    && entity.Name.EndsWith(filter.Filter, StringComparison.Ordinal))
                {
                    return false;
                }

                if (filter.FilterType == ExclusionFilterType.Contains
                    && !string.IsNullOrEmpty(entity.Name)
                    && string.IsNullOrEmpty(entity.ExclusionWildcard)
                    && entity.Name.Contains(filter.Filter))
                {
                    return false;
                }
            }

            return !entity.Exclude ?? true;
        }

        private static List<T> Add<T>(IEnumerable<TableModel> models, List<T> entities)
            where T : IEntity, new()
        {
            var objectType = DefineObjectType<T>();
            var newItems = models.Where(o => o.ObjectType == objectType).ToList();
            if (newItems.Count == 0)
            {
#pragma warning disable S1168
                return null;
#pragma warning restore S1168
            }

            var result = entities ?? new List<T>();
            var newIds = new HashSet<string>(newItems.Select(x => x.DisplayName));
            result.RemoveAll(x => !newIds.Contains(x.Name) && string.IsNullOrEmpty(x.ExclusionWildcard));
            foreach (var displayName in newItems.Select(t => t.DisplayName))
            {
                T existing = result.SingleOrDefault(t => t.Name == displayName);
                if (Equals(existing, default(T)))
                {
                    result.Add(new T { Name = displayName });
                }
            }

            return result.Count > 0 ? result : null;
        }

        private static ObjectType DefineObjectType<T>()
            where T : IEntity, new()
        {
            var instance = new T();
            ObjectType objectType = instance switch
            {
                View _ => ObjectType.View,
                Function _ => ObjectType.ScalarFunction,
                StoredProcedure _ => ObjectType.Procedure,
                _ => ObjectType.Table,
            };
            return objectType;
        }

        private static string GetRootNamespaceSuggestion(string fullPath)
        {
            var legacyConfig = GetLegacyOptions(fullPath);

            if (legacyConfig != null && !string.IsNullOrWhiteSpace(legacyConfig.ProjectRootNamespace))
            {
                return legacyConfig.ProjectRootNamespace;
            }

            var dir = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrEmpty(dir))
            {
                var csprojFiles = Directory.GetFiles(dir, "*.csproj");

                if (csprojFiles.Length == 1)
                {
                    return Path.GetFileNameWithoutExtension(csprojFiles[0]);
                }

                var info = new DirectoryInfo(dir);
                return info.Name;
            }

            return "RootNamespace";
        }

        private static string GetDbContextNameSuggestion(string connectionString, DatabaseType databaseType)
        {
            return DbContextNamer.GetDatabaseName(connectionString, databaseType) + "Context";
        }

        private static List<Schema> GetNamingOptions(string configPath)
        {
            var path = Path.Combine(Path.GetDirectoryName(configPath), "efpt.renaming.json");
            if (!File.Exists(path))
            {
                return new List<Schema>();
            }

            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(path, Encoding.UTF8)));
            var ser = new DataContractJsonSerializer(typeof(List<Schema>));
            return ser.ReadObject(ms) as List<Schema>;
        }

        private static ReverseEngineerCommandOptions GetLegacyOptions(string configPath)
        {
            var path = Path.Combine(Path.GetDirectoryName(configPath), "efpt.config.json");
            if (!File.Exists(path))
            {
                return null;
            }

            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(path, Encoding.UTF8)));
            var ser = new DataContractJsonSerializer(typeof(ReverseEngineerCommandOptions));
            return ser.ReadObject(ms) as ReverseEngineerCommandOptions;
        }
    }
}
