using Bricelam.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design;

namespace RevEng.Core;

public class LegacyPluralizer : IPluralizer
{
    public string Pluralize(string identifier) => new Pluralizer().Pluralize(identifier) ?? identifier;

    public string Singularize(string identifier) => new Pluralizer().Singularize(identifier) ?? identifier;
}