using System;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Provide services required to generate entity type classes using Handlebars templates.
    /// </summary>
    public interface IEntityTypeTemplateService : IHbsTemplateService
    {
        /// <summary>
        /// Entity type template.
        /// </summary>
        Func<object, string> EntityTypeTemplate { get; }

        /// <summary>
        /// Generate entity type class.
        /// </summary>
        /// <param name="data">Data used to generate entity type class.</param>
        /// <returns>Generated entity type class.</returns>
        string GenerateEntityType(object data);
    }
}