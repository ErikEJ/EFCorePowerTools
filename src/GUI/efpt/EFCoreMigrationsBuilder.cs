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
            return BuildMigrationResult(outputPath, null, contextName, false, false, null, null);
        }

        public List<Tuple<string, string>> AddMigration(string outputPath, string projectPath, string contextName, string migrationIdentifier, string nameSpace)
        {
            return BuildMigrationResult(outputPath, projectPath, contextName, true, false, migrationIdentifier, nameSpace);
        }

        public List<Tuple<string, string>> ScriptMigration(string outputPath, string contextName)
        {
            return BuildMigrationResult(outputPath, null, contextName, false, true, null, null);
        }

        private List<Tuple<string, string>> BuildMigrationResult(string outputPath, string projectPath, string contextName, bool addMigration, bool scriptMigration, string migrationIdentifier, string nameSpace)
        {
            var result = new List<Tuple<string, string>>();
            var operations = GetOperations(outputPath);
            var types = GetDbContextTypes(operations);

            foreach (var type in types)
            {
                if (type.Name == contextName)
                {
                    var dbContext = operations.CreateContext(type.Name);
                    if (scriptMigration)
                    {
                        result.Add(new Tuple<string, string>(type.Name,  ScriptMigration(dbContext, outputPath)));
                    }
                    else
                    {
                        result.Add(addMigration
                            ? new Tuple<string, string>(type.Name + "Add", AddMigration(dbContext, outputPath, projectPath, migrationIdentifier, nameSpace))
                            : new Tuple<string, string>(type.Name + "Apply", ApplyMigrations(dbContext)));
                    }
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
                try
                {
                    var dbContext = operations.CreateContext(type.Name);
                    result.Add(new Tuple<string, string>(type.Name, GetMigrationStatus(dbContext)));
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
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

        private string ScriptMigration(DbContext context, string outputPath)
        {
            var servicesBuilder = GetDesignTimeServicesBuilder(outputPath);

            var services = servicesBuilder.Build(context);

            EnsureServices(services);

            var migrator = services.GetRequiredService<IMigrator>();

            return migrator.GenerateScript(null, null, idempotent: true);
        }

        private string ApplyMigrations(DbContext context)
        {
            context.Database.Migrate();

            return "Done";
        }

        private string AddMigration(DbContext context, string outputPath, string projectPath, string name, string nameSpace)
        {
            var result = AddMigration(name, outputPath, projectPath, nameSpace, context);
            string status = result.MetadataFile + Environment.NewLine;
            status += result.MigrationFile + Environment.NewLine;
            status += result.SnapshotFile + Environment.NewLine;
            return status;
        }

        private MigrationFiles AddMigration(
            string name,
            string outputPath,
            string projectPath,
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

            var scaffolder = services.GetRequiredService<IMigrationsScaffolder>();
            var migration = scaffolder.ScaffoldMigration(name, nameSpace);

            return scaffolder.Save(projectPath, migration, null);
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

            return new DbContextOperations(reporter, assembly, assembly, Array.Empty<string>());
        }

        private DesignTimeServicesBuilder GetDesignTimeServicesBuilder(string outputPath)
        {
            var assembly = Load(outputPath);
            if (assembly == null)
            {
                throw new ArgumentException("Unable to load project assembly");
            }

            var reporter = new OperationReporter(
                new OperationReportHandler());

#if CORE21
            return new DesignTimeServicesBuilder(assembly, reporter, Array.Empty<string>());
#else
#if CORE22
            return new DesignTimeServicesBuilder(assembly, assembly, reporter, Array.Empty<string>());
#endif
#endif
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
