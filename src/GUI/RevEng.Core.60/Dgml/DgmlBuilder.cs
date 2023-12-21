using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;

namespace RevEng.Core.Dgml
{
    public class DgmlBuilder
    {
        private readonly string connectionString;
        private readonly ServiceProvider serviceProvider;
        private readonly List<string> schemas = Enumerable.Empty<string>().ToList();

        public DgmlBuilder(int databaseType, string connectionString, List<string> schemas)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), @"invalid connection string");
            }

            this.connectionString = connectionString.ApplyDatabaseType((DatabaseType)databaseType);

            var options = new ReverseEngineerCommandOptions
            {
                DatabaseType = (DatabaseType)databaseType,
                ConnectionString = connectionString,
            };

            this.schemas = schemas;

            serviceProvider = new ServiceCollection().AddEfpt(options, new List<string>(), new List<string>(), new List<string>()).BuildServiceProvider();
        }

        public string GetDgmlFileName()
        {
            var fileName = Path.Join(Path.GetTempPath(), Path.GetRandomFileName() + ".dgml");
            var model = GetModelInternal();

            var creator = new DatabaseModelToDgml(model, fileName);

            creator.CreateDgml();

            return fileName;
        }

        private DatabaseModel GetModelInternal()
        {
            var dbModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();

            var dbModelOptions = new DatabaseModelFactoryOptions(schemas: schemas);
            var dbModel = dbModelFactory!.Create(connectionString, dbModelOptions);

            return dbModel;
        }
    }
}
