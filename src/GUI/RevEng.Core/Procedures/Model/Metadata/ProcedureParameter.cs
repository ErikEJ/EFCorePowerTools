namespace RevEng.Core.Procedures.Model.Metadata
{
    public class ProcedureParameter
    {
        public string Name { get; set; }
        public string StoreType { get; set; }
        public int? Length { get; set; }
        public byte? Precision { get; set; }
        public byte? Scale { get; set; }
        public int Ordinal { get; set; }
        public bool Output { get; set; }
        public bool Nullable { get; set; }        
    }
}
