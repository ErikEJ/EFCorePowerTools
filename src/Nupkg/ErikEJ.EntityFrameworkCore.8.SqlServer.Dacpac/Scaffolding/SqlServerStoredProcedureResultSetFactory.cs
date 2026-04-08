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

            if (visitor.TempTableTypes.Count == 0 || visitor.Selects.Count == 0)
            {
                return new List<List<ModuleResultElement>>();
            }

            return CreateFromSelects(visitor.Selects.Select(select => new Select(select, visitor.TempTableTypes)), singleResult);
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

        /// <summary>
        /// Collects temp-table column types and query specifications from a stored procedure definition.
        /// </summary>
        private sealed class ProcedureDefinitionVisitor : TSqlFragmentVisitor
        {
            public Dictionary<string, DataType> TempTableTypes { get; } = new(StringComparer.InvariantCultureIgnoreCase);

            public List<QuerySpecification> Selects { get; } = new();

            /// <summary>
             /// Maps a temp-table column definition to a SqlSharpener <see cref="DataType"/> when the type is supported.
             /// </summary>
            private static bool TryCreateDataType(ColumnDefinition column, out DataType dataType)
            {
                dataType = null;

                if (column.DataType == null)
                {
                    return false;
                }

                var storeType = GetStoreTypeName(column.DataType);
                if (string.IsNullOrWhiteSpace(storeType))
                {
                    return false;
                }

                var typeMap = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, storeType);
                if (typeMap == null)
                {
                    return false;
                }

                dataType = new DataType
                {
                    Map = typeMap,
                    Nullable = !column.Constraints.OfType<NullableConstraintDefinition>().Any(c => !c.Nullable),
                    MaxLength = GetMaxLength(column.DataType),
                    Precision = GetPrecision(column.DataType),
                    Scale = GetScale(column.DataType),
                };

                return true;
            }

            /// <summary>
            /// Normalizes ScriptDom data type references to SQL Server store type names used by the type map.
            /// </summary>
            private static string GetStoreTypeName(DataTypeReference dataType)
            {
                var typeName = dataType switch
                {
                    SqlDataTypeReference sqlDataTypeReference => sqlDataTypeReference.SqlDataTypeOption.ToString(),
                    UserDataTypeReference userDataTypeReference => userDataTypeReference.Name?.BaseIdentifier?.Value,
                    _ => null,
                };

                return typeName?.ToLowerInvariant() switch
                {
                    "sysname" => "nvarchar",
                    var name => name,
                };
            }

            /// <summary>
            /// Extracts max length from parameterized SQL type declarations, preserving <c>MAX</c> as <c>-1</c>.
            /// </summary>
            private static int GetMaxLength(DataTypeReference dataType)
            {
                if (dataType is not ParameterizedDataTypeReference parameterizedDataType || parameterizedDataType.Parameters.Count == 0)
                {
                    return 0;
                }

                if (parameterizedDataType.Parameters[0].LiteralType == LiteralType.Max)
                {
                    return -1;
                }

                if (parameterizedDataType.Parameters[0] is IntegerLiteral integerLiteral
                    && int.TryParse(integerLiteral.Value, out var value))
                {
                    return value;
                }

                return 0;
            }

            /// <summary>
            /// Extracts precision from parameterized SQL type declarations when present.
            /// </summary>
            private static short? GetPrecision(DataTypeReference dataType)
            {
                if (dataType is not ParameterizedDataTypeReference parameterizedDataType || parameterizedDataType.Parameters.Count == 0)
                {
                    return null;
                }

                if (parameterizedDataType.Parameters[0] is IntegerLiteral integerLiteral
                    && short.TryParse(integerLiteral.Value, out var value))
                {
                    return value;
                }

                return null;
            }

            /// <summary>
            /// Extracts scale from parameterized SQL type declarations when present.
            /// </summary>
            private static short? GetScale(DataTypeReference dataType)
            {
                if (dataType is not ParameterizedDataTypeReference parameterizedDataType || parameterizedDataType.Parameters.Count < 2)
                {
                    return null;
                }

                if (parameterizedDataType.Parameters[1] is IntegerLiteral integerLiteral
                    && short.TryParse(integerLiteral.Value, out var value))
                {
                    return value;
                }

                return null;
            }

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
                    if (!TryCreateDataType(column, out var dataType))
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
