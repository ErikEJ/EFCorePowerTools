using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.IO;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class ModelExtension
    {
        public static string AsDgml(this DbContext context)
        {
#pragma warning disable CA1510 // Use ArgumentNullException throw helper
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Type type = context.GetType();

            var debugView = CreateDebugView(context);
            var dgml = Dgml.DgmlBuilder.Build(debugView, type.Name, GetTemplate());

            return dgml;
        }

        public static string AsSqlScript(this DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

#pragma warning restore CA1510 // Use ArgumentNullException throw helper
            return context.Database.GenerateCreateScript();
        }

        private static string CreateDebugView(DbContext context)
        {
            return context.GetService<IDesignTimeModel>().Model.ToDebugString(MetadataDebugStringOptions.LongDefault);
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
