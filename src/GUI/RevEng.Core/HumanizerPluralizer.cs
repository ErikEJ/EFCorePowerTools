using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Humanizer.Inflections;
using Microsoft.EntityFrameworkCore.Design;

namespace RevEng.Core
{
    public class HumanizerPluralizer : IPluralizer
    {
        public static void SetWordsNotToAlter(List<string> words)
        {
            words = words ?? throw new ArgumentNullException(nameof(words));
            words.ForEach(w => Vocabularies.Default.AddUncountable(w));
        }

        public string Pluralize(string identifier) => identifier?.Pluralize(inputIsKnownToBeSingular: false);

        public string Singularize(string identifier) => identifier?.Singularize(inputIsKnownToBePlural: false);
    }
}
