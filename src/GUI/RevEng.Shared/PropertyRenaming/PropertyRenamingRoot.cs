using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RevEng.Common.PropertyRenaming
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1724:", Justification = "Reviewed.")]
    [DataContract]
    public sealed class PropertyRenamingRoot
    {
        public PropertyRenamingRoot()
        {
            Classes = new List<ClassRenamer>();
        }
        
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string Namespace { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<ClassRenamer> Classes { get; set; }
    }
}
