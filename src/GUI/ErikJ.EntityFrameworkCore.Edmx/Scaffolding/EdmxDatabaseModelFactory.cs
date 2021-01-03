using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using LinqToEdmx;
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