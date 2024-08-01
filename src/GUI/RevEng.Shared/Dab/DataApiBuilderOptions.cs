using System.Collections.Generic;

namespace RevEng.Common.Dab
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO")]
    public class DataApiBuilderOptions
    {
        public DatabaseType DatabaseType { get; set; }

        public string ConnectionString { get; set; }

        public string ProjectPath { get; set; }

        public List<SerializationTableModel> Tables { get; set; }

        public string Dacpac { get; set; }

        public string ConnectionStringName { get; set; } = "dab-connection-string";
    }
}
