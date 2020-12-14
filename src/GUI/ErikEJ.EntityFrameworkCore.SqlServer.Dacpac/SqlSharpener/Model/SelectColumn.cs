using Microsoft.SqlServer.TransactSql.ScriptDom;
using dac = Microsoft.SqlServer.Dac.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dac;

namespace SqlSharpener.Model
{
    /// <summary>
    /// Represents a column in a SELECT statement in a stored procedure.
    /// </summary>
    [Serializable]
    public class SelectColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectColumn" /> class.
        /// </summary>
        /// <param name="name">The name or alias.</param>
        /// <param name="dataTypes">The data types.</param>
        /// <param name="isNullable">if set to <c>true</c> [is nullable].</param>
        public SelectColumn(string name, IDictionary<TypeFormat, string> dataTypes, bool isNullable)
        {
            this.Name = name;
            this.DataTypes = dataTypes ?? new Dictionary<TypeFormat, string>();
            this.IsNullable = isNullable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectColumn" /> class.
        /// </summary>
        /// <param name="selectScalarExpression">The select scalar expression.</param>
        /// <param name="bodyColumnTypes">The body column types.</param>
        /// <param name="tableAliases">The table aliases.</param>
        /// <param name="outerJoinedTables">The aliases or names of tables that were outer joined. Used to determine if a non-nulllable column could still be null.</param>
        /// <exception cref="System.InvalidOperationException">Could not find column within BodyDependencies:  + fullColName</exception>
        public SelectColumn(SelectScalarExpression selectScalarExpression, IDictionary<string, DataType> bodyColumnTypes, IDictionary<string, string> tableAliases, IEnumerable<string> outerJoinedTables)
        {
            if (selectScalarExpression.Expression is ColumnReferenceExpression)
            {
                var columnReferenceExpression = (ColumnReferenceExpression)selectScalarExpression.Expression;
                var identifiers = columnReferenceExpression.MultiPartIdentifier.Identifiers;
                var fullColName = this.GetFullColumnName(tableAliases, identifiers);

                this.Name = selectScalarExpression.ColumnName != null && selectScalarExpression.ColumnName.Value != null
                    ? selectScalarExpression.ColumnName.Value
                    : identifiers.Last().Value;

                var key = bodyColumnTypes.Keys.FirstOrDefault(x => x.EndsWith(fullColName, StringComparison.InvariantCultureIgnoreCase));
                if (key == null) throw new InvalidOperationException("Could not find column within BodyDependencies: " + fullColName);

                
                bool outerJoined = false;
                // If the column was defined in the SELECT with the table alias or name, check if the table was outer joined.
                if (identifiers.Count() > 1)
                {
                    var tableAliasOrName = identifiers.ElementAt(identifiers.Count() - 2).Value;
                    outerJoined = outerJoinedTables.Contains(tableAliasOrName);
                }
                else // If the column was defined in the SELECT without any qualification, then there must
                    // be only one column in the list of tables with that name. Look it up in the bodyColumnTypes.
                {
                    var keyParts = key.Split('.');
                    if (keyParts.Count() > 1)
                    {
                        var tableAliasOrName = keyParts.ElementAt(keyParts.Count() - 2);
                        outerJoined = outerJoinedTables.Contains(tableAliasOrName);
                    }
                }
                
                var bodyColumnType = bodyColumnTypes[key];
                this.DataTypes = bodyColumnType.Map;
                this.IsNullable = bodyColumnType.Nullable || outerJoined;
            }
            else if (selectScalarExpression.Expression is ConvertCall)
            {
                var convertCall = (ConvertCall)selectScalarExpression.Expression;
                this.Name = selectScalarExpression.ColumnName != null && selectScalarExpression.ColumnName.Value != null
                    ? selectScalarExpression.ColumnName.Value
                    : "Value";
                this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, convertCall.DataType.Name.BaseIdentifier.Value);
                this.IsNullable = true;
            }
            else if (selectScalarExpression.Expression is CastCall)
            {
                var castCall = (CastCall)selectScalarExpression.Expression;
                this.Name = selectScalarExpression.ColumnName != null && selectScalarExpression.ColumnName.Value != null
                    ? selectScalarExpression.ColumnName.Value
                    : "Value";
                this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, castCall.DataType.Name.BaseIdentifier.Value);
                this.IsNullable = true;
            }
            else if (selectScalarExpression.Expression is IntegerLiteral)
            {
                var integerLiteral = (IntegerLiteral)selectScalarExpression.Expression;
                this.Name = selectScalarExpression.ColumnName != null && selectScalarExpression.ColumnName.Value != null
                    ? selectScalarExpression.ColumnName.Value
                    : "Value";
                this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, "int");
                this.IsNullable = true;
            }
            else if (selectScalarExpression.Expression is VariableReference)
            {
                var variableReference = (VariableReference)selectScalarExpression.Expression;
                this.Name = variableReference.Name.TrimStart('@');
                this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.DotNetFrameworkType, "Object");
                this.IsNullable = true;
            }
            else
            {
                this.Name = selectScalarExpression.ColumnName != null && selectScalarExpression.ColumnName.Value != null
                    ? selectScalarExpression.ColumnName.Value
                    : "Value";
                this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.DotNetFrameworkType, "Object");
                this.IsNullable = true;
            }
        }

        /// <summary>
        /// Gets the name or alias.
        /// </summary>
        /// <value>
        /// The name or alias.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the data types.
        /// </summary>
        /// <value>
        /// The data types.
        /// </value>
        public IDictionary<TypeFormat, string> DataTypes { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets the fully qualified column name with any table aliases resolved.
        /// </summary>
        /// <param name="tableAliases">The table aliases.</param>
        /// <param name="identifiers">The identifiers in the MultiPartIdentifier.</param>
        /// <returns>
        /// The fully qualified column name.
        /// </returns>
        private string GetFullColumnName(IDictionary<string, string> tableAliases, IList<Identifier> identifiers)
        {
            var list = identifiers.Select(x => x.Value).ToArray();
            if (list.Count() > 1)
            {
                var tableIdentifier = list.ElementAt(list.Count() - 2);
                if (tableAliases.Keys.Any(x => x == tableIdentifier))
                {
                    list[list.Count() - 2] = tableAliases[tableIdentifier];
                }
            }
            return string.Join(".", list);
        }
    }
}
