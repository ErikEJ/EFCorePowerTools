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

            // Assume EDMX version is 3.0
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
                }
            }

            return dbModel;
        }

        public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}