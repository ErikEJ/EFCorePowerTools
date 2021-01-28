using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RevEng.Shared
{
    [DataContract]
    public class Schema
    {
        public Schema()
        {
            Tables = new List<TableRenamer>();
        }

        [DataMember]
        public bool UseSchemaName { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string SchemaName { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<TableRenamer> Tables { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string TableRegexPattern { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string TablePatternReplaceWith { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string ColumnRegexPattern { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string ColumnPatternReplaceWith { get; set; }
    }
}
