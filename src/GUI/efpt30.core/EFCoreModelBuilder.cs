using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Modelling
{
    public class EfCoreModelBuilder
    {
        public List<Tuple<string, string>> GenerateDebugView(string outputPath, string startupOutputPath)
        {
            return BuildResult(outputPath, startupOutputPath ?? outputPath, false);
        }

        public List<Tuple<string, string>> GenerateDatabaseCreateScript(string outputPath, string startupOutputPath)
        {
            return BuildResult(outputPath, startupOutputPath ?? outputPath, true);
        }

        private List<Tuple<string, string>> BuildResult(string outputPath, string startupOutputPath,  bool generateDdl)
        {
            var result = new List<Tuple<string, string>>();
            var operations = GetOperations(outputPath, startupOutputPath);
            var types = GetDbContextTypes(operations);

            foreach (var type in types)
            {
                var dbContext = operations.CreateContext(type.Name);

                var generated = generateDdl
                    ? GenerateCreateScript(dbContext)
#if CORE50
                    : dbContext.Model.ToDebugString(MetadataDebugStringOptions.LongDefault);
#elif CORE60
                    : dbContext.GetService<IDesignTimeModel>().Model.ToDebugString(MetadataDebugStringOptions.LongDefault);
#else
                    : dbContext.Model.AsModel().DebugView.View;
#endif
                result.Add(new Tuple<string, string>(type.Name, generated));
            }

            return result;
        }

        private static string GenerateCreateScript(DbContext dbContext)
        {
            var database = dbContext.Database;
            var model = database.GetService<IModel>();
            var differ = database.GetService<IMigrationsModelDiffer>();
            var generator = database.GetService<IMigrationsSqlGenerator>();
            var sql = database.GetService<ISqlGenerationHelper>();

#if CORE50
            var operations = differ.GetDifferences(null, model.GetRelationalModel());
#elif CORE60
            var operations = differ.GetDifferences(null, dbContext.GetService<IDesignTimeModel>().Model.GetRelationalModel());
#else
            var operations = differ.GetDifferences(null, model);
#endif
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

        private DbContextOperations GetOperations(string outputPath, string startupOutputPath)
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
#if CORE60
            //TODO Collect project, rootNamespace,language and nullable for optimize later
            return new DbContextOperations(reporter, assembly, startupAssembly ?? assembly, outputPath, null, null, false, Array.Empty<string>());
#else
            return new DbContextOperations(reporter, assembly, startupAssembly ?? assembly, Array.Empty<string>());
#endif
        }

        private Assembly Load(string assemblyPath)
        {
            return File.Exists(assemblyPath) ? Assembly.LoadFile(assemblyPath) : null;
        }
    }
}
