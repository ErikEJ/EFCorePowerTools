using System;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Provide services required to generate the DbContext class using Handlebars templates.
    /// </summary>
    public interface IDbContextTemplateService : IHbsTemplateService
    {
        /// <summary>
        /// DbContext template.
        /// </summary>
        Func<object, string> DbContextTemplate { get; }

        /// <summary>
        /// Generate DbContext class.
        /// </summary>
        /// <param name="data">Data used to generate DbContext class.</param>
        /// <returns>Generated DbContext class.</returns>
        string GenerateDbContext(object data);
    }
}