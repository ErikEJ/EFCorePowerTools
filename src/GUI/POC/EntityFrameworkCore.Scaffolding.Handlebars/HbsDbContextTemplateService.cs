using System;
using System.Collections.Generic;
using EntityFrameworkCore.Scaffolding.Handlebars.Helpers;
using HandlebarsLib = HandlebarsDotNet.Handlebars;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Provide services required to generate the DbContext class using Handlebars templates.
    /// </summary>
    public class HbsDbContextTemplateService : HbsTemplateService, IDbContextTemplateService
    {
        /// <summary>
        /// DbContext template.
        /// </summary>
        public Func<object, string> DbContextTemplate { get; private set; }

        /// <summary>
        /// Constructor for the DbContext template service.
        /// </summary>
        /// <param name="fileService">Template file service.</param>
        public HbsDbContextTemplateService(ITemplateFileService fileService) : base(fileService)
        {
        }

        /// <summary>
        /// Generate DbContext class.
        /// </summary>
        /// <param name="data">Data used to generate DbContext class.</param>
        /// <returns>Generated DbContext class.</returns>
        public virtual string GenerateDbContext(object data)
        {
            if (DbContextTemplate == null)
            {
                DbContextTemplate = CompileDbContextTemplate();
            }
            string dbContext = DbContextTemplate(data);
            return dbContext;
        }

        /// <summary>
        /// Compile the DbContext template.
        /// </summary>
        /// <returns>DbContext template.</returns>
        protected virtual Func<object, string> CompileDbContextTemplate()
        {
            var template = FileService.RetrieveTemplateFileContents(
                Constants.DbContextDirectory,
                Constants.DbContextTemplate + Constants.TemplateExtension);
            var contextTemplate = HandlebarsLib.Compile(template);
            return contextTemplate;
        }

        /// <summary>
        /// Get DbContext partial templates.
        /// </summary>
        /// <returns>Partial templates.</returns>
        protected override IDictionary<string, string> GetPartialTemplates()
        {
            var importTemplate = FileService.RetrieveTemplateFileContents(
                Constants.DbContextPartialsDirectory,
                Constants.DbContextImportTemplate + Constants.TemplateExtension);
            var propertyTemplate = FileService.RetrieveTemplateFileContents(
                Constants.DbContextPartialsDirectory,
                Constants.DbContextDbSetsTemplate + Constants.TemplateExtension);

            var templates = new Dictionary<string, string>
            {
                {
                    Constants.DbContextImportTemplate.ToLower(),
                    importTemplate
                },
                {
                    Constants.DbContextDbSetsTemplate.ToLower(),
                    propertyTemplate
                },
            };
            return templates;
        }
    }
}