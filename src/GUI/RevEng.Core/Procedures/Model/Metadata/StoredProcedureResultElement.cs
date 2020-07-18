namespace RevEng.Core.Procedures.Model.Metadata
{
    public class StoredProcedureResultElement
    {
        public string Name { get; set; }
        public string StoreType { get; set; }
        public int Ordinal { get; set; }
        public bool Nullable { get; set; }        
    }
}
