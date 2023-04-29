namespace RevEng.Core.Abstractions.Metadata
{
    public class ModuleResultElement
    {
        public string Name { get; set; }
        public string StoreType { get; set; }
        public int Ordinal { get; set; }
        public bool Nullable { get; set; }
        public short? Precision { get; set; }
        public short? Scale { get; set; }
    }
}
