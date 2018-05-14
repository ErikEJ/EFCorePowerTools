using System;
using System.Collections.Generic;
using System.IO;
using HandlebarsLib = HandlebarsDotNet.Handlebars;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Provide services required to generate classes using Handlebars templates.
    /// </summary>
    public abstract class HbsTemplateService : IHbsTemplateService
    {
        /// <summary>
        /// Template file service.
        /// </summary>
        protected readonly ITemplateFileService FileService;

        /// <summary>
        /// Constructor for the template service.
        /// </summary>
        /// <param name="fileService">Template file service.</param>
        protected HbsTemplateService(ITemplateFileService fileService)
        {
            FileService = fileService;
        }

        /// <summary>
        /// Register Handlebars helpers.
        /// </summary>
        /// <param name="helperName">Helper name.</param>
        /// <param name="helper">Helper delegate.</param>
        public virtual void RegisterHelper(string helperName, Action<TextWriter, object, object[]> helper)
        {
            HandlebarsLib.RegisterHelper(helperName, (output, context, args) => helper(output, context, args));
        }

        /// <summary>
        /// Register partial templates.
        /// </summary>
        public virtual void RegisterPartialTemplates()
        {
            var partialTemplates = GetPartialTemplates();
            foreach (var partialTemplate in partialTemplates)
            {
                HandlebarsLib.RegisterTemplate(partialTemplate.Key, partialTemplate.Value);
            }
        }

        /// <summary>
        /// Get partial templates.
        /// </summary>
        /// <returns>Partial templates.</returns>
        protected abstract IDictionary<string, string> GetPartialTemplates();
    }
}