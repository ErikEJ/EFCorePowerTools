using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using RevEng.Core.Abstractions.Metadata;
using SqlSharpener;
using SqlSharpener.Model;

namespace ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding
{
    /// <summary>
    /// Creates stored procedure result-set metadata by parsing SQL definitions and resolved select projections.
    /// </summary>
    public static class SqlServerStoredProcedureResultSetFactory
    {
        /// <summary>
        /// Parses a stored procedure definition and derives result sets from temp-table-backed <c>SELECT</c> statements.
        /// </summary>
        /// <param name="definition">The Transact-SQL stored procedure definition to parse.</param>
        /// <param name="singleResult">A value indicating whether only the first discovered result set should be returned.</param>
        /// <returns>The discovered result sets and their scaffoldable columns.</returns>
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Existing routine metadata APIs use List-based result sets.")]
        public static List<List<ModuleResultElement>> CreateFromDefinition(string definition, bool singleResult)
        {
            if (string.IsNullOrWhiteSpace(definition))
            {
                return new List<List<ModuleResultElement>>();
            }

            var parser = new TSql160Parser(false);
            var fragment = parser.Parse(new StringReader(definition), out var errors);

            if (errors.Count > 0 || fragment == null)
            {
                return new List<List<ModuleResultElement>>();
            }

            var visitor = new ProcedureDefinitionVisitor();
            fragment.Accept(visitor);

            if (visitor.Selects.Count == 0)
            {
                return new List<List<ModuleResultElement>>();
            }

            return CreateFromSelects(CreateSelects(visitor.Selects, visitor.TempTableTypes), singleResult);
        }

        /// <summary>
        /// Converts resolved <see cref="Select"/> metadata into scaffoldable result-set column definitions.
        /// </summary>
        /// <param name="selects">The resolved select projections to convert.</param>
        /// <param name="singleResult">A value indicating whether only the first non-empty result set should be returned.</param>
        /// <returns>The scaffoldable result sets and their columns.</returns>
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Existing routine metadata APIs use List-based result sets.")]
        public static List<List<ModuleResultElement>> CreateFromSelects(IEnumerable<Select> selects, bool singleResult)
        {
            var result = new List<List<ModuleResultElement>>();

            foreach (var select in selects ?? Enumerable.Empty<Select>())
            {
                var list = new List<ModuleResultElement>();
                var ordinal = 0;

                foreach (var column in select.Columns)
                {
                    if (column.DataTypes == null)
                    {
                        continue;
                    }

                    if (!column.DataTypes.TryGetValue(TypeFormat.SqlServerDbType, out var storeType)
                        || string.IsNullOrWhiteSpace(storeType))
                    {
                        continue;
                    }

                    list.Add(new ModuleResultElement
                    {
                        Name = column.Name,
                        Nullable = column.IsNullable,
                        StoreType = storeType,
                        Ordinal = ordinal++,
                        MaxLength = column.MaxLength,
                        Precision = column.Precision,
                        Scale = column.Scale,
                    });
                }

                if (list.Count > 0)
                {
                    result.Add(list);
                }

                if (singleResult && result.Count > 0)
                {
                    break;
                }
            }

            return result;
        }

        private static IEnumerable<Select> CreateSelects(IEnumerable<QuerySpecification> querySpecifications, IDictionary<string, DataType> bodyColumnTypes)
        {
            foreach (var querySpecification in querySpecifications)
            {
                Select select;

                try
                {
                    select = new Select(querySpecification, bodyColumnTypes);
                }
                catch (MissingBodyDependencyException)
                {
                    continue;
                }

                yield return select;
            }
        }

        /// <summary>
        /// Collects temp-table column types and query specifications from a stored procedure definition.
        /// </summary>
        private sealed class ProcedureDefinitionVisitor : TSqlFragmentVisitor
        {
            public Dictionary<string, DataType> TempTableTypes { get; } = new(StringComparer.InvariantCultureIgnoreCase);

            public List<QuerySpecification> Selects { get; } = new();

            public override void Visit(CreateTableStatement node)
            {
                base.Visit(node);

                var tableName = node.SchemaObjectName?.BaseIdentifier?.Value;
                if (string.IsNullOrWhiteSpace(tableName) || !tableName.StartsWith("#", StringComparison.Ordinal))
                {
                    return;
                }

                foreach (var column in node.Definition.ColumnDefinitions)
                {
                    if (!ScriptDomDataTypeHelper.TryCreateDataType(column, out var dataType))
                    {
                        continue;
                    }

                    TempTableTypes[$"{tableName}.{column.ColumnIdentifier.Value}"] = dataType;
                }
            }

            public override void Visit(SelectStatement node)
            {
                base.Visit(node);

                if (node.QueryExpression is QuerySpecification querySpecification)
                {
                    Selects.Add(querySpecification);
                }
            }
        }
    }
}
