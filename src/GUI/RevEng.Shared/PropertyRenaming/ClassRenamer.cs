using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RevEng.Common.PropertyRenaming
{
    [DataContract]
    public sealed class ClassRenamer
    {
        public ClassRenamer()
        {
            Properties = new List<PropertyRenamer>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<PropertyRenamer> Properties { get; set; }
    }
}
