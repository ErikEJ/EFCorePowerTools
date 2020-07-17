using System;
using System.Data;

namespace RevEng.Core.Procedures.Model
{
    public class StoredProcedureResultElement
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public bool Nullable { get; set; }        
        public Type ClrType { get; set; }
        public SqlDbType SqlDbType { get; set; }
    }
}
