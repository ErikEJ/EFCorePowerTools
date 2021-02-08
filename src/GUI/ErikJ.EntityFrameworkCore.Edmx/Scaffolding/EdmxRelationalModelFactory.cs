using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LinqToEdmx;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace ErikJ.EntityFrameworkCore.Edmx.Scaffolding
{
    // This is the conceptual part of the model
    public class EdmxRelationalModelFactory : IDatabaseModelFactory
    {
        // SQL Server db types
        private static readonly ISet<string> SQLServerDateTimePrecisionTypes = new HashSet<string> { "datetimeoffset", "datetime2", "time" };

        private static readonly ISet<string> SQLServerMaxLengthRequiredTypes
            = new HashSet<string> { "binary", "varbinary", "char", "varchar", "nchar", "nvarchar" };

        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

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

            // Detect the EDMX file version
            // FIXME Provide an alternative to not load the whole document in memory /!\
            XDocument edmxFile = XDocument.Load(edmxPath);

            var edmxVersion = ((XElement)edmxFile.FirstNode).FirstAttribute.Value;

            // FIXME Assume EDMX version is 3.0 for now

            if (string.Compare(edmxVersion, @"3.0", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                var edmxv3 = LinqToEdmx.EdmxV3.Load(edmxPath);

                // The conceptual entity name can be anything.
                // Most of the time it is the pluralized srore table name though.
                var entitySets = edmxv3.GetItems<LinqToEdmx.Model.ConceptualV3.EntityContainer>().First().EntitySets;

                foreach (var entity in entitySets)
                {
                    //var mappings = edmxv3.GetItems<LinqToEdmx.MapV3.EntitySetMapping>().First(esm => esm.Name == entity.Name).Name;
                    // We should go through the mapping to find the the store name of a conceptual entity
                    var storeName = edmxv3.GetItems<LinqToEdmx.MapV3.EntitySetMapping>().First(esm => esm.Name == entity.Name).EntityTypeMappings.First().MappingFragments.First().StoreEntitySet;
                    var item = edmxv3.GetItems<LinqToEdmx.Model.ConceptualV3.EntityType>().First(e => e.Name == entity.EntityType.Replace("Self.", string.Empty));

                    var dbTable = new DatabaseTable
                    {
                        Name = entity.Name,
                        Schema = GetSchemaNameV3(edmxv3, storeName),
                    };


                    GetColumnsV3(item, dbTable/*, typeAliases, model.GetObjects<TSqlDefaultConstraint>(DacQueryScopes.UserDefined).ToList()*/, edmxv3);
                    GetPrimaryKeyV3(item, dbTable);

                    dbModel.Tables.Add(dbTable);
                }

                foreach (var entity in entitySets)
                {
                    // We should go through the mapping to find the the store name of a conceptual entity
                    var storeName = edmxv3.GetItems<LinqToEdmx.MapV3.EntitySetMapping>().First(esm => esm.Name == entity.Name).EntityTypeMappings.First().MappingFragments.First().StoreEntitySet;

                    GetForeignKeysV3(edmxv3, entity.Name, storeName, dbModel);

                    // Unique constraints are not available in the model
                    // GetUniqueConstraints(item, dbModel);

                    // Indexes are not available in the model
                    // GetIndexes(item, dbModel);
                }
            }

            return dbModel;
        }

        public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
        {
            throw new NotImplementedException();
        }

        // FIXME There could be a table or a view with the same name in multiple schema /!\
        internal string GetSchemaNameV3(EdmxV3 edmxv3, string entityName)
        {
            var entitySetContainer = edmxv3.GetItems<LinqToEdmx.Model.StorageV3.EntityContainer>();
            foreach (var container in entitySetContainer)
            {
                foreach (var entitySet in container.EntitySets.Where(es => es.Type == "Tables"))
                {
                    if (entitySet.Name == entityName)
                    {
                        if (string.IsNullOrEmpty(entitySet.Schema1) && string.IsNullOrEmpty(entitySet.Schema))
                        {
                            throw new InvalidOperationException(@$"[{entityName}] schema could not be null. This usually indicates a bug");
                        }
                        return entitySet.Schema ?? entitySet.Schema1;
                    }
                }

                foreach (var entitySet in container.EntitySets.Where(es => es.Type == "Views"))
                {
                    if (entitySet.EntityType.Contains(entityName))
                    {
                        if (string.IsNullOrEmpty(entitySet.Schema1) && string.IsNullOrEmpty(entitySet.Schema))
                        {
                            throw new InvalidOperationException(@$"[{entityName}] schema could not be null. This usually indicates a bug");
                        }
                        return entitySet.Schema ?? entitySet.Schema1;
                    }
                }
            }
            throw new InvalidOperationException(@$"Unable to identify the database object schema for entity [{entityName}]. This usually indicates a bug");
        }

        private void GetColumnsV3(LinqToEdmx.Model.ConceptualV3.EntityType item, DatabaseTable dbTable/*, IReadOnlyDictionary<string, (string storeType, string typeName)> typeAliases, List<TSqlDefaultConstraint> defaultConstraints,*/, LinqToEdmx.EdmxV3 model)
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
                // Default constraints are not available in the model.
                // var def = defaultConstraints.Where(d => d.TargetColumn.First().Name.ToString() == col.Name.ToString()).FirstOrDefault();
                string storeType = GetStoreTypeV3(col);
                string defaultValue = null; //def != null ? FilterClrDefaults(systemTypeName, col.Nullable, def.Expression.ToLowerInvariant()) : null;

                var dbColumn = new DatabaseColumn
                {
                    Table = dbTable,
                    Name = col.Name,
                    IsNullable = col.Nullable,
                    StoreType = storeType,
                    DefaultValueSql = defaultValue,

                    // ComputedColumnSQL is not available in the model
                    // ComputedColumnSql =,

                    // ValueGenerated is not available in the model
                    // ValueGenerated = col.StoreGeneratedPattern == "Identity"
                    //    ? ValueGenerated.OnAdd
                    //    : storeType == "rowversion"
                    //        ? ValueGenerated.OnAddOrUpdate
                    //        : default(ValueGenerated?)
                };

                if (storeType == "rowversion")
                {
                    dbColumn["ConcurrencyToken"] = true;
                }

                //var description = model.GetObjects<TSqlExtendedProperty>(DacQueryScopes.UserDefined)
                //    .Where(p => p.Name.Parts.Count == 5)
                //    .Where(p => p.Name.Parts[0] == "SqlColumn")
                //    .Where(p => p.Name.Parts[1] == dbTable.Schema)
                //    .Where(p => p.Name.Parts[2] == dbTable.Name)
                //    .Where(p => p.Name.Parts[3] == dbColumn.Name)
                //    .Where(p => p.Name.Parts[4] == "MS_Description")
                //    .FirstOrDefault();

                // Description is not available in the model.
                //dbColumn.Comment = FixExtendedPropertyValue(description?.Value);

                dbTable.Columns.Add(dbColumn);
            }
        }

        // FIXME Add provider string to deal with provider specific data type string to be returned. Only SQL Server for now.
        private string GetStoreTypeV3(LinqToEdmx.Model.ConceptualV3.EntityProperty col)
        {
            // SQL Server
            string dataTypeName = col.Type.ToString();

            if (dataTypeName == "timestamp" || dataTypeName == "uniqueidentifier")
            {
                return "rowversion";
            }

            if (dataTypeName == "decimal"
                || dataTypeName == "numeric")
            {
                return $"{dataTypeName}({col.Precision}, {col.Scale})";
            }

            if (SQLServerDateTimePrecisionTypes.Contains(dataTypeName)
                && col.Scale != 7)
            {
                return $"{dataTypeName}({col.Scale})";
            }

            if (SQLServerMaxLengthRequiredTypes.Contains(dataTypeName))
            {
                // FIXME how is max implemented in SSDL for SQL Server?
                if (int.Parse(col.MaxLength.ToString()) == -1)
                {
                    return $"{dataTypeName}(max)";
                }

                return $"{dataTypeName}({col.MaxLength})";
            }

            // Integral type
            return col.Type.ToString();

            // FIXME PostgreSQL

            // FIXME MySQL
        }

        private void GetPrimaryKeyV3(LinqToEdmx.Model.ConceptualV3.EntityType conceptualTable, DatabaseTable dbTable)
        {
            if (conceptualTable.Key.PropertyRefs.Count() == 0) return;

            var pk = conceptualTable.Key;
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

            foreach (var pkCol in conceptualTable.Key.PropertyRefs)
            {
                var dbCol = dbTable.Columns
                    .Single(c => c.Name == pkCol.Name);

                primaryKey.Columns.Add(dbCol);
            }

            dbTable.PrimaryKey = primaryKey;

        }

        private void GetForeignKeysV3(EdmxV3 model, string conceptualTableName, string storeTableName, DatabaseModel dbModel)
        {
            var table = dbModel.Tables
                .Single(t => t.Name == conceptualTableName
                && t.Schema == GetSchemaNameV3(model, storeTableName));

            // The foreign key informations are stored in the Association Object
            var associations = model.GetItems<LinqToEdmx.Model.StorageV3.Association>().Where(a => a.ReferentialConstraint.Dependent.Role == conceptualTableName);

            if (!associations.Any())
            {
                // Give a chance to a reflexive constraint
                // TODO make sure that this is the right way to deal with this case.
                var endType = string.Concat(@"Self.", conceptualTableName);
                associations = model.GetItems<LinqToEdmx.Model.StorageV3.Association>().Where(a => a.Ends.All(e => e.Type == endType));
            }

            // No association ? No FK then.
            if (!associations.Any())
                return;

            foreach (var association in associations)
            {
                // The entity name could be derived if present multiple times (a table has a foreign key to himself => See ProductCategory which has a parent ProductCategory)
                // Te association set allows us to disambiguate the table name.
                var associationSet = model.GetItems<LinqToEdmx.Model.StorageV3.EntityContainer>().First().AssociationSets.Where(@as => @as.Name == association.Name).SingleOrDefault();

                var entityName = association.ReferentialConstraint.Principal.Role;


                // Look for the table to which the columns are constrained
                // Remember that we could be constrained to ourself :)
                var principalTable = dbModel.Tables.SingleOrDefault(t => t.Name == entityName && t.Schema == GetSchemaNameV3(model, entityName));
                if (principalTable == null)
                {
                    entityName = associationSet.Ends.Single(e => e.Role == entityName).EntitySet;
                    principalTable = dbModel.Tables.SingleOrDefault(t => t.Name == entityName && t.Schema == GetSchemaNameV3(model, entityName));
                }
                if (principalTable == null)
                {
                    throw new InvalidOperationException(@$"Unable to get foreign keys for entity with name [{entityName}]. This usually indicates a bug");
                }

                var foreignKey = new DatabaseForeignKey
                {
                    // The name of the foreign key
                    Name = association.Name,
                    // The table that contains the foreign key constraint (Dependent in the Edmx)
                    Table = table,
                    // The table to which the columns are constrained (Principal in the Edmx)
                    PrincipalTable = principalTable,

                };

                var end = associationSet.Ends[0];

                // OnDelete = ConvertToReferentialAction(fk.DeleteAction);

                // Finish to populate the foreign key definition with principal and dependent columns
                var rc = association.ReferentialConstraint;

                foreach (var fkCol in association.ReferentialConstraint.Principal.PropertyRefs)
                {
                    var dbCol = principalTable.Columns
                        .Single(c => c.Name == fkCol.Name);

                    if (dbCol != null)
                    {
                        foreignKey.PrincipalColumns.Add(dbCol);
                    }
                }
                foreach (var fkCol in association.ReferentialConstraint.Dependent.PropertyRefs)
                {
                    // Best case scenario, the columns name are the same in both entities
                    var dbCol = table.Columns
                        .Single /*OrDefault*/(c => c.Name == fkCol.Name);
                    if (dbCol != null)
                    {
                        foreignKey.Columns.Add(dbCol);
                    }
                }

                if (foreignKey.PrincipalColumns.Count > 0)
                {
                    table.ForeignKeys.Add(foreignKey);
                }
            }
        }
    }
}
