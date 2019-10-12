using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            return context.Database.GenerateCreateScript();
        }

        private static string CreateDebugView(DbContext context)
        {
            var model = context.Model;
            return model.AsModel().DebugView.View;
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
