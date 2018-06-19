using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class ModelExtension
    {
        public static string AsDgml(this DbContext context)
        {
            Type type = context.GetType();
            var dgmlBuilder = new DgmlBuilder.DgmlBuilder();

            var debugView = CreateDebugView(context);
            var dgml = dgmlBuilder.Build(debugView, type.Name, GetTemplate());

            return dgml;
        }

        public static string AsSqlScript(this DbContext context)
        {
            return GenerateCreateScript(context);
        }

        private static string CreateDebugView(DbContext context)
        {
            var model = context.Model;
            return model.AsModel().DebugView.View;
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

        private static string GetTemplate()
        {
            var resourceName = "DgmlBuilder.template.dgml";

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
