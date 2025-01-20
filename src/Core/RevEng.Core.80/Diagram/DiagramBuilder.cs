﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;

namespace RevEng.Core.Diagram
{
    public class DiagramBuilder
    {
        private readonly string connectionString;
        private readonly ServiceProvider serviceProvider;
        private readonly List<string> schemas = Enumerable.Empty<string>().ToList();

        public DiagramBuilder(int databaseType, string connectionString, List<string> schemas)
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

            serviceProvider = new ServiceCollection().AddEfpt(options, [], [], []).BuildServiceProvider();
        }

        public string GetDgmlFileName()
        {
            var fileName = Path.Join(Path.GetTempPath(), Path.GetRandomFileName() + ".dgml");
            var model = GetModelInternal();

            var creator = new DatabaseModelToDgml(model, fileName);

            creator.CreateDgml();

            return fileName;
        }

        public string GetErDiagramFileName()
        {
            var model = GetModelInternal();

            var creator = new DatabaseModelToMermaid(model);

            var diagram = creator.CreateMermaid(createMarkdown: false);

            var fileName = Path.Join(Path.GetTempPath(), Path.GetRandomFileName() + ".mmd");
            File.WriteAllText(fileName, diagram, Encoding.UTF8);

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
