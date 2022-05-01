using System.Runtime.Serialization;

namespace RevEng.Common
{
    [DataContract]
    public class ColumnNamer
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NewName { get; set; }
    }
}
