namespace RevEng.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    /// <summary>
    /// A class holding schema information about database objects.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{" + nameof(DisplayName) + ",nq}")]
    public class TableModel
    {
        public TableModel(
            string name,
            string schema,
            DatabaseType databaseType,
            ObjectType objectType,
            IEnumerable<ColumnModel> columns)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));
            }

            DatabaseType = databaseType;
            Name = name;
            Schema = schema;
            ObjectType = objectType;
            Columns = columns;
        }

        /// <summary>
        /// Gets the database object display name.
        /// </summary>
        [IgnoreDataMember]
        public string DisplayName
        {
            get
            {
                if (DatabaseType == DatabaseType.SQLServer || DatabaseType == DatabaseType.SQLServerDacpac)
                {
                    return $"[{Schema}].[{Name}]";
                }
                else
                {
                    return string.IsNullOrEmpty(Schema)
                        ? Name
                        : $"{Schema}.{Name}";
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the schema (can be null).
        /// </summary>
        [DataMember]
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the object type.
        /// </summary>
        [DataMember]
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<ColumnModel> Columns { get; set; }

        /// <summary>
        /// Gets the database type.
        /// </summary>
        [DataMember]
        public DatabaseType DatabaseType { get; private set; }
    }
}
