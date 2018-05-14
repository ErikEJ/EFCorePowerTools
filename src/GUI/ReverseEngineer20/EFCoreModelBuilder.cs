using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReverseEngineer20
{
    public class EfCoreModelBuilder
    {
        public List<Tuple<string, string>> GenerateDebugView(string outputPath)
        {
            return BuildResult(outputPath, false);
        }

        public List<Tuple<string, string>> GenerateDatabaseCreateScript(string outputPath)
        {
            return BuildResult(outputPath, true);
        }

        public List<Tuple<string, string>> GenerateMigrationStatusList(string outputPath)
        {
            return GetMigrationStatus(outputPath);
        }

        public List<Tuple<string, string>> Migrate(string outputPath, string contextName)
        {
            return BuildMigrationResult(outputPath, contextName, false, null);
        }

        public List<Tuple<string, string>> AddMigration(string outputPath, string contextName, string migrationIdentifier)
        {
            return BuildMigrationResult(outputPath, contextName, true, migrationIdentifier);
        }

        private List<Tuple<string, string>> BuildResult(string outputPath, bool generateDdl)
        {
            var result = new List<Tuple<string, string>>();
            var operations = GetOperations(outputPath);
            var types = GetDbContextTypes(operations);

            foreach (var type in types)
            {
                var dbContext = operations.CreateContext(type.Name);
                result.Add(generateDdl
                    ? new Tuple<string, string>(type.Name, GenerateCreateScript(dbContext))
                    : new Tuple<string, string>(type.Name, dbContext.Model.AsModel().DebugView.View));
            }

            return result;
        }

        private List<Tuple<string, string>> BuildMigrationResult(string outputPath, string contextName, bool addMigration, string migrationIdentifier)
        {
            var result = new List<Tuple<string, string>>();
            var operations = GetOperations(outputPath);
            var types = GetDbContextTypes(operations);

            foreach (var type in types)
            {
                if (type.Name == contextName)
                {
                    var dbContext = operations.CreateContext(type.Name);
                    result.Add(addMigration 
                        ? new Tuple<string, string>(type.Name, ApplyMigrations(dbContext))
                        : new Tuple<string, string>(type.Name, AddMigration(dbContext, migrationIdentifier)));
                    break;
                }
            }

            return result;
        }

        private List<Tuple<string, string>> GetMigrationStatus(string outputPath)
        {
            var result = new List<Tuple<string, string>>();
            var operations = GetOperations(outputPath);
            var types = GetDbContextTypes(operations);

            foreach (var type in types)
            {
                var dbContext = operations.CreateContext(type.Name);
                result.Add(new Tuple<string, string>(type.Name, GetMigrationStatus(dbContext)));
            }
            return result;
        }

        private string GetMigrationStatus(DbContext context)
        {
            var relationalDatabaseCreator = context.GetService<IDatabaseCreator>() as IRelationalDatabaseCreator;
            if (relationalDatabaseCreator == null)
            {
                throw new Exception("Not a relational database, migrations are not supported");
            }
            var databaseExists = relationalDatabaseCreator.Exists();

            var migrationsAssembly = context.GetService<IMigrationsAssembly>();
            var modelDiffer = context.GetService<IMigrationsModelDiffer>();

            var pendingModelChanges
                = (!databaseExists || migrationsAssembly.ModelSnapshot != null)
                    && modelDiffer.HasDifferences(migrationsAssembly.ModelSnapshot?.Model, context.Model);

            if (pendingModelChanges) return "Changes";

            var pendingMigrations
                = (databaseExists
                    ? context.Database.GetPendingMigrations()
                    : context.Database.GetMigrations())
                .ToArray();

            if (pendingMigrations.Any()) return "Pending";

            return "InSync";
        }

        private string ApplyMigrations(DbContext context)
        {
            context.Database.Migrate();

            return GetMigrationStatus(context);
        }

        private string AddMigration(DbContext context, string name)
        {
            // ??? How to do it?? context.Database.Migrate();

            return GetMigrationStatus(context);
        }

        private static string GenerateCreateScript(DbContext dbContext)
        {
            var database = dbContext.Database;
            var model = database.GetService<IModel>();
            var differ = database.GetService<IMigrationsModelDiffer>();
            var generator = database.GetService<IMigrationsSqlGenerator>();
            var sql = database.GetService<ISqlGenerationHelper>();

            var operations = differ.GetDifferences(null, model);
            var commands = generator.Generate(operations, model);

            var builder = new StringBuilder();
            foreach (var command in commands)
            {
                builder
                    .Append(command.CommandText)
                    .AppendLine(sql.BatchTerminator);
            }

            return builder.ToString();
        }

        private List<Type> GetDbContextTypes(DbContextOperations operations)
        {
            var types = operations.GetContextTypes().ToList();
            if (types.Count == 0)
            {
                throw new ArgumentException("No EF Core DbContext types found in the project");
            }
            return types;
        }

        private DbContextOperations GetOperations(string outputPath)
        {
            var assembly = Load(outputPath);
            if (assembly == null)
            {
                throw new ArgumentException("Unable to load project assembly");
            }

            var reporter = new OperationReporter(
                new OperationReportHandler());

            var operations = new DbContextOperations(reporter, assembly, assembly);
            return operations;
        }

        private Assembly Load(string assemblyPath)
        {
            return File.Exists(assemblyPath) ? Assembly.LoadFile(assemblyPath) : null;
        }
    }
}
