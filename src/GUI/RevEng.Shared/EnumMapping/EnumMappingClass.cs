using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RevEng.Common.EnumMapping
{
    [DataContract]
    public sealed class EnumMappingClass
    {
        public EnumMappingClass()
        {
            Properties = new List<EnumMappingProperty>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<EnumMappingProperty> Properties { get; set; }
    }
}
