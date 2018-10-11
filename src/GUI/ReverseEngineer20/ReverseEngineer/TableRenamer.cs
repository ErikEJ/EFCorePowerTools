using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReverseEngineer20.ReverseEngineer
{
    public class TableRenamer
    {
        public  bool UseSchemaName { get; set; }

        public string SchemaName { get; set; }

        public string OldTableName { get; set; }

        public string NewTableName { get; set; }

        public List<ColumnNamer> CustomColumnsNames { get; set; }
    }
}
