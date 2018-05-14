using System;
using System.Collections.Generic;
using EntityFrameworkCore.Scaffolding.Handlebars.Helpers;
using HandlebarsLib = HandlebarsDotNet.Handlebars;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Provide services required to generate entity type classes using Handlebars templates.
    /// </summary>
    public class HbsEntityTypeTemplateService : HbsTemplateService, IEntityTypeTemplateService
    {
        /// <summary>
        /// Entity type template.
        /// </summary>
        public Func<object, string> EntityTypeTemplate { get; private set; }

        /// <summary>
        /// Constructor for entity type template service.
        /// </summary>
        /// <param name="fileService">Template file service.</param>
        public HbsEntityTypeTemplateService(ITemplateFileService fileService) : base(fileService)
        {
        }

        /// <summary>
        /// Generate entity type class.
        /// </summary>
        /// <param name="data">Data used to generate entity type class.</param>
        /// <returns>Generated entity type class.</returns>
        public virtual string GenerateEntityType(object data)
        {
            if (EntityTypeTemplate == null)
            {
                EntityTypeTemplate = CompileEntityTypeTemplate();
            }
            string entityType = EntityTypeTemplate(data);
            return entityType;
        }

        /// <summary>
        /// Compile entity type template.
        /// </summary>
        /// <returns>Entity type template.</returns>
        protected virtual Func<object, string> CompileEntityTypeTemplate()
        {
            var template = FileService.RetrieveTemplateFileContents(
                Constants.EntityTypeDirectory,
                Constants.EntityTypeTemplate + Constants.TemplateExtension);
            var entityTemplate = HandlebarsLib.Compile(template);
            return entityTemplate;
        }

        /// <summary>
        /// Get partial templates.
        /// </summary>
        /// <returns>Partial templates.</returns>
        protected override IDictionary<string, string> GetPartialTemplates()
        {
            var ctorTemplate = FileService.RetrieveTemplateFileContents(
                Constants.EntityTypePartialsDirectory,
                Constants.EntityTypeCtorTemplate + Constants.TemplateExtension);
            var importTemplate = FileService.RetrieveTemplateFileContents(
                Constants.EntityTypePartialsDirectory,
                Constants.EntityTypeImportTemplate + Constants.TemplateExtension);
            var propertyTemplate = FileService.RetrieveTemplateFileContents(
                Constants.EntityTypePartialsDirectory,
                Constants.EntityTypePropertyTemplate + Constants.TemplateExtension);

            var templates = new Dictionary<string, string>
            {
                {
                    Constants.EntityTypeCtorTemplate.ToLower(),
                    ctorTemplate
                },
                {
                    Constants.EntityTypeImportTemplate.ToLower(),
                    importTemplate
                },
                {
                    Constants.EntityTypePropertyTemplate.ToLower(),
                    propertyTemplate
                },
            };
            return templates;
        }
    }
}