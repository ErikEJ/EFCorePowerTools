using Humanizer;
using Microsoft.EntityFrameworkCore.Design;

namespace RevEng.Core
{
    public class HumanizerPluralizer : IPluralizer
    {
        public string Pluralize(string identifier)
           => identifier?.Pluralize(inputIsKnownToBeSingular: false);

        public string Singularize(string identifier)
            => identifier?.Singularize(inputIsKnownToBePlural: false);
    }
}
