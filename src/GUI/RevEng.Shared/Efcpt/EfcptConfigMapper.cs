using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

namespace RevEng.Common.Efcpt
{
    public static class EfcptConfigMapper
    {
        public static ReverseEngineerCommandOptions ToOptions(this EfcptConfig config, string connectionString, string provider, string projectPath, bool isDacpac, string configPath)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentNullException(nameof(provider));
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

            var databaseType = Providers.GetDatabaseTypeFromProvider(provider, isDacpac);
            var fileLayout = config.FileLayout ?? new FileLayout();
            var replacements = config.Replacements ?? new Replacements();
            var typeMappings = config.TypeMappings ?? new TypeMappings();
            var names = config.Names ?? new Names();
            var dbContextDefaultName = names.DbContextName
                                       ?? GetDbContextNameSuggestion(connectionString, databaseType);

            return new ReverseEngineerCommandOptions
            {
                ConnectionString = connectionString,
                DatabaseType = databaseType,
                ProjectPath = projectPath,
                OutputPath = fileLayout.OutputPath,
                OutputContextPath = fileLayout.OutputDbContextPath,
                UseSchemaFolders = fileLayout.UseSchemaFoldersPreview,
                ModelNamespace = names.ModelNamespace,
                ContextNamespace = names.DbContextNamespace,
                UseFluentApiOnly = !config.CodeGeneration.UseDataAnnotations,
                ContextClassName = dbContextDefaultName,
                Tables = BuildObjectList(config),
                UseDatabaseNames = config.CodeGeneration.UseDatabaseNames,
                UseInflector = config.CodeGeneration.UseInflector,
                UseT4 = config.CodeGeneration.UseT4,
                IncludeConnectionString = config.CodeGeneration.EnableOnConfiguring,
                SelectedToBeGenerated = selectedToBeGenerated,
                Dacpac = isDacpac ? connectionString : null,
                CustomReplacers = GetNamingOptions(configPath),
                UseLegacyPluralizer = config.CodeGeneration.UseLegacyInflector,
                UncountableWords = replacements.UncountableWords?.ToList(),
                UseSpatial = typeMappings.UseSpatial,
                UseHierarchyId = typeMappings.UseHierarchyId,
                UseDbContextSplitting = fileLayout.SplitDbContextPreview,
                UseNodaTime = typeMappings.UseNodaTime,
                UseBoolPropertiesWithoutDefaultSql = config.CodeGeneration.RemoveDefaultSqlFromBoolProperties,
                UseNoDefaultConstructor = true, // TBD for EF Core 6
                RunCleanup = config.CodeGeneration.SoftDeleteObsoleteFiles,
                UseManyToManyEntity = config.CodeGeneration.UseManyToManyEntity,
                UseMultipleSprocResultSets = config.CodeGeneration.DiscoverMultipleStoredProcedureResultsetsPreview,
                UseLegacyResultSetDiscovery = config.CodeGeneration.UseAlternateStoredProcedureResultsetDiscovery,
                PreserveCasingWithRegex = replacements.PreserveCasingWithRegex,
                UseDateOnlyTimeOnly = typeMappings.UseDateOnlyTimeOnly,
                UseNullableReferences = config.CodeGeneration.UseNullableReferenceTypes,
                ProjectRootNamespace = names.RootNamespace,

                // Not supported/implemented:
                UseHandleBars = false,
                SelectedHandlebarsLanguage = 0, // handlebars support, will not support it
                OptionsPath = null, // handlebars support, will not support it
                LegacyLangVersion = false, // no 3.1 support
                MergeDacpacs = false, // not implemented, will consider if asked for
                DefaultDacpacSchema = null, // not implemented, will consider if asked for
                UseNoObjectFilter = false, // will always add all objects and use exclusions to filter list (for now)
                UseAsyncCalls = true, // not implemented, will consider if asked for
                FilterSchemas = false, // not implemented
                Schemas = null, // not implemented
            };
        }

        public static bool TryGetEfcptConfig(string fullPath, string connectionString, DatabaseType databaseType, List<TableModel> objects, out EfcptConfig config)
        {
            if (File.Exists(fullPath))
            {
                config = JsonSerializer.Deserialize<EfcptConfig>(File.ReadAllText(fullPath, Encoding.UTF8));
            }
            else
            {
                config = new EfcptConfig
                {
                    Names =
                    {
                        DbContextName = GetDbContextNameSuggestion(connectionString, databaseType),
                        RootNamespace = GetRootNamespaceSuggestion(fullPath),
                    },
                };
                // TODO Consider if more default values should be picked up from existing efpt.config.json
            }

            config.Tables = Add(objects, config.Tables);
            config.Views = Add(objects, config.Views);
            config.StoredProcedures = Add(objects, config.StoredProcedures);
            config.Functions = Add(objects, config.Functions);

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(fullPath, JsonSerializer.Serialize(config, options), Encoding.UTF8);

            return true;
        }

        private static List<T> Add<T>(IEnumerable<TableModel> models, List<T> entities)
            where T : IEntity, new()
        {
            var result = entities ?? new List<T>();
            var objectType = DefineObjectType<T>();
            var newItems = models.Where(o => o.ObjectType == objectType).ToList();
            if (newItems.Count == 0)
            {
                return result;
            }

            var ids = new HashSet<string>(newItems.Select(x => x.DisplayName));
            result.RemoveAll(x => !ids.Contains(x.Name));
            foreach (var table in from table in newItems let existing = result.SingleOrDefault(t => t.Name == table.DisplayName) where existing == null select table)
            {
                result.Add(new T { Name = table.DisplayName });
            }

            return result;
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

            Console.WriteLine(dir);

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

        private static List<SerializationTableModel> BuildObjectList(EfcptConfig config)
        {
            var objects = new List<SerializationTableModel>();

            void ToSerializationModel<T>(IEnumerable<T> entities, Action<IEnumerable<SerializationTableModel>> addRange)
                where T : IEntity, new()
            {
                var objectType = DefineObjectType<T>();
                addRange(from entity in entities where !entity.Exclude select new SerializationTableModel(entity.Name, objectType, null));
            }

            ToSerializationModel(config.Tables, objects.AddRange);
            ToSerializationModel(config.Views, objects.AddRange);
            ToSerializationModel(config.StoredProcedures, objects.AddRange);
            ToSerializationModel(config.Functions, objects.AddRange);

            return objects;
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
