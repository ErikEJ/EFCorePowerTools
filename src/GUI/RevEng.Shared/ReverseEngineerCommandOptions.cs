using System.Collections.Generic;

namespace RevEng.Common
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO")]
    public class ReverseEngineerCommandOptions
    {
        public DatabaseType DatabaseType { get; set; }

        public string ConnectionString { get; set; }

        public string ProjectPath { get; set; }

        public string OutputPath { get; set; }

        public string OptionsPath { get; set; }

        public string OutputContextPath { get; set; }

        public bool UseSchemaFolders { get; set; }
        public bool UseSchemaNamespaces { get; set; }

        public string ModelNamespace { get; set; }
        public string ContextNamespace { get; set; }
        public string ProjectRootNamespace { get; set; }
        public bool UseFluentApiOnly { get; set; }
        public string ContextClassName { get; set; }
        public List<SerializationTableModel> Tables { get; set; }
        public bool UseDatabaseNames { get; set; }
        public bool UseInflector { get; set; }
        public bool UseHandleBars { get; set; }
        public bool UseT4 { get; set; }
        public string T4TemplatePath { get; set; }
        public int SelectedHandlebarsLanguage { get; set; }
        public bool IncludeConnectionString { get; set; }
        public int SelectedToBeGenerated { get; set; }
        public string Dacpac { get; set; }
        public List<Schema> CustomReplacers { get; set; }
        public string DefaultDacpacSchema { get; set; }
        public bool UseLegacyPluralizer { get; set; }
        public List<string> UncountableWords { get; set; }
        public bool UseSpatial { get; set; }
        public bool UseHierarchyId { get; set; }
        public bool UseDbContextSplitting { get; set; }
        public bool UseNodaTime { get; set; }
        public bool UseBoolPropertiesWithoutDefaultSql { get; set; }
        public bool UseNullableReferences { get; set; }
        public bool UseNoObjectFilter { get; set; }
        public bool UseNoNavigations { get; set; }
        public bool UseNoDefaultConstructor { get; set; }
        public bool FilterSchemas { get; set; }
        public List<SchemaInfo> Schemas { get; set; }
        public bool RunCleanup { get; set; }
        public bool UseManyToManyEntity { get; set; }
        public bool UseMultipleSprocResultSets { get; set; }
        public bool MergeDacpacs { get; set; }
        public bool UseLegacyResultSetDiscovery { get; set; }
        public bool UseAsyncCalls { get; set; }
        public bool UseDecimalDataAnnotation { get; set; }
        public bool PreserveCasingWithRegex { get; set; }
        public bool UseDateOnlyTimeOnly { get; set; }
        public bool UsePrefixNavigationNaming { get; set; }
    }
}
