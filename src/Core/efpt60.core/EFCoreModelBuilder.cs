using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Modelling
{
    internal static class EfCoreModelBuilder
    {
#pragma warning disable CA1002 // Do not expose generic lists
        public static List<Tuple<string, string>> GenerateDebugView(string outputPath, string startupOutputPath)
        {
            return BuildResult(outputPath, startupOutputPath ?? outputPath, false);
        }

        public static List<Tuple<string, string>> GenerateDatabaseCreateScript(string outputPath, string startupOutputPath)
        {
            return BuildResult(outputPath, startupOutputPath ?? outputPath, true);
        }
#pragma warning restore CA1002 // Do not expose generic lists

        private static List<Tuple<string, string>> BuildResult(string outputPath, string startupOutputPath, bool generateDdl)
        {
            var result = new List<Tuple<string, string>>();
            var operations = GetOperations(outputPath, startupOutputPath);
            var types = GetDbContextTypes(operations);

            foreach (var type in types)
            {
                var dbContext = operations.CreateContext(type.Name);

                var generated = generateDdl
                    ? GenerateCreateScript(dbContext)
                    : dbContext.GetService<IDesignTimeModel>().Model.ToDebugString(MetadataDebugStringOptions.LongDefault);

                result.Add(new Tuple<string, string>(type.Name, generated));
            }

            return result;
        }

        private static string GenerateCreateScript(DbContext dbContext)
        {
            var database = dbContext.Database;
            var migrator = database.GetService<IMigrator>();

            if (migrator == null)
            {
                var databaseProvider = database.GetService<IDatabaseProvider>();
                throw new OperationException(DesignStrings.NonRelationalProvider(databaseProvider?.Name ?? "Unknown"));
            }

            return migrator.GenerateScript(null, null, MigrationsSqlGenerationOptions.Idempotent);
        }

        private static List<Type> GetDbContextTypes(DbContextOperations operations)
        {
            var types = operations.GetContextTypes().ToList();
            if (types.Count == 0)
            {
                throw new ArgumentException("No EF Core DbContext types found in the project");
            }

            return types;
        }

        private static DbContextOperations GetOperations(string outputPath, string startupOutputPath)
        {
            var assembly = Load(outputPath);
            if (assembly == null)
            {
                throw new ArgumentException("Unable to load project assembly");
            }

            Assembly startupAssembly = null;

            if (startupOutputPath != outputPath)
            {
                startupAssembly = Load(startupOutputPath);
            }

            var reporter = new OperationReporter(
                new OperationReportHandler());

#if CORE90
            return new DbContextOperations(reporter, assembly, startupAssembly ?? assembly, outputPath, null, null, null, false, Array.Empty<string>());
#else
            return new DbContextOperations(reporter, assembly, startupAssembly ?? assembly, outputPath, null, null, false, Array.Empty<string>());
#endif
        }

        private static Assembly Load(string assemblyPath)
        {
            return File.Exists(assemblyPath) ? Assembly.LoadFile(assemblyPath) : null;
        }
    }
}
