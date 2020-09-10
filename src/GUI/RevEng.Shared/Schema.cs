using System.Collections.Generic;

namespace ReverseEngineer20.ReverseEngineer
{
    public class Schema
    {
        public Schema()
        {
            Tables = new List<TableRenamer>();
        }
        public bool UseSchemaName { get; set; }

        public string SchemaName { get; set; }

        public List<TableRenamer> Tables { get; set; }

        public string TableRegexPattern { get; set; }

        public string TablePatternReplaceWith { get; set; }

        public string ColumnRegexPattern { get; set; }

        public string ColumnPatternReplaceWith { get; set; }
    }
}
