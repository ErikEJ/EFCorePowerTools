using Humanizer;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
using System.Linq;

namespace ReverseEngineer20.ReverseEngineer
{
    public class InflectorPluralizer : IPluralizer
    {
        private readonly Dictionary<string, string> _customCharacterPluralize;
        private readonly Dictionary<string, string> _customCharacterSingularize;

        public InflectorPluralizer(List<Schema> customReplacers = null)
        {
            if(customReplacers != null)
            {
                _customCharacterPluralize = customReplacers
                    .SelectMany(x => x.Tables)
                    .Where(x => !string.IsNullOrWhiteSpace(x.VariableName))
                    .ToDictionary(key => key.Name.ToPascalCase(), value => value.VariableName);

                _customCharacterSingularize = customReplacers
                    .SelectMany(x => x.Tables)
                    .Where(x => !string.IsNullOrWhiteSpace(x.NewName))
                    .ToDictionary(key => key.Name.ToPascalCase(), value => value.NewName);
            }
        }

        public string Pluralize(string name)
        {
            if (name != null)
            {
                if (_customCharacterPluralize?.ContainsKey(name) ?? false)
                    return _customCharacterPluralize[name];

                return name.Pluralize(inputIsKnownToBeSingular: false);
            }
            return name;
        }

        public string Singularize(string name)
        {
            if (name != null)
            {
                if (_customCharacterSingularize?.ContainsKey(name) ?? false)
                    return _customCharacterSingularize[name];

                return name.Singularize(inputIsKnownToBePlural: false);
            }
            return name;
        }
    }
}