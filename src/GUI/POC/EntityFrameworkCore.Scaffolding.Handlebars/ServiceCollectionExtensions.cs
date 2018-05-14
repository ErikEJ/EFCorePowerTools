using System;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Helper methods for configuring code generation for Entity Framework Core using Handlebars templates.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static string ProjectRoot { get; private set; }
        /// <summary>
        ///     <para>
        ///         Registers the Handlebars scaffolding generator as a service in the <see cref="IServiceCollection" />.
        ///         This allows you to customize generated DbContext and entity type classes by modifying the Handlebars 
        ///         templates in the CodeTemplates folder.
        ///     </para>
        ///     <para>
        ///         Has <paramref name="options" /> that allow you to choose whether to generate only the DbContext class, 
        ///         only entity type classes, or both DbContext and entity type classes (the default).
        ///         This can be useful when placing model classes in a separate class library.
        ///     </para>
        /// </summary>
        /// <param name="services"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <param name="options">Options for reverse engineering classes from an existing database.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddHandlebarsScaffolding(this IServiceCollection services,
            string projectRoot, ReverseEngineerOptions options = ReverseEngineerOptions.DbContextAndEntities)
        {
            ProjectRoot = projectRoot;
            Type dbContextGeneratorImpl;
            var dbContextGeneratorType = typeof(ICSharpDbContextGenerator);
            if (options == ReverseEngineerOptions.DbContextOnly
                || options == ReverseEngineerOptions.DbContextAndEntities)
                dbContextGeneratorImpl = typeof(HbsCSharpDbContextGenerator);
            else
                dbContextGeneratorImpl = typeof(NullCSharpDbContextGenerator);
            services.AddSingleton(dbContextGeneratorType, dbContextGeneratorImpl);

            Type entityGeneratorImpl;
            var entityGeneratorType = typeof(ICSharpEntityTypeGenerator);
            if (options == ReverseEngineerOptions.EntitiesOnly
                || options == ReverseEngineerOptions.DbContextAndEntities)
                entityGeneratorImpl = typeof(HbsCSharpEntityTypeGenerator);
            else
                entityGeneratorImpl = typeof(NullCSharpEntityTypeGenerator);
            services.AddSingleton(entityGeneratorType, entityGeneratorImpl);

            services.AddSingleton<ITemplateFileService, FileSystemTemplateFileService>();
            services.AddSingleton<IDbContextTemplateService, HbsDbContextTemplateService>();
            services.AddSingleton<IEntityTypeTemplateService, HbsEntityTypeTemplateService>();
            services.AddSingleton<IScaffoldingCodeGenerator, HbsCSharpScaffoldingGenerator>();
            return services;
        }
    }
}