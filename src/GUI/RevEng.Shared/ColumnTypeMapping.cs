using System.Runtime.Serialization;

namespace RevEng.Shared
{
    [DataContract]
    public class ColumnTypeMapping
    {
        [DataMember]
        public string StoreType { get; set; }

        [DataMember]
        public string ClrType { get; set; }

        [DataMember]
        public bool IsInferred { get; set; }

        [DataMember]
        public bool? ScaffoldUnicode { get; set; }

        [DataMember]
        public bool? ScaffoldFixedLength { get; set; }

        [DataMember]
        public int? ScaffoldMaxLength { get; set; }

        [DataMember]
        public int? ScaffoldPrecision { get; set; }

        [DataMember]
        public int? ScaffoldScale { get; set; }
    }
}
