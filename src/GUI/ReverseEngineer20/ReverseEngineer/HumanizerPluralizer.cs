using Humanizer;
using Microsoft.EntityFrameworkCore.Design;

namespace ReverseEngineer20.ReverseEngineer
{
    public class HumanizerPluralizer : IPluralizer
    {
        public string Pluralize(string name)
        {
            return name?.Pluralize(inputIsKnownToBeSingular: false);
        }

        public string Singularize(string name)
        {
            return name?.Singularize(inputIsKnownToBePlural: false);
        }
    }
}