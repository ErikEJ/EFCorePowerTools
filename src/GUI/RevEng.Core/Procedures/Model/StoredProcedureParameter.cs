using System;
using System.Data;

namespace RevEng.Core.Procedures.Model
{
    public class StoredProcedureParameter
    {
        public string Name { get; set; }
        public int? Length { get; set; }
        public byte? Precision { get; set; }
        public byte? Scale { get; set; }
        public int Order { get; set; }
        public bool Output { get; set; }
        public bool Nullable { get; set; }        
        public Type ClrType { get; set; }
        public SqlDbType SqlDbType { get; set; }
    }
}
