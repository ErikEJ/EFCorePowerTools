using Humanizer;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
using System.Linq;

namespace ReverseEngineer20.ReverseEngineer
{
    public class InflectorPluralizer : IPluralizer
    {
        private readonly Dictionary<string, string> _customerCharacterPluralize;
        private readonly Dictionary<string, string> _customerCharacterSingularize;

        public InflectorPluralizer(List<Schema> customReplacers)
        {
            _customerCharacterPluralize = customReplacers
                .SelectMany(x => x.Tables)
                .Where(x => !string.IsNullOrWhiteSpace(x.VariableName))
                .ToDictionary(key => key.Name.ToPascalCase(), value => value.VariableName);

            _customerCharacterSingularize = customReplacers
                .SelectMany(x => x.Tables)
                .Where(x => !string.IsNullOrWhiteSpace(x.NewName))
                .ToDictionary(key => key.Name.ToPascalCase(), value => value.NewName);

        }

        public string Pluralize(string name)
        {
            if (name != null)
            {
                if (_customerCharacterPluralize.ContainsKey(name))
                    return _customerCharacterPluralize[name];

                return name.Pluralize(inputIsKnownToBeSingular: false);
            }
            return name;
        }

        public string Singularize(string name)
        {
            if (name != null)
            {
                if (_customerCharacterSingularize.ContainsKey(name))
                    return _customerCharacterSingularize[name];

                return name.Singularize(inputIsKnownToBePlural: false);
            }
            return name;
        }
    }
}