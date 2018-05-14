using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReverseEngineer20
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
        public string ProjectRootNamespace { get; set; }
        public bool UseFluentApiOnly { get; set; }
        public string ContextClassName { get; set; }
        public List<string> Tables { get; set; }
        public bool UseDatabaseNames { get; set; }
        public bool UseInflector { get; set; }
        public bool IdReplace { get; set; }
        public bool UseHandleBars { get; set; }
        public bool IncludeConnectionString { get; set; }
        public int SelectedToBeGenerated { get; set; }
        [IgnoreDataMember]
        public string Dacpac { get; set; }
        public string DefaultDacpacSchema { get; set; }
    }
}