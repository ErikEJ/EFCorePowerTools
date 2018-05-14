using System;
using System.IO;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Provide services required to generate classes using Handlebars templates.
    /// </summary>
    public interface IHbsTemplateService
    {
        /// <summary>
        /// Register partial templates.
        /// </summary>
        void RegisterPartialTemplates();

        /// <summary>
        /// Register Handlebars helpers.
        /// </summary>
        /// <param name="helperName">Helper name.</param>
        /// <param name="helper">Helper delegate.</param>
        void RegisterHelper(string helperName, Action<TextWriter, object, object[]> helper);
    }
}