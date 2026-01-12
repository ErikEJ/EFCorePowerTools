namespace RevEng.Core.Abstractions.Metadata
{
    public class ModuleParameterTvpColumn
    {
        public string Name { get; set; }

        public string DataType { get; set; }

        public int? MaxLength { get; set; }

        public int? Precision { get; set; }

        public bool Nullable { get; set; }
    }
}