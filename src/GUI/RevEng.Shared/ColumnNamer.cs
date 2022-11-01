using System.Runtime.Serialization;

namespace RevEng.Common
{
    [DataContract]
    public class ColumnNamer
    {
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the optional alternative name to look for if Name is not found.
        /// Used in navigation renaming since prediction of the generated name can be difficult.
        /// This way, for example, the user can use Name to suggest the "Fk+Navigation(s)" name while
        /// AlternateName supplies the basic pluralization name.
        /// </summary>
        [DataMember]
        public string AlternateName { get; set; }

        [DataMember]
        public string NewName { get; set; }
    }
}
