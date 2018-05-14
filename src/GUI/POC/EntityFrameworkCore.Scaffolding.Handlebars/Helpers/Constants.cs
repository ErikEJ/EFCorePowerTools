namespace EntityFrameworkCore.Scaffolding.Handlebars.Helpers
{
    /// <summary>
    /// Constants used by the Handlebars scaffolding generator.
    /// </summary>
    public static class Constants
    {
        /// <summary>Spaces Handlebars helper.</summary>
        public const string SpacesHelper = "spaces";

        /// <summary>Handlebars templates file extension.</summary>
        public const string TemplateExtension = ".hbs";

        /// <summary>Code templates folder.</summary>
        public const string CodeTemplatesDirectory = "CodeTemplates";

        /// <summary>DbContext template folder.</summary>
        public const string DbContextDirectory = CodeTemplatesDirectory + "/CSharpDbContext";

        /// <summary>DbContext partial templates folder.</summary>
        public const string DbContextPartialsDirectory = DbContextDirectory + "/Partials";

        /// <summary>Entity type template folder.</summary>
        public const string EntityTypeDirectory = CodeTemplatesDirectory + "/CSharpEntityType";

        /// <summary>Entity type partial templates folder.</summary>
        public const string EntityTypePartialsDirectory = EntityTypeDirectory + "/Partials";

        /// <summary>Entity type template.</summary>
        public const string EntityTypeTemplate = "Class";

        /// <summary>Entity type constructor template.</summary>
        public const string EntityTypeCtorTemplate = "Constructor";

        /// <summary>Entity type imports template.</summary>
        public const string EntityTypeImportTemplate = "Imports";

        /// <summary>Entity type properties template.</summary>
        public const string EntityTypePropertyTemplate = "Properties";

        /// <summary>DbContext template.</summary>
        public const string DbContextTemplate = "DbContext";

        /// <summary>DbContext imports template.</summary>
        public const string DbContextImportTemplate = "DbImports";

        /// <summary>DbContext DbSets template.</summary>
        public const string DbContextDbSetsTemplate = "DbSets";
    }
}