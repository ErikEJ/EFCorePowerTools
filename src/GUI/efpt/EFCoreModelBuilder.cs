using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
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

            //TODO Use OperationHandler output!!
            var reporter = new OperationReporter(new OperationReportHandler());

#if CORE21 || CORE22 
            return new DbContextOperations(reporter, assembly, assembly, Array.Empty<string>());
#else
            return new DbContextOperations(reporter, assembly, assembly);
#endif
        }

        private Assembly Load(string assemblyPath)
        {
            return File.Exists(assemblyPath) ? Assembly.LoadFile(assemblyPath) : null;
        }
    }
}
