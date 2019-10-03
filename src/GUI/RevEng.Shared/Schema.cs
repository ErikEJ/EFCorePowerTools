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
    }
}
