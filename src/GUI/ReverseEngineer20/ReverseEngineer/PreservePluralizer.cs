using Bricelam.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.RegularExpressions;

namespace ReverseEngineer20.ReverseEngineer
{
    public class PreservePluralizer : IPluralizer
    {
        public string Pluralize(string name)
        {
            if (name.Contains("_"))
            {
                var baseName = name.Substring(0, name.LastIndexOf('_') + 1);
                var lastWord = name.Substring(name.LastIndexOf('_') + 1);
                lastWord = new Pluralizer().Pluralize(lastWord) ?? lastWord;
                return baseName + lastWord;
            }
            return new Pluralizer().Pluralize(name) ?? name;
        }

        public string Singularize(string name)
        {
            if (name.Contains("_"))
            {
                var baseName = name.Substring(0, name.LastIndexOf('_') + 1);
                var lastWord = name.Substring(name.LastIndexOf('_') + 1);
                lastWord = new Pluralizer().Singularize(lastWord) ?? lastWord;
                return baseName + lastWord;
            }
            return new Pluralizer().Singularize(name) ?? name;
        }
    }
}
