using System;
using System.Collections.Generic;
using System.Linq;
using Dac = Microsoft.SqlServer.Dac.Model;

namespace SqlSharpener.Model
{
    /// <summary>
    /// Represents a table in the model.
    /// </summary>
    [Serializable]
    public class Table
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        /// <param name="columns">The columns.</param>
        public Table(string name, IEnumerable<Column> columns)
        {
            this.Name = name;
            this.Columns = columns ?? new List<Column>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table" /> class.
        /// </summary>
        /// <param name="tSqlObject">The TSqlObject representing the table.</param>
        public Table(Dac.TSqlObject tSqlObject)
        {
            ArgumentNullException.ThrowIfNull(tSqlObject);

            // Get the name.
            this.Name = tSqlObject.Name.Parts[tSqlObject.Name.Parts.Count - 1];

            // Get the columns
            var columns = new List<Column>();
            var sqlColumns = tSqlObject.ObjectType.Name == "TableType" ? tSqlObject.GetReferenced(Dac.TableType.Columns) : tSqlObject.GetReferenced(Dac.Table.Columns);
            foreach (var sqlColumn in sqlColumns)
            {
                var column = new Column(sqlColumn);
                columns.Add(column);
            }

            this.Columns = columns;
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public IEnumerable<Column> Columns { get; private set; }
    }
}
