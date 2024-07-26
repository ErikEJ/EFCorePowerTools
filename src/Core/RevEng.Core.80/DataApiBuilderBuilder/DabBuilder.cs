using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;
using RevEng.Common.Dab;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;

namespace RevEng.Core.DataApiBuilderBuilder
{
    public class DabBuilder
    {
        private readonly ServiceProvider serviceProvider;
        private readonly DataApiBuilderOptions options;

        public DabBuilder(DataApiBuilderOptions dataApiBuilderCommandOptions)
        {
            ArgumentNullException.ThrowIfNull(dataApiBuilderCommandOptions);

            options = dataApiBuilderCommandOptions;

            options.ConnectionString = dataApiBuilderCommandOptions.ConnectionString.ApplyDatabaseType(dataApiBuilderCommandOptions.DatabaseType);

            var revEngOptions = new ReverseEngineerCommandOptions
            {
                DatabaseType = dataApiBuilderCommandOptions.DatabaseType,
                ConnectionString = options.ConnectionString,
                Dacpac = dataApiBuilderCommandOptions.Dacpac,
            };

            serviceProvider = new ServiceCollection().AddEfpt(revEngOptions, new List<string>(), new List<string>(), new List<string>()).BuildServiceProvider();
        }

        public string GetDabConfigCmdFile()
        {
            var folder = Path.GetDirectoryName(options.ProjectPath);

            if (folder == null)
            {
                return string.Empty;
            }

            var fileName = Path.Combine(folder, "dab-config.cmd");

            string databaseType = string.Empty;

            switch (options.DatabaseType)
            {
                case DatabaseType.Undefined:
                    break;
                case DatabaseType.SQLServer:
                    databaseType = "mssql";
                    break;
                case DatabaseType.SQLite:
                    break;
                case DatabaseType.Npgsql:
                    databaseType = "postgresql";
                    break;
                case DatabaseType.Mysql:
                    databaseType = "mysql";
                    break;
                case DatabaseType.Oracle:
                    break;
                case DatabaseType.SQLServerDacpac:
                    break;
                case DatabaseType.Firebird:
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(databaseType))
            {
                return string.Empty;
            }

            var model = GetModelInternal();
            var procedures = GetStoredProcedures();

            var sb = new StringBuilder();

            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo off");

            sb.AppendLine(CultureInfo.InvariantCulture, $"dotnet tool install -g Microsoft.DataApiBuilder");

            sb.AppendLine(CultureInfo.InvariantCulture, $"dab init -c dab-config.json --database-type {databaseType} --connection-string \"@env('dab-connection-string')\" --host-mode Development");

            foreach (var dbObject in model.Tables)
            {
                var columns = dbObject.Columns.Select(c => c.Name).ToList();
                var columnList = string.Join(",", columns);

                if (dbObject.PrimaryKey != null)
                {
                   var type = dbObject.Name.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase);
                   sb.AppendLine(CultureInfo.InvariantCulture, $"dab add \"{type}\" --source \"[{dbObject.Schema}].[{dbObject.Name}]\" --fields.include \"{columnList}\" --permissions \"anonymous:*\" ");
                }

                //TODO: guess and set primary key for views --source.key-fields
            }

            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo run 'dab validate' to validate your configuration");
            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo run 'dab start' to start the development API host");

            File.WriteAllText(fileName, sb.ToString(), Encoding.ASCII);

            return fileName;
        }

        private DatabaseModel GetModelInternal()
        {
            var dbModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();

            var dbModelOptions = new DatabaseModelFactoryOptions(options.Tables.Where(t => t.ObjectType.HasColumns()).Select(m => m.Name), null);

            var dbModel = dbModelFactory!.Create(options.ConnectionString, dbModelOptions);

            return dbModel;
        }

        private RoutineModel GetStoredProcedures()
        {
            var procedureModelFactory = serviceProvider.GetService<IProcedureModelFactory>();

            if (procedureModelFactory == null)
            {
                return new RoutineModel();
            }

            var modelFactoryOptions = new ModuleModelFactoryOptions
            {
                DiscoverMultipleResultSets = false,
                UseLegacyResultSetDiscovery = false,
                UseDateOnlyTimeOnly = true,
                FullModel = true,
                Modules = options.Tables.Where(t => t.ObjectType == ObjectType.Procedure).Select(m => m.Name),
                ModulesUsingLegacyDiscovery = options.Tables
                        .Where(t => t.ObjectType == ObjectType.Procedure && t.UseLegacyResultSetDiscovery)
                        .Select(m => m.Name),
                MappedModules = new Dictionary<string, string>(),
            };

            return procedureModelFactory.Create(options.Dacpac ?? options.ConnectionString, modelFactoryOptions);
        }
    }
}
