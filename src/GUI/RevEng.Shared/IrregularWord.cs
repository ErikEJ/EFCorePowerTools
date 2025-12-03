using System.Runtime.Serialization;

namespace RevEng.Common
{
    [DataContract]
    public class IrregularWord
    {
        [DataMember]
        public string Singular { get; set; }

        [DataMember]
        public string Plural { get; set; }

        [DataMember]
        public bool MatchEnding { get; set; } = true;
    }
}