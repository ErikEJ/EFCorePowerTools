using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RevEng.Common
{
    public class ReverseEngineerOptions
    {
#pragma warning disable CA2227 // Collection properties should be read only
        [IgnoreDataMember]
        public DatabaseType DatabaseType { get; set; }
        [IgnoreDataMember]
        public string ConnectionString { get; set; }
        [IgnoreDataMember]
        public string ProjectPath { get; set; }
        public string OutputPath { get; set; }
        [IgnoreDataMember]
        public string OptionsPath { get; set; }
        public string OutputContextPath { get; set; }
        public bool UseSchemaFolders { get; set; }
        public bool UseSchemaNamespaces { get; set; }
        public string ProjectRootNamespace { get; set; }
        public string ModelNamespace { get; set; }
        public string ContextNamespace { get; set; }
        public bool UseFluentApiOnly { get; set; } = true;
        public string ContextClassName { get; set; }
        public List<SerializationTableModel> Tables { get; set; }
        public bool UseDatabaseNames { get; set; }
        public bool UseInflector { get; set; } = true;
        public List<string> UncountableWords { get; set; }
        public bool UseHandleBars { get; set; }
        public bool UseT4 { get; set; }
        public int SelectedHandlebarsLanguage { get; set; } = 2;
        public bool IncludeConnectionString { get; set; }
        public int SelectedToBeGenerated { get; set; }
        [IgnoreDataMember]
        public string Dacpac { get; set; }
        [IgnoreDataMember]
        public List<Schema> CustomReplacers { get; set; }
        [IgnoreDataMember]
        public string DefaultDacpacSchema { get; set; }
        public bool UseLegacyPluralizer { get; set; }
        public bool UseSpatial { get; set; }
        public bool UseHierarchyId { get; set; }
        public bool UseDbContextSplitting { get; set; }
        public bool UseNodaTime { get; set; }
        public bool FilterSchemas { get; set; }
        public bool UseBoolPropertiesWithoutDefaultSql { get; set; }
        public bool UseNullableReferences { get; set; }
        public bool UseNoObjectFilter { get; set; }
        public bool UseNoDefaultConstructor { get; set; }
        public bool UseNoNavigations { get; set; }
        public bool UseManyToManyEntity { get; set; }
        public CodeGenerationMode CodeGenerationMode { get; set; } = CodeGenerationMode.EFCore7;
        public string UiHint { get; set; }
        public List<SchemaInfo> Schemas { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        [IgnoreDataMember]
        public bool InstallNuGetPackage { get; set; }
        public bool PreserveCasingWithRegex { get; set; } = true;
        public bool UseDateOnlyTimeOnly { get; set; }
        public string T4TemplatePath { get; set; }
        public bool UseDecimalDataAnnotationForSprocResult { get; set; } = true;
        public bool UsePrefixNavigationNaming { get; set; }
        public bool UseAsyncStoredProcedureCalls { get; set; } = true;
    }
}
