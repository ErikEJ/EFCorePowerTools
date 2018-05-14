using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Entity type generator that does not generate any code.
    /// </summary>
    public class NullCSharpEntityTypeGenerator : ICSharpEntityTypeGenerator
    {
        /// <summary>
        /// Generate entity type class.
        /// </summary>
        /// <param name="entityType">Represents an entity type in an <see cref="T:Microsoft.EntityFrameworkCore.Metadata.IModel" />.</param>
        /// <param name="namespace">Entity type namespace.</param>
        /// <param name="useDataAnnotations">If true use data annotations.</param>
        /// <returns>Generated entity type.</returns>
        public string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
        {
            return string.Empty;
        }
    }
}