using System.Runtime.Serialization;

namespace RevEng.Shared
{
    [DataContract]
    public class NavigationRenamer
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NewName { get; set; }
    }
}
