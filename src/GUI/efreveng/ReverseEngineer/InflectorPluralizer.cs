using Humanizer;
using Microsoft.EntityFrameworkCore.Design;

namespace ReverseEngineer20.ReverseEngineer
{
    public class InflectorPluralizer : IPluralizer
    {
        public string Pluralize(string name)
        {
            if (name != null)
            {
                return name.Pluralize(inputIsKnownToBeSingular: false);
            }
            return name;
        }

        public string Singularize(string name)
        {
            if (name != null)
            {
                return name.Singularize(inputIsKnownToBePlural: false);
            }
            return name;
        }
    }
}