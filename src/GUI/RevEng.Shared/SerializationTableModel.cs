using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RevEng.Shared
{
    /// <summary>
    /// A class holding information about database objects.
    /// </summary>
    [DataContract]
    public class SerializationTableModel
    {
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
        /// Initializes a new instance of the <see cref="SerializationTableModel"/> class for a specific table.
        /// </summary>
        /// <param name="name">The table name.</param>
        /// <exception cref="ArgumentException"><paramref name="schema"/> or <paramref name="name"/> are null or only white spaces.</exception>
        public SerializationTableModel(string name,
                            ObjectType objectType,
                            IList<string> excludedColumns)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));

            Name = name;
            ExcludedColumns = excludedColumns?.Count > 0 ? excludedColumns : null;
            ObjectType = objectType;
        }
    }
}