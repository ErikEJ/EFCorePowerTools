using System.Runtime.Serialization;

namespace RevEng.Common
{
    [DataContract]
    public class ReplacementRule
    {
        [DataMember]
        public string Rule { get; set; }

        [DataMember]
        public string Replacement { get; set; }
    }
}