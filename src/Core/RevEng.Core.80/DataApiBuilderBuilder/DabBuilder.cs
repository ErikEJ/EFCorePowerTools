﻿using System;
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
            if (options.ProjectPath == null)
            {
                return string.Empty;
            }

            var fileName = Path.Combine(options.ProjectPath, "dab-config.cmd");

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
            ////var procedures = GetStoredProcedures();

            var sb = new StringBuilder();

            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo off");

            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo This cmd file creates a Data API Builder configuration based on the chosen database objects.");
            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo To run the cmd, create an .env file with the following contents:");
            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo dab-connection-string=<your connection string>");
            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo ** Make sure to exclude the .env file from source control**");
            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo **");

            sb.AppendLine(CultureInfo.InvariantCulture, $"dotnet tool install -g Microsoft.DataApiBuilder");

            sb.AppendLine(CultureInfo.InvariantCulture, $"dab init -c dab-config.json --database-type {databaseType} --connection-string \"@env('dab-connection-string')\" --host-mode Development");

            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo Adding tables");

            foreach (var dbObject in model.Tables)
            {
                if (BreaksOn(dbObject))
                {
                    continue;
                }

                var columnList = string.Join(
                    ",",
                    dbObject.Columns
                    .Select(c => c.Name).ToList());

                var type = dbObject.Name.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase);

                if (dbObject.PrimaryKey != null)
                {
                   sb.AppendLine(CultureInfo.InvariantCulture, $"dab add \"{type}\" --source \"[{dbObject.Schema}].[{dbObject.Name}]\" --fields.include \"{columnList}\" --permissions \"anonymous:*\" ");
                }
            }

            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo Adding views and tables without primary key");

            foreach (var dbObject in model.Tables)
            {
                if (BreaksOn(dbObject))
                {
                    continue;
                }

                var columnList = string.Join(
                    ",",
                    dbObject.Columns
                    .Select(c => c.Name).ToList());

                var type = dbObject.Name.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase);

                if (dbObject.PrimaryKey == null)
                {
                    var strategy = "Id column";
                    var candidate = dbObject.Columns.FirstOrDefault(c => c.Name.Equals("id", StringComparison.OrdinalIgnoreCase));
                    if (candidate == null)
                    {
                        strategy = "first Id column";
                        candidate = dbObject.Columns.FirstOrDefault(c => c.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase));
                        if (candidate == null)
                        {
                            strategy = "first column";
                            candidate = dbObject.Columns[0];
                        }
                    }

                    sb.AppendLine(CultureInfo.InvariantCulture, $"@echo No primary key found for table/view '{dbObject.Name}', using {strategy} ({candidate.Name}) as key field");
                    sb.AppendLine(CultureInfo.InvariantCulture, $"dab add \"{type}\" --source \"[{dbObject.Schema}].[{dbObject.Name}]\" --fields.include \"{columnList}\" --source.key-fields \"{candidate.Name}\" --permissions \"anonymous:*\" ");
                }
            }

            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo Adding relationships");

            foreach (var dbObject in model.Tables)
            {
                if (BreaksOn(dbObject))
                {
                    continue;
                }

                var type = dbObject.Name.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase);

                foreach (var fk in dbObject.ForeignKeys)
                {
                    var fkType = fk.PrincipalTable.Name.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase);
                    sb.AppendLine(CultureInfo.InvariantCulture, $"dab update {type} --relationship {fkType} --target.entity {fkType} --cardinality one");
                    sb.AppendLine(CultureInfo.InvariantCulture, $"dab update {fkType} --relationship {type} --target.entity {type} --cardinality many");
                }
            }

            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo **");
            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo ** run 'dab validate' to validate your configuration **");
            sb.AppendLine(CultureInfo.InvariantCulture, $"@echo ** run 'dab start' to start the development API host **");

            File.WriteAllText(fileName, sb.ToString(), Encoding.ASCII);

            return fileName;
        }

        private static bool BreaksOn(DatabaseTable dbObject)
        {
            return dbObject.Columns
                .Any(c => c.StoreType == "hierarchyid" || c.StoreType == "geometry" || c.StoreType == "geography")
                || !dbObject.Columns.Any();
        }

        private DatabaseModel GetModelInternal()
        {
            var dbModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();

            var dbModelOptions = new DatabaseModelFactoryOptions(options.Tables.Where(t => t.ObjectType.HasColumns()).Select(m => m.Name), null);

            var dbModel = dbModelFactory!.Create(options.ConnectionString, dbModelOptions);

            return dbModel;
        }

        ////private RoutineModel GetStoredProcedures()
        ////{
        ////    var procedureModelFactory = serviceProvider.GetService<IProcedureModelFactory>();

        ////    if (procedureModelFactory == null)
        ////    {
        ////        return new RoutineModel();
        ////    }

        ////    var modelFactoryOptions = new ModuleModelFactoryOptions
        ////    {
        ////        DiscoverMultipleResultSets = false,
        ////        UseLegacyResultSetDiscovery = false,
        ////        UseDateOnlyTimeOnly = true,
        ////        FullModel = true,
        ////        Modules = options.Tables.Where(t => t.ObjectType == ObjectType.Procedure).Select(m => m.Name),
        ////        ModulesUsingLegacyDiscovery = options.Tables
        ////                .Where(t => t.ObjectType == ObjectType.Procedure && t.UseLegacyResultSetDiscovery)
        ////                .Select(m => m.Name),
        ////        MappedModules = new Dictionary<string, string>(),
        ////    };

        ////    return procedureModelFactory.Create(options.Dacpac ?? options.ConnectionString, modelFactoryOptions);
        ////}
    }
}
