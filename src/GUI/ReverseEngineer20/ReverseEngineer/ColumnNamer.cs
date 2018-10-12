using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReverseEngineer20.ReverseEngineer
{
    public class ColumnNamer
    {
        public string OldColumnName { get; set; }

        public string NewColumnName { get; set; }
    }
}
