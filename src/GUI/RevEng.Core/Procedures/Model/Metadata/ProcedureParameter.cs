namespace RevEng.Core.Procedures.Model.Metadata
{
    public class ProcedureParameter
    {
        public string Name { get; set; }
        public string StoreType { get; set; }
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public int Ordinal { get; set; }
        public bool Output { get; set; }
        public bool Nullable { get; set; }
        public string TypeName { get; set; }
        
    }
}
