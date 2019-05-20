using Microsoft.EntityFrameworkCore.Design;

namespace ReverseEngineer20.ReverseEngineer
{
    public class InflectorPluralizer : IPluralizer
    {
        public string Pluralize(string name)
        {
            return new Pluralize.NET.Pluralizer().Pluralize(name) ?? name;
        }

        public string Singularize(string name)
        {
            return new Pluralize.NET.Pluralizer().Singularize(name) ?? name;
        }
    }
}
