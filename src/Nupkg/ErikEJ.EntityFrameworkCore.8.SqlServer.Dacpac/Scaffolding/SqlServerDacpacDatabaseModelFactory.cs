using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using GOEddie.Dacpac.References;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.SqlServer.Dac.Extensions.Prototype;
using Microsoft.SqlServer.Dac.Model;

[assembly: CLSCompliant(false)]

namespace ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding
{
    public class SqlServerDacpacDatabaseModelFactory : IDatabaseModelFactory
    {
        private static readonly HashSet<string> DateTimePrecisionTypes = new HashSet<string> { "datetimeoffset", "datetime2", "time" };

        private static readonly HashSet<string> MaxLengthRequiredTypes
            = new HashSet<string> { "binary", "varbinary", "char", "varchar", "nchar", "nvarchar" };

        private readonly SqlServerDacpacDatabaseModelFactoryOptions dacpacOptions;

        private readonly IRelationalTypeMappingSource relationalTypeMappingSource;

        public SqlServerDacpacDatabaseModelFactory()
        {
        }

        public SqlServerDacpacDatabaseModelFactory(SqlServerDacpacDatabaseModelFactoryOptions options)
        {
            dacpacOptions = options;
        }

        public SqlServerDacpacDatabaseModelFactory(IRelationalTypeMappingSource mappingSource)
        {
            // injections when using as a separate nuget
            relationalTypeMappingSource = mappingSource;
        }

        public SqlServerDacpacDatabaseModelFactory(SqlServerDacpacDatabaseModelFactoryOptions options, IRelationalTypeMappingSource mappingSource)
        {
            dacpacOptions = options;
            relationalTypeMappingSource = mappingSource;
        }

        public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
        {
            throw new NotImplementedException();
        }

        public DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(@"invalid path", nameof(connectionString));
            }

            if (!File.Exists(connectionString))
            {
                throw new ArgumentException($"Dacpac file not found: {connectionString}");
            }

            var schemas = options.Schemas;
            var tables = options.Tables;

            var dbModel = new DatabaseModel
            {
                DatabaseName = Path.GetFileNameWithoutExtension(connectionString),
                DefaultSchema = schemas.Any() ? schemas.First() : "dbo",
            };

            dbModel["Scaffolding:ConnectionString"] = $"Data Source=(local);Initial Catalog={dbModel.DatabaseName};Integrated Security=true";

            if (dacpacOptions?.MergeDacpacs ?? false)
            {
                connectionString = DacpacConsolidator.Consolidate(connectionString);
            }

            using var model = new TSqlTypedModel(connectionString);

            var typeAliases = GetTypeAliases(model);

            var items = model.GetObjects<TSqlTable>(DacQueryScopes.UserDefined)
                .Where(t => !t.GetProperty<bool>(Table.IsAutoGeneratedHistoryTable))
                .Where(t => tables == null || !tables.Any() || tables.Contains($"[{t.Name.Parts[0]}].[{t.Name.Parts[1]}]"))
                .Where(t => $"{t.Name.Parts[1]}" != HistoryRepository.DefaultTableName)
                .Where(t => !schemas.Any() || schemas.Contains(t.Name.Parts[0]))
                .ToList();

            foreach (var item in items)
            {
                var dbTable = new DatabaseTable
                {
                    Name = item.Name.Parts[1],
                    Schema = item.Name.Parts[0],
                };

                if (item.MemoryOptimized)
                {
                    dbTable[SqlServerAnnotationNames.MemoryOptimized] = true;
                }

                var tableColumns = GetColumns(item, dbTable, typeAliases, model.GetObjects<TSqlDefaultConstraint>(DacQueryScopes.UserDefined).ToList(), model);
                GetPrimaryKey(item, dbTable);

                var description = model.GetObjects<TSqlExtendedProperty>(DacQueryScopes.UserDefined)
                    .Where(p => p.Name.Parts.Count == 4)
                    .Where(p => p.Name.Parts[0] == "SqlTableBase")
                    .Where(p => p.Name.Parts[1] == dbTable.Schema)
                    .Where(p => p.Name.Parts[2] == dbTable.Name)
                    .FirstOrDefault(p => p.Name.Parts[3] == "MS_Description");

                dbTable.Comment = FixExtendedPropertyValue(description?.Value);

                var temporal = item.GetReferenced(Table.TemporalSystemVersioningHistoryTable).ToArray();

                if (temporal.Length != 0)
                {
                    dbTable[SqlServerAnnotationNames.IsTemporal] = true;
                    dbTable[SqlServerAnnotationNames.TemporalHistoryTableName] = temporal[0].Name.Parts[1];
                    dbTable[SqlServerAnnotationNames.TemporalHistoryTableSchema] = temporal[0].Name.Parts[0];

                    foreach (var col in tableColumns)
                    {
                        var generatedAlwaysType = col.GetProperty<ColumnGeneratedAlwaysType>(Column.GeneratedAlwaysType);

                        if (generatedAlwaysType == ColumnGeneratedAlwaysType.GeneratedAlwaysAsRowStart)
                        {
                            dbTable[SqlServerAnnotationNames.TemporalPeriodStartPropertyName] = col.Name.Parts[2];
                        }

                        if (generatedAlwaysType == ColumnGeneratedAlwaysType.GeneratedAlwaysAsRowEnd)
                        {
                            dbTable[SqlServerAnnotationNames.TemporalPeriodEndPropertyName] = col.Name.Parts[2];
                        }
                    }
                }

                dbModel.Tables.Add(dbTable);
            }

            foreach (var item in items)
            {
                GetForeignKeys(item, dbModel);

                var dbTable = dbModel.Tables
                    .Single(t => t.Name == item.Name.Parts[1]
                    && t.Schema == item.Name.Parts[0]);

                GetUniqueConstraints(item, dbTable);
                GetIndexes(item, dbTable);
                GetTriggers(item, dbTable);
            }

            var views = model.GetObjects<TSqlView>(DacQueryScopes.UserDefined)
                .Where(t => tables == null || !tables.Any() || tables.Contains($"[{t.Name.Parts[0]}].[{t.Name.Parts[1]}]"))
                .ToList();

            var tableTypes = model.GetObjects<TSqlTableType>(DacQueryScopes.UserDefined);

            foreach (var view in views)
            {
                var dbView = new DatabaseView
                {
                    Name = view.Name.Parts[1],
                    Schema = view.Name.Parts[0],
                };

                GetViewColumns(view, dbView, typeAliases, tableTypes);

                dbModel.Tables.Add(dbView);
            }

            return dbModel;
        }

        private static Dictionary<string, (string A, string B)> GetTypeAliases(TSqlTypedModel model)
        {
            var items = model.GetObjects<TSqlDataType>(DacQueryScopes.UserDefined)
                .ToList();

            var typeAliasMap = new Dictionary<string, (string, string)>(StringComparer.OrdinalIgnoreCase);

            foreach (var udt in items)
            {
                int maxLength = udt.UddtIsMax ? -1 : udt.UddtLength;
                var storeType = GetStoreType(udt.Type.First().Name.Parts[0], maxLength, udt.UddtPrecision, udt.UddtScale);
                typeAliasMap.Add($"{udt.Name.Parts[0]}.{udt.Name.Parts[1]}", (storeType, udt.Type.First().Name.Parts[0]));
            }

            return typeAliasMap;
        }

        private static void GetPrimaryKey(TSqlTable table, DatabaseTable dbTable)
        {
            if (!table.PrimaryKeyConstraints.Any())
            {
                return;
            }

            var pk = table.PrimaryKeyConstraints.First();
            var primaryKey = new DatabasePrimaryKey
            {
                Name = pk.Name.HasName ? pk.Name.Parts[1] : null,
                Table = dbTable,
            };

            if (!pk.Clustered)
            {
                primaryKey["SqlServer:Clustered"] = false;
            }

            foreach (var pkPart in pk.Columns.Select(pk => pk.Name.Parts))
            {
                var dbCol = dbTable.Columns.FirstOrDefault(c => c.Name == pkPart[2])
                        ?? dbTable.Columns.FirstOrDefault(
                            c => c.Name.Equals(pkPart[2], StringComparison.OrdinalIgnoreCase));

                if (dbCol == null)
                {
                    return;
                }

                primaryKey.Columns.Add(dbCol);
            }

            dbTable.PrimaryKey = primaryKey;
        }

        private static void GetForeignKeys(TSqlTable table, DatabaseModel dbModel)
        {
            var dbTable = dbModel.Tables
                .Single(t => t.Name == table.Name.Parts[1]
                && t.Schema == table.Name.Parts[0]);

            var fks = table.ForeignKeyConstraints.ToList();
            foreach (var fk in fks)
            {
                var foreignTable = dbModel.Tables
                    .SingleOrDefault(t => t.Name == fk.ForeignTable.First().Name.Parts[1]
                    && t.Schema == fk.ForeignTable.First().Name.Parts[0]);

                if (foreignTable == null)
                {
                    continue;
                }

                var foreignKey = new DatabaseForeignKey
                {
                    Name = fk.Name.HasName ? fk.Name.Parts[1] : null,
                    Table = dbTable,
                    PrincipalTable = foreignTable,
                    OnDelete = ConvertToReferentialAction(fk.DeleteAction),
                };

                foreach (var fkCol in fk.Columns)
                {
                    var dbCol = dbTable.Columns
                        .Single(c => c.Name == fkCol.Name.Parts[2]);

                    foreignKey.Columns.Add(dbCol);
                }

                foreach (var fkCol in fk.ForeignColumns)
                {
                    var dbCol = foreignTable.Columns
                        .SingleOrDefault(c => c.Name == fkCol.Name.Parts[2]);

                    if (dbCol != null)
                    {
                        foreignKey.PrincipalColumns.Add(dbCol);
                    }
                }

                if (foreignKey.PrincipalColumns.Count > 0)
                {
                    dbTable.ForeignKeys.Add(foreignKey);
                }
            }
        }

        private static void GetTriggers(TSqlTable table, DatabaseTable dbTable)
        {
            var triggers = table.Triggers.ToList();

            if (triggers.Count != 0)
            {
                dbTable.Triggers.Add(new DatabaseTrigger { Name = "trigger" });
            }
        }

        private static void GetViewColumns(TSqlView item, DatabaseView dbTable, Dictionary<string, (string StoreType, string TypeName)> typeAliases, IEnumerable<TSqlTableType> tableTypes)
        {
            var viewColumns = item.Element.GetChildren(DacQueryScopes.UserDefined);

            var matchingTableType = tableTypes.FirstOrDefault(tt => tt.Name.ToString() == item.Name.ToString()
                                                                 && tt.Columns.All(ttc => viewColumns.Any(vc => vc.Name.ToString() == ttc.Name.ToString()))); // All Column Names of Table Type are also in View

            foreach (var column in viewColumns)
            {
                string storeType = null;

                var referenced = column.GetReferenced(DacQueryScopes.UserDefined).FirstOrDefault() ?? column;

                if (referenced == null)
                {
                    continue;
                }

                if (referenced.ObjectType.Name != "Column")
                {
                    continue;
                }

                var col = (TSqlColumn)TSqlModelElement.AdaptInstance(referenced);

                if (col.ColumnType == ColumnType.ComputedColumn)
                {
                    if (matchingTableType == null)
                    {
                        continue;
                    }

                    var matchingColumn = matchingTableType.Columns.FirstOrDefault(c => c.Name.ToString() == col.Name.ToString());

                    if (matchingColumn == null)
                    {
                        continue;
                    }

                    var dataTypeName = matchingColumn.DataType.First().Name.Parts[0];
                    int maxLength = matchingColumn.IsMax ? -1 : matchingColumn.Length;
                    storeType = GetStoreType(dataTypeName, maxLength, matchingColumn.Precision, matchingColumn.Scale);
                }
                else if (col.DataType.First().Name.Parts.Count > 1)
                {
                    if (typeAliases.TryGetValue($"{col.DataType.First().Name.Parts[0]}.{col.DataType.First().Name.Parts[1]}", out var value))
                    {
                        storeType = value.StoreType;
                    }
                }
                else
                {
                    var dataTypeName = col.DataType.First().Name.Parts[0];
                    int maxLength = col.IsMax ? -1 : col.Length;
                    storeType = GetStoreType(dataTypeName, maxLength, col.Precision, col.Scale);
                }

                var dbColumn = new DatabaseColumn
                {
                    Table = dbTable,
                    Name = column.Name.Parts[2],
                    IsNullable = col.Nullable,
                    StoreType = storeType,
                };

                dbTable.Columns.Add(dbColumn);
            }
        }

        private static string GetStoreType(string dataTypeName, int maxLength, int precision, int scale)
        {
            if (dataTypeName == "timestamp")
            {
                return "rowversion";
            }

            if (dataTypeName == "sysname")
            {
                return "nvarchar(128)";
            }

            if (dataTypeName == "decimal"
                || dataTypeName == "numeric")
            {
                return $"{dataTypeName}({precision}, {scale})";
            }

            if (DateTimePrecisionTypes.Contains(dataTypeName)
                && scale != 7)
            {
                return $"{dataTypeName}({scale})";
            }

            if (MaxLengthRequiredTypes.Contains(dataTypeName))
            {
                if (maxLength == -1)
                {
                    return $"{dataTypeName}(max)";
                }

                return $"{dataTypeName}({maxLength})";
            }

            return dataTypeName;
        }

        private static string FilterClrDefaults(string dataTypeName, bool nullable, string defaultValue)
        {
            defaultValue = StripParentheses(defaultValue);

            if (defaultValue == null
                || defaultValue == "NULL"
                || defaultValue == "null")
            {
                return null;
            }

            if (nullable)
            {
                return defaultValue;
            }

            if (defaultValue == "0")
            {
                if (dataTypeName == "bigint"
                    || dataTypeName == "bit"
                    || dataTypeName == "decimal"
                    || dataTypeName == "float"
                    || dataTypeName == "int"
                    || dataTypeName == "money"
                    || dataTypeName == "numeric"
                    || dataTypeName == "real"
                    || dataTypeName == "smallint"
                    || dataTypeName == "smallmoney"
                    || dataTypeName == "tinyint")
                {
                    return null;
                }
            }
            else if (defaultValue == "0.0")
            {
                if (dataTypeName == "decimal"
                    || dataTypeName == "float"
                    || dataTypeName == "money"
                    || dataTypeName == "numeric"
                    || dataTypeName == "real"
                    || dataTypeName == "smallmoney")
                {
                    return null;
                }
            }
            else if ((defaultValue == "CONVERT([real],(0))" && dataTypeName == "real")
                || (defaultValue == "0.0000000000000000e+000" && dataTypeName == "float")
                || (defaultValue == "'0001-01-01'" && dataTypeName == "date")
                || (defaultValue == "'1900-01-01T00:00:00.000'" && (dataTypeName == "datetime" || dataTypeName == "smalldatetime"))
                || (defaultValue == "'0001-01-01T00:00:00.000'" && dataTypeName == "datetime2")
                || (defaultValue == "'0001-01-01T00:00:00.000+00:00'" && dataTypeName == "datetimeoffset")
                || (defaultValue == "'00:00:00'" && dataTypeName == "time")
                || (defaultValue == "'00000000-0000-0000-0000-000000000000'" && dataTypeName == "uniqueidentifier"))
            {
                return null;
            }

            return defaultValue;
        }

        private static string StripParentheses(string defaultValue)
        {
            if (defaultValue.StartsWith('(') && defaultValue.EndsWith(')'))
            {
                defaultValue = defaultValue.Substring(1, defaultValue.Length - 2);
                return StripParentheses(defaultValue);
            }

            return defaultValue;
        }

        private static ReferentialAction? ConvertToReferentialAction(ForeignKeyAction onDeleteAction)
        {
            switch (onDeleteAction)
            {
                case ForeignKeyAction.NoAction:
                    return ReferentialAction.NoAction;

                case ForeignKeyAction.Cascade:
                    return ReferentialAction.Cascade;

                case ForeignKeyAction.SetNull:
                    return ReferentialAction.SetNull;

                case ForeignKeyAction.SetDefault:
                    return ReferentialAction.SetDefault;

                default:
                    return null;
            }
        }

        private static string FixExtendedPropertyValue(string value)
        {
            if (value == null)
            {
                return null;
            }

            if (value.StartsWith("N'", StringComparison.Ordinal))
            {
                value = value.Substring(2);
            }

            // support for CDATA without N prefix: <Value><![CDATA['Enum: 0-Val1, 1-Val2, 2-Val3, 3-Val4']]></Value>
            if (value.StartsWith('\''))
            {
                value = value.Substring(1);
            }

            if (value.EndsWith('\''))
            {
                value = value.Remove(value.Length - 1, 1);
            }

            return value;
        }

        private static bool IsNumeric(Type type)
        {
            type = UnwrapNullableType(type);

            return IsInteger(type)
                   || type == typeof(decimal)
                   || type == typeof(float)
                   || type == typeof(double);
        }

        private static bool IsInteger(Type type)
        {
            type = UnwrapNullableType(type);

            return type == typeof(int)
                   || type == typeof(long)
                   || type == typeof(short)
                   || type == typeof(byte)
                   || type == typeof(uint)
                   || type == typeof(ulong)
                   || type == typeof(ushort)
                   || type == typeof(sbyte)
                   || type == typeof(char);
        }

        private static Type UnwrapNullableType(Type type)
            => Nullable.GetUnderlyingType(type) ?? type;

        private object TryParseClrDefault(string dataTypeName, string defaultValueSql)
        {
            defaultValueSql = defaultValueSql?.Trim();
            if (string.IsNullOrEmpty(defaultValueSql))
            {
                return null;
            }

            var mapping = relationalTypeMappingSource?.FindMapping(dataTypeName);
            if (mapping == null)
            {
                return null;
            }

            Unwrap();
            if (defaultValueSql.StartsWith("CONVERT", StringComparison.OrdinalIgnoreCase))
            {
                defaultValueSql = defaultValueSql.Substring(defaultValueSql.IndexOf(',', StringComparison.CurrentCulture) + 1);
                defaultValueSql = defaultValueSql.Substring(0, defaultValueSql.LastIndexOf(')'));
                Unwrap();
            }

            if (defaultValueSql.Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var type = mapping.ClrType;
            if (type == typeof(bool)
                && int.TryParse(defaultValueSql, out var intValue))
            {
                return intValue != 0;
            }

            if (IsNumeric(type))
            {
                try
                {
                    return Convert.ChangeType(defaultValueSql, type, CultureInfo.InvariantCulture);
                }
                catch (Exception exp) when (exp is InvalidCastException or FormatException or OverflowException or ArgumentNullException)
                {
                    // Ignored
                    return null;
                }
            }

            if ((defaultValueSql.StartsWith('\'') || defaultValueSql.StartsWith("N'", StringComparison.OrdinalIgnoreCase))
                && defaultValueSql.EndsWith('\''))
            {
                var startIndex = defaultValueSql.IndexOf('\'', StringComparison.CurrentCulture);
                defaultValueSql = defaultValueSql.Substring(startIndex + 1, defaultValueSql.Length - (startIndex + 2));

                if (type == typeof(string))
                {
                    return defaultValueSql;
                }

                if (type == typeof(bool)
                    && bool.TryParse(defaultValueSql, out var boolValue))
                {
                    return boolValue;
                }

                if (type == typeof(Guid)
                    && Guid.TryParse(defaultValueSql, out var guid))
                {
                    return guid;
                }

                if (type == typeof(DateTime)
                    && DateTime.TryParse(defaultValueSql, CultureInfo.CurrentCulture, out var dateTime))
                {
                    return dateTime;
                }

                if (type == typeof(DateOnly)
                    && DateOnly.TryParse(defaultValueSql, CultureInfo.CurrentCulture, out var dateOnly))
                {
                    return dateOnly;
                }

                if (type == typeof(TimeOnly)
                    && TimeOnly.TryParse(defaultValueSql, CultureInfo.CurrentCulture, out var timeOnly))
                {
                    return timeOnly;
                }

                if (type == typeof(DateTimeOffset)
                    && DateTimeOffset.TryParse(defaultValueSql, CultureInfo.CurrentCulture, out var dateTimeOffset))
                {
                    return dateTimeOffset;
                }
            }

            return null;

            void Unwrap()
            {
                while (defaultValueSql.StartsWith('(') && defaultValueSql.EndsWith(')'))
                {
                    defaultValueSql = defaultValueSql.Substring(1, defaultValueSql.Length - 2).Trim();
                }
            }
        }

        private IEnumerable<TSqlColumn> GetColumns(TSqlTable item, DatabaseTable dbTable, Dictionary<string, (string StoreType, string TypeName)> typeAliases, List<TSqlDefaultConstraint> defaultConstraints, TSqlTypedModel model)
        {
            var tableColumns = item.Columns
                .Where(i => i.ColumnType != ColumnType.ColumnSet

                // Computed columns not supported for now
                // Probably not possible: https://stackoverflow.com/questions/27259640/get-datatype-of-computed-column-from-dacpac
                && i.ColumnType != ColumnType.ComputedColumn);

            foreach (var col in tableColumns)
            {
                var def = defaultConstraints.Find(d => d.TargetColumn.First().Name.ToString() == col.Name.ToString());
                string storeType = null;
                string systemTypeName = null;

                if (col.DataType.First().Name.Parts.Count > 1 && typeAliases.TryGetValue($"{col.DataType.First().Name.Parts[0]}.{col.DataType.First().Name.Parts[1]}", out var value))
                {
                    storeType = value.StoreType;
                    systemTypeName = value.TypeName;
                }
                else
                {
                    var dataTypeName = col.DataType.First().Name.Parts[0];
                    if (col.DataType.First().Name.Parts.Count > 1)
                    {
                        dataTypeName = col.DataType.First().Name.Parts[1];
                    }

                    int maxLength = col.IsMax ? -1 : col.Length;
                    storeType = GetStoreType(dataTypeName, maxLength, col.Precision, col.Scale);
                    systemTypeName = dataTypeName;
                }

                string defaultValue = def != null ? FilterClrDefaults(systemTypeName, col.Nullable, def.Expression) : null;

                var dbColumn = new DatabaseColumn
                {
                    Table = dbTable,
                    Name = col.Name.Parts[2],
                    IsNullable = col.Nullable,
                    StoreType = storeType,

                    // this property affects whether the bool type will be nullable
                    DefaultValue = TryParseClrDefault(systemTypeName, defaultValue),
                    DefaultValueSql = defaultValue,
                    ComputedColumnSql = col.Expression,
                    ValueGenerated = col.IsIdentity
                        ? ValueGenerated.OnAdd
                        : storeType == "rowversion"
                            ? ValueGenerated.OnAddOrUpdate
                            : default(ValueGenerated?),
                };
                if (storeType == "rowversion")
                {
                    dbColumn["ConcurrencyToken"] = true;
                }

                var description = model.GetObjects<TSqlExtendedProperty>(DacQueryScopes.UserDefined)
                    .Where(p => p.Name.Parts.Count == 5)
                    .Where(p => p.Name.Parts[0] == "SqlColumn")
                    .Where(p => p.Name.Parts[1] == dbTable.Schema)
                    .Where(p => p.Name.Parts[2] == dbTable.Name)
                    .Where(p => p.Name.Parts[3] == dbColumn.Name)
                    .FirstOrDefault(p => p.Name.Parts[4] == "MS_Description");

                dbColumn.Comment = FixExtendedPropertyValue(description?.Value);

                var generatedAlwaysType = col.GetProperty<ColumnGeneratedAlwaysType>(Column.GeneratedAlwaysType);

                if (generatedAlwaysType != ColumnGeneratedAlwaysType.GeneratedAlwaysAsRowStart && generatedAlwaysType != ColumnGeneratedAlwaysType.GeneratedAlwaysAsRowEnd)
                {
                    dbTable.Columns.Add(dbColumn);
                }
            }

            return tableColumns;
        }

        private void GetUniqueConstraints(TSqlTable table, DatabaseTable dbTable)
        {
            var uqs = table.UniqueConstraints.ToList();
            foreach (var uq in uqs)
            {
                if (dacpacOptions != null
                    && dacpacOptions.ExcludedIndexes != null
                    && dacpacOptions.ExcludedIndexes.TryGetValue(table.Name.ToString(), out var indexes)
                    && indexes != null
                    && uq.Name.HasName
                    && indexes.Contains(uq.Name.Parts[1]))
                {
                    continue;
                }

                var uniqueConstraint = new DatabaseUniqueConstraint
                {
                    Name = uq.Name.HasName ? uq.Name.Parts[1] : null,
                    Table = dbTable,
                };

                if (uq.Clustered)
                {
                    uniqueConstraint["SqlServer:Clustered"] = true;
                }

                foreach (var uqCol in uq.Columns)
                {
                    var dbCol = dbTable.Columns
                        .SingleOrDefault(c => c.Name == uqCol.Name.Parts[2]);

                    if (dbCol == null)
                    {
                        continue;
                    }

                    uniqueConstraint.Columns.Add(dbCol);
                }

                if (uniqueConstraint.Columns.Count > 0)
                {
                    dbTable.UniqueConstraints.Add(uniqueConstraint);
                }
            }
        }

        private void GetIndexes(TSqlTable table, DatabaseTable dbTable)
        {
            var ixs = table.Indexes.ToList();
            foreach (var sqlIx in ixs)
            {
                var ix = sqlIx as TSqlIndex;

                if (sqlIx == null)
                {
                    continue;
                }

                if (dacpacOptions != null
                    && dacpacOptions.ExcludedIndexes != null
                    && dacpacOptions.ExcludedIndexes.TryGetValue(table.Name.ToString(), out var indexes)
                    && indexes != null
                    && indexes.Contains(ix.Name.Parts[2]))
                {
                    continue;
                }

                var index = new DatabaseIndex
                {
                    Name = ix.Name.Parts[2],
                    Table = dbTable,
                    IsUnique = ix.Unique,
                    Filter = ix.FilterPredicate,
                };

                if (ix.Clustered)
                {
                    index["SqlServer:Clustered"] = true;
                }

                foreach (var column in ix.Columns)
                {
                    var dbCol = dbTable.Columns
                        .SingleOrDefault(c => c.Name == column.Name.Parts[2]);

                    if (dbCol != null)
                    {
                        index.Columns.Add(dbCol);
                    }
                }

                if (index.Columns.Count > 0)
                {
                    dbTable.Indexes.Add(index);
                }
            }
        }
    }
}