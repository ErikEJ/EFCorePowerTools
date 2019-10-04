using Bricelam.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design;

namespace ReverseEngineer20.ReverseEngineer
{
    public class LegacyInflectorPluralizer : IPluralizer
    {
        public string Pluralize(string name)
        {
            return new Pluralizer().Pluralize(name) ?? name;
        }

        public string Singularize(string name)
        {
            return new Pluralizer().Singularize(name) ?? name;
        }
    }
}
