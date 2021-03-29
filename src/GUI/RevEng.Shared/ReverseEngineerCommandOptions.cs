using System.Collections.Generic;

namespace RevEng.Shared
{
    public class ReverseEngineerCommandOptions
    {
        public DatabaseType DatabaseType { get; set; }
        public string ConnectionString { get; set; }
        public string ProjectPath { get; set; }
        public string OutputPath { get; set; }
        public string OutputContextPath { get; set; }
        public string ModelNamespace { get; set; }
        public string ContextNamespace { get; set; }
        public string ProjectRootNamespace { get; set; }
        public bool UseFluentApiOnly { get; set; }
        public string ContextClassName { get; set; }
        public List<SerializationTableModel> Tables { get; set; }
        public bool UseDatabaseNames { get; set; }
        public bool UseInflector { get; set; }
        public bool UseHandleBars { get; set; }
        public int SelectedHandlebarsLanguage { get; set; }
        public bool IncludeConnectionString { get; set; }
        public int SelectedToBeGenerated { get; set; }
        public string Dacpac { get; set; }
        public List<Schema> CustomReplacers { get; set; }
        public string DefaultDacpacSchema { get; set; }
        public bool UseLegacyPluralizer { get; set; }
        public bool UseSpatial { get; set; }
        public bool UseDbContextSplitting { get; set; }
        public bool UseNodaTime { get; set; }
        public bool UseBoolPropertiesWithoutDefaultSql { get; set; }
        public bool UseNullableReferences { get; set; }
        public bool UseNoConstructor { get; set; }
        public bool UseNoNavigations { get; set; }
        public bool UseNoObjectFilter { get; set; }
        public bool FilterSchemas { get; set; }
        public List<SchemaInfo> Schemas { get; set; }
        public bool ProceduresReturnList { get; set; }

    }
}