using System.Runtime.Serialization;

namespace ReverseEngineer20.ReverseEngineer
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
