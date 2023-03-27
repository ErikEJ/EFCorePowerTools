using System.Collections.Generic;
using System.Runtime.Serialization;
using RevEng.Common;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class ReverseEngineerOptions
    {
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
        public string ProjectRootNamespace { get; set; }
        public string ModelNamespace { get; set; }
        public string ContextNamespace { get; set; }
        public bool UseFluentApiOnly { get; set; } = true;
        public string ContextClassName { get; set; }
        public List<SerializationTableModel> Tables { get; set; }
        public bool UseDatabaseNames { get; set; }
        public bool UseInflector { get; set; }
        public List<string> UncountableWords { get; set; }
        public bool UseHandleBars { get; set; }
        public bool UseT4 { get; set; }
        public int SelectedHandlebarsLanguage { get; set; }
        public bool IncludeConnectionString { get; set; }
        public int SelectedToBeGenerated { get; set; }
        [IgnoreDataMember]
        public string Dacpac { get; set; }
        [IgnoreDataMember]
        public List<Schema> CustomReplacers { get; set; }
        [IgnoreDataMember]
        public Model CustomPropertyReplacers { get; set; }
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
        public bool UseManyToManyEntity { get; set; }
        public CodeGenerationMode CodeGenerationMode { get; set; } = CodeGenerationMode.EFCore6;
        public string UiHint { get; set; }
        public List<SchemaInfo> Schemas { get; set; }
        [IgnoreDataMember]
        public bool InstallNuGetPackage { get; set; }
        public bool PreserveCasingWithRegex { get; set; } = true;
        public bool UseDateOnlyTimeOnly { get; set; }
    }
}
