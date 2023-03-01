using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RevEng.Common
{
    /// <summary>
    /// A class holding information about database objects.
    /// </summary>
    [DataContract]
    public class SerializationTableModel
    {
        public SerializationTableModel(
            string name,
            ObjectType objectType,
            IList<string> excludedColumns)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));
            }

            Name = name;
            ExcludedColumns = excludedColumns?.Count > 0 ? excludedColumns : null;
            ObjectType = objectType;
        }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the object type.
        /// </summary>
        [DataMember]
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the columns excluded from reverse engineering.
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public IEnumerable<string> ExcludedColumns { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to exclude table from default result set discovery.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool UseLegacyResultSetDiscovery { get; set; }

        /// <summary>
        /// Gets or sets a map type to an existing DbSet.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string MappedType { get; set; }
    }
}
