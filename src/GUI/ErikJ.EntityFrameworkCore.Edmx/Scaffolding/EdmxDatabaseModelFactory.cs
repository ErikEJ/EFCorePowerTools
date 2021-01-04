using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using LinqToEdmx;
using LinqToEdmx.Model.ConceptualV3;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace ErikJ.EntityFrameworkCore.Edmx.Scaffolding
{
    public class EdmxDatabaseModelFactory : IDatabaseModelFactory
    {
        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public EdmxDatabaseModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
        }

        internal string GetSchemaNameV3(EdmxV3 edmxv3, string entityName)
        {
            var entitySetContainer = edmxv3.GetItems<LinqToEdmx.Model.StorageV3.EntityContainer>();
            foreach (var container in entitySetContainer)
            {
                foreach (var entitySet in container.EntitySets.Where(es => es.Type == "Tables"))
                {
                    if (entitySet.Name == entityName)
                    {
                        return entitySet.Schema;
                    }
                }

                // FIXME Return the correct schema for views
                //foreach (var entitySet in container.EntitySets.Where(es => es.Type == "Views"))
                //{
                //    if (entitySet.EntityType.Contains(entityName))
                //    {
                //        return entitySet.Schema;
                //    }
                //}
            }
            return string.Empty;
        }

        public DatabaseModel Create(string edmxPath, DatabaseModelFactoryOptions options)
        {
            if (string.IsNullOrEmpty(edmxPath))
            {
                throw new ArgumentException(@"invalid path", nameof(edmxPath));
            }
            if (!File.Exists(edmxPath))
            {
                throw new ArgumentException($"Edmx file not found: {edmxPath}");
            }

            var schemas = options.Schemas;
            var tables = options.Tables;

            var dbModel = new DatabaseModel
            {
                DatabaseName = Path.GetFileNameWithoutExtension(edmxPath),
                DefaultSchema = schemas.Count() > 0 ? schemas.First() : "dbo"
            };

            // FIXME Assume EDMX version is 3.0 for now
            var edmxv3 = LinqToEdmx.EdmxV3.Load(edmxPath);

            if (string.Compare(edmxv3.Version, @"3.0", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                var items = edmxv3.GetItems<LinqToEdmx.Model.ConceptualV3.EntityType>();

                foreach (var item in items)
                {
                    var dbTable = new DatabaseTable
                    {
                        Name = item.Name,
                        Schema = GetSchemaNameV3(edmxv3, item.Name),
                    };

                    GetColumns(item, dbTable/*, typeAliases, model.GetObjects<TSqlDefaultConstraint>(DacQueryScopes.UserDefined).ToList()*/, edmxv3);
                    GetPrimaryKey(item, dbTable);

                    dbModel.Tables.Add(dbTable);
                }
            }

            return dbModel;
        }

        public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
        {
            throw new NotImplementedException();
        }

        private void GetColumns(LinqToEdmx.Model.ConceptualV3.EntityType item, DatabaseTable dbTable/*, IReadOnlyDictionary<string, (string storeType, string typeName)> typeAliases, List<TSqlDefaultConstraint> defaultConstraints,*/, LinqToEdmx.EdmxV3 model)
        {
            var tableColumns = item.Properties;
                //.Where(i => !i.GetProperty<bool>(Column.IsHidden)
                //&& i.ColumnType != ColumnType.ColumnSet
                //// Computed columns not supported for now
                //// Probably not possible: https://stackoverflow.com/questions/27259640/get-datatype-of-computed-column-from-dacpac
                //&& i.ColumnType != ColumnType.ComputedColumn
                //);

            foreach (var col in tableColumns)
            {
                //var def = defaultConstraints.Where(d => d.TargetColumn.First().Name.ToString() == col.Name.ToString()).FirstOrDefault();
                string storeType = GetStoreType(item, col, model);
                string systemTypeName = col.Type.ToString();

                //if (col.DataType.First().Name.Parts.Count > 1)
                //{
                //    if (typeAliases.TryGetValue($"{col.DataType.First().Name.Parts[0]}.{col.DataType.First().Name.Parts[1]}", out var value))
                //    {
                //        storeType = value.storeType;
                //        systemTypeName = value.typeName;
                //    }
                //}
                //else
                //{
                    var dataTypeName = col.Type.ToString();
                    int maxLength = int.Parse(col.MaxLength.ToString());
                    storeType = GetStoreType(dataTypeName, maxLength, col.Precision, col.Scale);
                    systemTypeName = dataTypeName;
                //}

                string defaultValue = def != null ? FilterClrDefaults(systemTypeName, col.Nullable, def.Expression.ToLowerInvariant()) : null;

                var dbColumn = new DatabaseColumn
                {
                    Table = dbTable,
                    Name = col.Name.Parts[2],
                    IsNullable = col.Nullable,
                    StoreType = storeType,
                    DefaultValueSql = defaultValue,
                    ComputedColumnSql = col.Expression,
                    ValueGenerated = col.IsIdentity
                        ? ValueGenerated.OnAdd
                        : storeType == "rowversion"
                            ? ValueGenerated.OnAddOrUpdate
                            : default(ValueGenerated?)
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
                    .Where(p => p.Name.Parts[4] == "MS_Description")
                    .FirstOrDefault();

                dbColumn.Comment = FixExtendedPropertyValue(description?.Value);

                dbTable.Columns.Add(dbColumn);
            }
        }

        private string GetStoreType(LinqToEdmx.Model.ConceptualV3.EntityType item, LinqToEdmx.Model.ConceptualV3.EntityProperty col, EdmxV3 model)
        {
            var entityTypeMappings = model.GetItems<EntityContainer>();
            var storage = entityTypeMappings.Where(m => m.)
        }

        private void GetPrimaryKey(LinqToEdmx.Model.ConceptualV3.EntityType table, DatabaseTable dbTable)
        {
            if (table.Key.PropertyRefs.Count() == 0) return;

            var pk = table.Key;
            var primaryKey = new DatabasePrimaryKey
            {
                // We do not have information about the primary key name in the model.
                // So we're making it up
                Name = string.Concat("PK_", dbTable.Name),
                Table = dbTable
            };

            // We do not know if the primary key is clustered because we're not limited to SQL Server Here. 
            //if (!pk.Clustered)
            //{
            //    primaryKey["SqlServer:Clustered"] = false;
            //}

            foreach (var pkCol in table.Properties)
            {
                var dbCol = dbTable.Columns
                    .Single(c => c.Name == pkCol.Name);

                primaryKey.Columns.Add(dbCol);
            }

            dbTable.PrimaryKey = primaryKey;

        }

    }
}