using System.Runtime.Serialization;

namespace RevEng.Common.TableColumnRenaming
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
