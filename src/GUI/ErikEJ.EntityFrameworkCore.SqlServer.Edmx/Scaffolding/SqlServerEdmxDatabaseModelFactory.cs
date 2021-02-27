using LinqToEdmx;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ErikEJ.EntityFrameworkCore.SqlServer.Edmx.Scaffolding
{
    // This is the storage side of the scaffolding logic
    // Because the EDMX format was not designed with the database creation in mind, the SSDL part of the schema won't contains default values, extended properties, primary keys, unique constraints and index informations.
    public class SqlServerEdmxDatabaseModelFactory : IDatabaseModelFactory
    {
        // SQL Server db types
        private static readonly ISet<string> SQLServerDateTimePrecisionTypes = new HashSet<string> { "datetimeoffset", "datetime2", "time" };

        private static readonly ISet<string> SQLServerMaxLengthRequiredTypes
            = new HashSet<string> { "binary", "varbinary", "char", "varchar", "nchar", "nvarchar" };

        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public SqlServerEdmxDatabaseModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
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
                            throw new InvalidOperationException($"[{entityName}] schema could not be null. This usually indicates a bug");
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
                            throw new InvalidOperationException($"[{entityName}] schema could not be null. This usually indicates a bug");
                        }
                        return entitySet.Schema ?? entitySet.Schema1;
                    }
                }
            }
            throw new InvalidOperationException($"Unable to identify the database object schema for entity [{entityName}]. This usually indicates a bug");
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

            // Detect the EDMX file version
            XDocument edmxFile = XDocument.Load(edmxPath);

            var edmxVersion = ((XElement)edmxFile.FirstNode).FirstAttribute.Value;

            if (string.Compare(edmxVersion, @"3.0", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                var edmxv3 = EdmxV3.Load(edmxPath);
                if (!edmxv3.Runtimes[0].StorageModels.StorageSchema.Provider.Equals("System.Data.SqlClient", StringComparison.Ordinal))
                {
                    throw new NotSupportedException("Only SQL Server EDMX files are currently supported");
                }

                var items = edmxv3.GetItems<LinqToEdmx.Model.StorageV3.EntityTypeStore>();

                foreach (var item in items)
                {
                    var dbTable = new DatabaseTable
                    {
                        Name = item.Name,
                        Schema = GetSchemaNameV3(edmxv3, item.Name),
                    };

                    GetColumnsV3(item, dbTable, edmxv3);
                    GetPrimaryKeyV3(item, dbTable);

                    dbModel.Tables.Add(dbTable);
                }

                foreach (var item in items)
                {
                    GetForeignKeysV3(edmxv3, item, dbModel);
                }
            }
            else
            {
                throw new NotSupportedException("Only V3 edmx files supported.");
            }

            return dbModel;
        }

        public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
        {
            throw new NotImplementedException();
        }

        private void GetColumnsV3(LinqToEdmx.Model.StorageV3.EntityTypeStore item, DatabaseTable dbTable/*, IReadOnlyDictionary<string, (string storeType, string typeName)> typeAliases, List<TSqlDefaultConstraint> defaultConstraints,*/, LinqToEdmx.EdmxV3 model)
        {
            var tableColumns = item.Properties;

            foreach (var col in tableColumns)
            {
                string storeType = GetStoreTypeV3(col);
                string defaultValue = null;

                var dbColumn = new DatabaseColumn
                {
                    Table = dbTable,
                    Name = col.Name,
                    IsNullable = col.Nullable,
                    StoreType = storeType,
                    DefaultValueSql = defaultValue,
                };
                if (storeType == "rowversion")
                {
                    dbColumn["ConcurrencyToken"] = true;
                }

                dbTable.Columns.Add(dbColumn);
            }
        }

        private string GetStoreTypeV3(LinqToEdmx.Model.StorageV3.EntityProperty col)
        {
            // SQL Server
            string dataTypeName = col.Type.ToString();

            if (dataTypeName == "timestamp")
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
        }

        private void GetPrimaryKeyV3(LinqToEdmx.Model.StorageV3.EntityTypeStore table, DatabaseTable dbTable)
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

            foreach (var pkCol in table.Key.PropertyRefs)
            {
                var dbCol = dbTable.Columns
                    .Single(c => c.Name == pkCol.Name);

                primaryKey.Columns.Add(dbCol);
            }

            dbTable.PrimaryKey = primaryKey;

        }

        private void GetForeignKeysV3(EdmxV3 model, LinqToEdmx.Model.StorageV3.EntityTypeStore storeTable, DatabaseModel dbModel)
        {
            var table = dbModel.Tables
                .Single(t => t.Name == storeTable.Name
                && t.Schema == GetSchemaNameV3(model, storeTable.Name));

            // The foreign key informations are stored in the Association Object
            var associations = model.GetItems<LinqToEdmx.Model.StorageV3.Association>().Where(a => a.ReferentialConstraint.Dependent.Role == storeTable.Name);

            if (!associations.Any())
            {
                // Give a chance to a reflexive constraint
                // TODO make sure that this is the right way to deal with this case.
                var endType = string.Concat(@"Self.", storeTable.Name);
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
                if ( principalTable == null)
                {
                    entityName = associationSet.Ends.Single(e => e.Role == entityName).EntitySet;
                    principalTable = dbModel.Tables.SingleOrDefault(t => t.Name == entityName && t.Schema == GetSchemaNameV3(model, entityName));
                }
                if (principalTable == null)
                {
                    throw new InvalidOperationException($"Unable to get foreign keys for entity with name [{entityName}]. This usually indicates a bug");
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