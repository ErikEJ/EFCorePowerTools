namespace RevEng.Shared
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
        /// <summary>
        /// Gets the database object display name
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
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the schema (can be null)
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

        /// <summary>
        /// Initializes a new instance of the <see cref="TableModel"/> class for a specific table.
        /// </summary>
        /// <param name="displayName">The object name used by scaffolding.</param>
        /// <param name="name">The object name.</param>
        /// <param name="name">The object schema.</param>
        /// <param name="hasPrimaryKey">Whether or not a primary key exists for the table.</param>
        /// <param name="showKeylessWarning">Show warning that the table or view is keyless.</param>
        /// <exception cref="ArgumentException"><paramref name="schema"/> or <paramref name="name"/> are null or only white spaces.</exception>
        public TableModel(string name,
                            string schema,
                            DatabaseType databaseType,
                            ObjectType objectType,
                            IEnumerable<ColumnModel> columns)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));

            DatabaseType = databaseType;
            Name = name;
            Schema = schema;
            ObjectType = objectType;
            Columns = columns;
        }
    }
}