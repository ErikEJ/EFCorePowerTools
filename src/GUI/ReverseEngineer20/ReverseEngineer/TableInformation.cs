namespace ReverseEngineer20.ReverseEngineer
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// A class holding a certain information about tables.
    /// </summary>
    [DebuggerDisplay("{" + nameof(SafeFullName) + ",nq}")]
    public class TableInformation
    {
        /// <summary>
        /// Gets the schema name of the table.
        /// </summary>
        public string Schema { get; }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets whether a primary key exists for the table or not.
        /// </summary>
        public bool HasPrimaryKey { get; }

        /// <summary>
        /// Gets the unsafe (unescaped) full name of the table.
        /// </summary>
        public string UnsafeFullName => $"{Schema}.{Name}";

        /// <summary>
        /// Gets the safe (escaped) full name of the table.
        /// </summary>
        public string SafeFullName => $"[{Schema}].[{Name}]";

        /// <summary>
        /// Initializes a new instance of the <see cref="TableInformation"/> class for a specific table.
        /// </summary>
        /// <param name="schema">The schema name of the table.</param>
        /// <param name="name">The table name.</param>
        /// <param name="hasPrimaryKey">Whether or not a primary key exists for the table.</param>
        /// <exception cref="ArgumentException"><paramref name="schema"/> or <paramref name="name"/> are null or only white spaces.</exception>
        public TableInformation(string schema,
                                string name,
                                bool hasPrimaryKey)
        {
            if (string.IsNullOrWhiteSpace(schema))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(schema));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));

            Schema = schema;
            Name = name;
            HasPrimaryKey = hasPrimaryKey;
        }

        /// <summary>
        /// Parses the given <paramref name="table"/> into a <see cref="TableInformation"/> instance.
        /// </summary>
        /// <param name="table">The table to parse.</param>
        /// <exception cref="ArgumentException"><paramref name="table"/> is null, contains only white spaces, or cannot be parsed.</exception>
        /// <returns>The created <see cref="TableInformation"/> instance.</returns>
        /// <remarks><paramref name="table"/> should have the format <b>schema.table</b>.</remarks>
        public static TableInformation Parse(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(table));

            var split = table.Split('.');
            if (split.Length != 2)
                throw new ArgumentException(@"Value cannot be parsed.", nameof(table));

            var schema = split[0];
            var name = split[1];
            return new TableInformation(schema, name, true);
        }

        /// <summary>
        /// Parses the given <paramref name="table"/> into a <see cref="TableInformation"/> instance.
        /// </summary>
        /// <param name="table">The table to parse.</param>
        /// <param name="hasPrimaryKey">Whether or not a primary key exists for the table.</param>
        /// <exception cref="ArgumentException"><paramref name="table"/> is null, contains only white spaces, or cannot be parsed.</exception>
        /// <returns>The created <see cref="TableInformation"/> instance.</returns>
        /// <remarks><paramref name="table"/> should have the format <b>schema.table</b>.</remarks>
        public static TableInformation Parse(string table, bool hasPrimaryKey)
        {
            if (string.IsNullOrWhiteSpace(table))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(table));

            var split = table.Split('.');
            if (split.Length != 2)
                throw new ArgumentException(@"Value cannot be parsed.", nameof(table));

            var schema = split[0];
            var name = split[1];
            return new TableInformation(schema, name, hasPrimaryKey);
        }
    }
}