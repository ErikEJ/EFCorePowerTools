namespace RevEng.Core.Abstractions.Metadata
{
    public class ModuleParameter
    {
        public string Name { get; set; }
        public string StoreType { get; set; }
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool Output { get; set; }
        public bool Nullable { get; set; }
        public string TypeName { get; set; }
        public bool IsReturnValue { get; set; }
    }
}
