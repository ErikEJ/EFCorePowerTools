using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReverseEngineer20
{
    public class EfCoreMigrationsBuilder
    {
        public List<Tuple<string, string>> GenerateMigrationStatusList(string outputPath)
        {
            return GetMigrationStatus(outputPath);
        }

        public List<Tuple<string, string>> Migrate(string outputPath, string contextName)
        {
            return BuildMigrationResult(outputPath, contextName, false, null, null);
        }

        public List<Tuple<string, string>> AddMigration(string outputPath, string contextName, string migrationIdentifier, string nameSpace)
        {
            return BuildMigrationResult(outputPath, contextName, true, migrationIdentifier, nameSpace);
        }

        private List<Tuple<string, string>> BuildMigrationResult(string outputPath, string contextName, bool addMigration, string migrationIdentifier, string nameSpace)
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
                        : new Tuple<string, string>(type.Name, AddMigration(dbContext, outputPath, migrationIdentifier, nameSpace)));
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

            var migrations = context.Database.GetMigrations().ToArray();

            if (!migrations.Any()) return "NoMigrations";

            var pendingMigrations
                = (databaseExists
                    ? context.Database.GetPendingMigrations()
                    : migrations)
                .ToArray();

            if (pendingMigrations.Any()) return "Pending";

            return "InSync";
        }

        private string ApplyMigrations(DbContext context)
        {
            context.Database.Migrate();

            return GetMigrationStatus(context);
        }

        private string AddMigration(DbContext context, string outputPath, string name, string nameSpace)
        {
            //Add files to result!!
            return GetMigrationStatus(context);
        }

        public virtual MigrationFiles AddMigration(
            string name,
            string outputPath,
            string nameSpace,
            DbContext context)
        {
            var servicesBuilder = GetDesignTimeServicesBuilder(outputPath);

            var services = servicesBuilder.Build(context);

            EnsureServices(services);

            var assembly = Load(outputPath);
            if (assembly == null)
            {
                throw new ArgumentException("Unable to load project assembly");
            }
            EnsureMigrationsAssembly(services, assembly);

            //For 2.1 use:
            //var scaffolder = services.GetRequiredService<IMigrationsScaffolder>();
            //var migration = scaffolder.ScaffoldMigration(name, nameSpace, subNamespace, _language);

            var scaffolder = services.GetRequiredService<MigrationsScaffolder>();
            var migration = scaffolder.ScaffoldMigration(name, nameSpace);
            var files = scaffolder.Save(Path.GetDirectoryName(outputPath), migration, null);

            return files;
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

            //TODO Use OperationHandler output!!
            var reporter = new OperationReporter(
                new OperationReportHandler());

            var operations = new DbContextOperations(reporter, assembly, assembly);
            return operations;
        }

        private DesignTimeServicesBuilder GetDesignTimeServicesBuilder(string outputPath)
        {
            var assembly = Load(outputPath);
            if (assembly == null)
            {
                throw new ArgumentException("Unable to load project assembly");
            }

            //TODO Use OperationHandler output!!
            var reporter = new OperationReporter(
                new OperationReportHandler());

            var builder = new DesignTimeServicesBuilder(assembly, reporter);
            return builder;
        }

        private Assembly Load(string assemblyPath)
        {
            return File.Exists(assemblyPath) ? Assembly.LoadFile(assemblyPath) : null;
        }

        private static void EnsureServices(IServiceProvider services)
        {
            var migrator = services.GetService<IMigrator>();
            if (migrator == null)
            {
                var databaseProvider = services.GetService<IDatabaseProvider>();
                throw new OperationException(DesignStrings.NonRelationalProvider(databaseProvider?.Name ?? "Unknown"));
            }
        }

        private void EnsureMigrationsAssembly(IServiceProvider services, Assembly assembly)
        {
            var assemblyName = assembly.GetName();
            var options = services.GetRequiredService<IDbContextOptions>();
            var contextType = services.GetRequiredService<ICurrentDbContext>().Context.GetType();
            var migrationsAssemblyName = RelationalOptionsExtension.Extract(options).MigrationsAssembly
                                         ?? contextType.GetTypeInfo().Assembly.GetName().Name;
            if (assemblyName.Name != migrationsAssemblyName
                && assemblyName.FullName != migrationsAssemblyName)
            {
                throw new OperationException(
                    DesignStrings.MigrationsAssemblyMismatch(assemblyName.Name, migrationsAssemblyName));
            }
        }
    }
}
