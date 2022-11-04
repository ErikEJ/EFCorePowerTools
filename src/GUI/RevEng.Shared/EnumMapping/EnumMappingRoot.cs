using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RevEng.Common.EnumMapping
{
    [DataContract]
    public sealed class EnumMappingRoot
    {
        public EnumMappingRoot()
        {
            Classes = new List<EnumMappingClass>();
        }
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string Namespace { get; set; }
        
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<EnumMappingClass> Classes { get; set; }
    }
}
