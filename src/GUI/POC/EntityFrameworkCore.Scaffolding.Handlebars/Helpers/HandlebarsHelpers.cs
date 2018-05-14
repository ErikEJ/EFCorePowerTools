using System;
using System.IO;

namespace EntityFrameworkCore.Scaffolding.Handlebars.Helpers
{
    /// <summary>
    /// Handlebars helpers used by the scaffolding generator.
    /// </summary>
    public static class HandlebarsHelpers
    {
        /// <summary>
        /// Get the spaces Handlebars helper.
        /// </summary>
        /// <returns>Spaces Handlebars helper.</returns>
        public static Action<TextWriter, object, object[]> GetSpacesHelper()
        {
            return (writer, context, parameters) =>
            {
                var spaces = string.Empty;
                if (parameters.Length > 0
                    && parameters[0] is string param
                    && int.TryParse(param, out int count))
                {
                    for (int i = 0; i < count; i++)
                        spaces += " ";
                }
                writer.Write(spaces);
            };
        }
    }
}