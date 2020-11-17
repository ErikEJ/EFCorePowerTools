namespace EFCorePowerTools.Shared.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using RevEng.Shared;

    /// <summary>
    /// A class holding information about database objects.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{" + nameof(Name) + ",nq}")]
    public class TableModel
    {
        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets whether a primary key exists for the table or not.
        /// </summary>
        [DataMember]
        public bool HasPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the object type.
        /// </summary>
        [DataMember]
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the columns excluded from reverse engineering.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<string> Columns { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableInformationModel"/> class for a specific table.
        /// </summary>
        /// <param name="name">The table name.</param>
        /// <param name="hasPrimaryKey">Whether or not a primary key exists for the table.</param>
        /// <param name="showKeylessWarning">Show warning that the table or view is keyless.</param>
        /// <exception cref="ArgumentException"><paramref name="schema"/> or <paramref name="name"/> are null or only white spaces.</exception>
        public TableModel(string name,
                            bool hasPrimaryKey,
                            ObjectType objectType,
                            IEnumerable<string> columns)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));

            Name = name;
            HasPrimaryKey = hasPrimaryKey;
            ObjectType = objectType;
            Columns = columns;
        }
    }
}