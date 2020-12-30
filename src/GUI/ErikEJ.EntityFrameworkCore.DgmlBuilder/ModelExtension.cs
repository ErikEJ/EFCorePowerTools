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
#pragma warning disable EF1001 // Internal EF Core API usage.
#if CORE50
            return model.AsModel().DebugView.LongView;
#else
            return model.AsModel().DebugView.View;
#endif
#pragma warning restore EF1001 // Internal EF Core API usage.
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
