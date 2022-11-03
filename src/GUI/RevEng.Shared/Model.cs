using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RevEng.Common
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1724:", Justification = "Reviewed.")]
    [DataContract]
    public class Model
    {
        public Model()
        {
            Classes = new List<ClassRenamer>();
        }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<ClassRenamer> Classes { get; set; }
    }
}
