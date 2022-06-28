namespace RevEng.Common
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
        public ColumnModel(
            string name,
            bool isPrimaryKey,
            bool isForeignKey)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));
            }

            Name = name;
            IsPrimaryKey = isPrimaryKey;
            IsForeignKey = isForeignKey;
        }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a primary key exists for the table or not.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a foreign key exists for the table or not.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool IsForeignKey { get; set; }
    }
}
