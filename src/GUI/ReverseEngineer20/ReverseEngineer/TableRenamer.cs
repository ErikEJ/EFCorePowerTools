using System.Collections.Generic;

namespace ReverseEngineer20.ReverseEngineer
{
    public class TableRenamer
    {
        public string Name { get; set; }

        public string NewName { get; set; }

        public List<ColumnNamer> Columns { get; set; }
    }
}
