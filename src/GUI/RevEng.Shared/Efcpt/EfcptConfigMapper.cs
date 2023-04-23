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
        public static ReverseEngineerCommandOptions ToOptions(this EfcptConfig config, string connectionString, string provider, string projectPath, bool isDacpac)
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

            int selectedToBegenerated = 0; // "all"
            if (config.codegeneration.type == "dbcontext")
            {
                selectedToBegenerated = 1;
            }

            if (config.codegeneration.type == "entities")
            {
                selectedToBegenerated = 2;
            }

            var databaseType = Providers.GetDatabaseTypeFromProvider(provider, isDacpac);

            var dbContextDefaultName = config.names?.dbcontextname
                ?? GetDbContextNameSuggestion(connectionString, databaseType);

            return new ReverseEngineerCommandOptions
            {
                ConnectionString = connectionString,
                DatabaseType = databaseType,
                ProjectPath = projectPath,
                OutputPath = config.filelayout?.outputpath,
                OutputContextPath = config.filelayout?.outputdbcontextpath,
                UseSchemaFolders = config.filelayout?.useschemafolderspreview ?? false,
                ModelNamespace = config.names?.modelnamespace,
                ContextNamespace = config.names?.dbcontextnamespace,
                UseFluentApiOnly = !config.codegeneration.usedataannotations,
                ContextClassName = config.names?.dbcontextname ?? dbContextDefaultName,
                Tables = BuildObjectList(config),
                UseDatabaseNames = config.codegeneration.usedatabasenames,
                UseInflector = config.codegeneration.useinflector,
                UseT4 = config.codegeneration.uset4,
                IncludeConnectionString = config.codegeneration.enableonconfiguring,
                SelectedToBeGenerated = selectedToBegenerated,
                Dacpac = isDacpac ? connectionString : null,
                CustomReplacers = GetNamingOptions(projectPath),
                UseLegacyPluralizer = config.codegeneration.uselegacyinflector,
                UncountableWords = config.replacements?.uncountablewords.ToList(),
                UseSpatial = config.typemappings?.usespatial ?? false,
                UseHierarchyId = config.typemappings?.useHierarchyId ?? false,
                UseDbContextSplitting = config.filelayout?.splitdbcontextpreview ?? false,
                UseNodaTime = config.typemappings?.useNodaTime ?? false,
                UseBoolPropertiesWithoutDefaultSql = config.codegeneration.removedefaultsqlfromboolproperties,
                UseNoDefaultConstructor = true, // TBD for EF Core 6
                RunCleanup = config.codegeneration.softdeleteobsoletefiles,
                UseManyToManyEntity = config.codegeneration.usemanytomanyentity,
                UseMultipleSprocResultSets = config.codegeneration.discovermultiplestoredprocedureresultsetspreview,
                UseLegacyResultSetDiscovery = config.codegeneration.usealternatestoredprocedureresultsetdiscovery,
                PreserveCasingWithRegex = config.replacements?.preservecasingwithregex ?? false,
                UseDateOnlyTimeOnly = config.typemappings?.useDateOnlyTimeOnly ?? false,
                UseNullableReferences = config.codegeneration.usenullablereferencetypes,

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
                ProjectRootNamespace = null,
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
                config = new EfcptConfig();
                config.names.dbcontextname = GetDbContextNameSuggestion(connectionString, databaseType);
                config.names.rootnamespace = GetRootNamespace(fullPath);
            }

            // TODO Merge objects

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(fullPath, JsonSerializer.Serialize(config, options), Encoding.UTF8);

            return true;
        }

        private static string GetRootNamespace(string fullPath)
        {
            // TODO Improve by looking for a single .csproj in current folder, and suggest it's name
            var dir = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrEmpty(dir))
            {
                var info = new DirectoryInfo(dir);
                return info.Name;
            }

            return "Project";
        }

        private static List<SerializationTableModel> BuildObjectList(EfcptConfig config)
        {
            var objects = new List<SerializationTableModel>();

            if (config.tables != null)
            {
                foreach (var table in config.tables)
                {
                    if (!table.exclude)
                    {
                        objects.Add(new SerializationTableModel(table.name, ObjectType.Table, null));
                    }
                }
            }

            if (config.views != null)
            {
                foreach (var view in config.views)
                {
                    if (!view.exclude)
                    {
                        objects.Add(new SerializationTableModel(view.name, ObjectType.View, null));
                    }
                }
            }

            if (config.storedprocedures != null)
            {
                foreach (var sproc in config.storedprocedures)
                {
                    if (!sproc.exclude)
                    {
                        var proc = new SerializationTableModel(sproc.name, ObjectType.Procedure, null);
                        proc.MappedType = sproc.mappedtype;
                        proc.UseLegacyResultSetDiscovery = sproc.uselegacyresultsetdiscovery;

                        objects.Add(proc);
                    }
                }
            }

            if (config.functions != null)
            {
                foreach (var function in config.functions)
                {
                    if (!function.exclude)
                    {
                        var func = new SerializationTableModel(function.name, ObjectType.ScalarFunction, null);

                        objects.Add(func);
                    }
                }
            }

            return objects;
        }

        private static string GetDbContextNameSuggestion(string connectionString, DatabaseType databaseType)
        {
            return DbContextNamer.GetDatabaseName(connectionString, databaseType) + "Context";
        }

        private static List<Schema> GetNamingOptions(string optionsPath)
        {
            var optionsCustomNamePath = Path.Combine(Path.GetDirectoryName(optionsPath), "efpt.renaming.json");

            if (!File.Exists(optionsCustomNamePath))
            {
                return new List<Schema>();
            }

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(optionsCustomNamePath, Encoding.UTF8))))
            {
                var ser = new DataContractJsonSerializer(typeof(List<Schema>));
                return ser.ReadObject(ms) as List<Schema>;
            }
        }
    }
}
