using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlSharpener.Model
{
    /// <summary>
    /// Represents a SELECT statement in a stored procedure.
    /// </summary>
    [Serializable]
    public class Select
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Select"/> class.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="isSingleRow">if set to <c>true</c> the select statement uses a TOP 1 clause or is a function call.</param>
        /// <param name="tableAliases">The table aliases.</param>
        public Select(IEnumerable<SelectColumn> columns, bool isSingleRow, IDictionary<string, string> tableAliases)
        {
            this.Columns = columns ?? new List<SelectColumn>();
            this.IsSingleRow = isSingleRow;
            this.TableAliases = tableAliases ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Select"/> class.
        /// </summary>
        /// <param name="querySpecification">The query specification.</param>
        /// <param name="bodyColumnTypes">The body column types.</param>
        public Select(QuerySpecification querySpecification, IDictionary<string, DataType> bodyColumnTypes)
        {
            // Get any table aliases.
            var aliasResolutionVisitor = new AliasResolutionVisitor();
            querySpecification.Accept(aliasResolutionVisitor);
            this.TableAliases = aliasResolutionVisitor.Aliases;

            var outerJoinedTables = new List<string>();
            if (querySpecification.FromClause != null)
            {
                foreach (var join in querySpecification.FromClause.TableReferences.OfType<QualifiedJoin>())
                {
                    FillOuterJoins(outerJoinedTables, join, false);
                }
            }

            var topInt = querySpecification.TopRowFilter != null ? querySpecification.TopRowFilter.Expression as IntegerLiteral : null;
            this.IsSingleRow = topInt != null && topInt.Value == "1" && querySpecification.TopRowFilter.Percent == false;
            this.Columns = querySpecification.SelectElements.OfType<SelectScalarExpression>().Select(x => new SelectColumn(x, bodyColumnTypes, this.TableAliases, outerJoinedTables)).ToList();
        }

        /// <summary>
        /// Traverses the joins and gets a list of tables that have been outer joined.
        /// </summary>
        /// <param name="outerJoinedTables">The outer joined tables list.</param>
        /// <param name="qualifiedJoin">The qualified join.</param>
        /// <param name="isParentOuterJoined">if set to <c>true</c> a parent join was outer joined.</param>
        private void FillOuterJoins(List<string> outerJoinedTables, QualifiedJoin qualifiedJoin, bool isParentOuterJoined)
        {
            var tableReferences = new List<TableReference>();
            if (qualifiedJoin.QualifiedJoinType == QualifiedJoinType.LeftOuter)
            {
                if (isParentOuterJoined) tableReferences.Add(qualifiedJoin.FirstTableReference);
                tableReferences.Add(qualifiedJoin.SecondTableReference);
            }
            else if (qualifiedJoin.QualifiedJoinType == QualifiedJoinType.RightOuter)
            {
                if (isParentOuterJoined) tableReferences.Add(qualifiedJoin.SecondTableReference);
                tableReferences.Add(qualifiedJoin.FirstTableReference);
            }
            else if (qualifiedJoin.QualifiedJoinType == QualifiedJoinType.FullOuter || qualifiedJoin.QualifiedJoinType == QualifiedJoinType.Inner)
            {
                if (isParentOuterJoined)
                {
                    tableReferences.Add(qualifiedJoin.FirstTableReference);
                    tableReferences.Add(qualifiedJoin.SecondTableReference);
                }
            }

            foreach (var tableReference in tableReferences)
            {
                var nestedQualifiedJoin = tableReference as QualifiedJoin;
                var namedTableReference = tableReference as NamedTableReference;
                if (nestedQualifiedJoin != null)
                {
                    FillOuterJoins(outerJoinedTables, nestedQualifiedJoin, true);
                }
                else if (namedTableReference != null)
                {
                    var aliasOrName = namedTableReference.Alias != null && !string.IsNullOrEmpty(namedTableReference.Alias.Value)
                        ? namedTableReference.Alias.Value
                        : namedTableReference.SchemaObject.BaseIdentifier.Value;
                    outerJoinedTables.Add(aliasOrName);
                }
            }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public IEnumerable<SelectColumn> Columns { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this SELECT uses a TOP 1 clause or is a function call.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance uses a TOP 1 clause or is a function call; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleRow { get; private set; }

        /// <summary>
        /// Gets the table aliases.
        /// </summary>
        /// <value>
        /// The table aliases.
        /// </value>
        public IDictionary<string, string> TableAliases { get; private set; }
    }
}
