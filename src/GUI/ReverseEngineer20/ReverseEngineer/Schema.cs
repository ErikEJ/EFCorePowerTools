﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
