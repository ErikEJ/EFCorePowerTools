namespace RevEng.Shared
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    /// <summary>
    /// A class holding information about database objects.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{" + nameof(Name) + ",nq}")]
    public class ColumnModel
    {
        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets whether a primary key exists for the table or not.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets whether a foreign key exists for the table or not.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnModel"/> class for a specific column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="isPrimaryKey">Whether or not the column is part of the primary key.</param>
        /// <exception cref="ArgumentException"><paramref name="schema"/> or <paramref name="name"/> are null or only white spaces.</exception>
        public ColumnModel(string name,
                            bool isPrimaryKey,
                            bool isForeignKey)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));

            Name = name;
            IsPrimaryKey = isPrimaryKey;
            IsForeignKey = isForeignKey;
        }
    }
}