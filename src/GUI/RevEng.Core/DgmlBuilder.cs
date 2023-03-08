using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;
using RevEng.Core.Dgml;

namespace RevEng.Core
{
    public class DgmlBuilder
    {
        private readonly string connectionString;
        private readonly ServiceProvider serviceProvider;

        public DgmlBuilder(int databaseType, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), @"invalid connection string");
            }

            this.connectionString = SqlServerHelper.SetConnectionString((DatabaseType)databaseType, connectionString);

            var options = new ReverseEngineerCommandOptions
            {
                DatabaseType = (DatabaseType)databaseType,
                ConnectionString = connectionString,
            };

            serviceProvider = ServiceProviderBuilder.Build(options, new List<string>(), new List<string>(), new List<string>());
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

            var dbModelOptions = new DatabaseModelFactoryOptions();
            var dbModel = dbModelFactory!.Create(connectionString, dbModelOptions);

            return dbModel;
        }
    }
}
