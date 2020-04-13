using ReverseEngineer20.ReverseEngineer;
using System.Collections.Generic;

namespace ReverseEngineer20
{
    using EFCorePowerTools.Shared.Models;

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
        public List<TableInformationModel> Tables { get; set; }
        public bool UseDatabaseNames { get; set; }
        public bool UseInflector { get; set; }
        public bool IdReplace { get; set; }
        public bool UseHandleBars { get; set; }
        public int SelectedHandlebarsLanguage { get; set; }
        public bool IncludeConnectionString { get; set; }
        public int SelectedToBeGenerated { get; set; }
        public string Dacpac { get; set; }
        public List<Schema> CustomReplacers { get; set; }
        public string DefaultDacpacSchema { get; set; }
        public bool UseLegacyPluralizer { get; set; }
        public bool DoNotCombineNamespace { get; set; }
        public bool UseSpatial { get; set; }
        public bool UseDbContextSplitting { get; set; }
    }
}