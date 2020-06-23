// Copyright (c) Microsoft Open Technologies, Inc. and others. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Contributors:
//     Microsoft Open Technologies, Inc. - initial API and implementation
//     Brice Lambson - simplify and port to EF Core

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Design;

namespace Bricelam.EntityFrameworkCore.Design
{
    /// <summary>
    /// Default pluralization service implementation to be used by Entity Framework. This pluralization service is based on
    /// English locale.
    /// </summary>
    public class Pluralizer : IPluralizer
    {
        readonly IReadOnlyDictionary<string, string> _irregularPluralsReverseList;
        readonly IReadOnlyDictionary<string, string> _assimilatedClassicalInflectionReverseList;
        readonly IReadOnlyDictionary<string, string> _oSuffixReverseList;
        readonly IReadOnlyDictionary<string, string> _classicalInflectionReverseList;

        readonly IEnumerable<string> _knownSingularWords;
        readonly IEnumerable<string> _knownPluralWords;
        readonly CultureInfo _culture = new CultureInfo("en-US");

        readonly IEnumerable<string> _uninflectiveSuffixes = new[] { "fish", "ois", "sheep", "deer", "pos", "itis", "ism" };

        readonly IEnumerable<string> _uninflectiveWords = new HashSet<string>
        {
            "bison", "flounder", "pliers", "bream", "gallows", "proceedings", "breeches", "graffiti", "rabies", "britches",
            "headquarters", "salmon", "carp", "herpes", "scissors", "chassis", "high-jinks", "sea-bass", "clippers",
            "homework", "series", "cod", "innings", "shears", "contretemps", "jackanapes", "species", "corps", "mackerel",
            "swine", "debris", "measles", "trout", "diabetes", "mews", "tuna", "djinn", "mumps", "whiting", "eland", "news",
            "wildebeest", "elk", "pincers", "police", "hair", "ice", "chaos", "milk", "cotton", "corn", "millet", "hay",
            "pneumonoultramicroscopicsilicovolcanoconiosis", "information", "rice", "tobacco", "aircraft", "rabies",
            "scabies", "diabetes", "traffic", "cotton", "corn", "millet", "rice", "hay", "hemp", "tobacco", "cabbage", "okra",
            "broccoli", "asparagus", "lettuce", "beef", "pork", "venison", "bison", "mutton", "cattle", "offspring",
            "molasses", "shambles", "shingles"
        };

        readonly IReadOnlyDictionary<string, string> _irregularVerbReverseList = new Dictionary<string, string>
        {
            { "are", "am" },
            { "were", "was" },
            { "have", "has" }
        };

        readonly IEnumerable<string> _pronounList = new HashSet<string>
        {
            "I", "we", "you", "he", "she", "they", "it", "me", "us", "him", "her", "them", "myself", "ourselves", "yourself",
            "himself", "herself", "itself", "oneself", "oneselves", "my", "our", "your", "his", "their", "its", "mine",
            "yours", "hers", "theirs", "this", "that", "these", "those", "all", "another", "any", "anybody", "anyone",
            "anything", "both", "each", "other", "either", "everyone", "everybody", "everything", "most", "much", "nothing",
            "nobody", "none", "one", "others", "some", "somebody", "someone", "something", "what", "whatever", "which",
            "whichever", "who", "whoever", "whom", "whomever", "whose"
        };

        readonly IReadOnlyDictionary<string, string> _irregularPluralsList = new Dictionary<string, string>
        {
            { "brother", "brothers" },
            { "child", "children" },
            { "cow", "cows" },
            { "ephemeris", "ephemerides" },
            { "genie", "genies" },
            { "money", "moneys" },
            { "mongoose", "mongooses" },
            { "mythos", "mythoi" },
            { "octopus", "octopuses" },
            { "ox", "oxen" },
            { "soliloquy", "soliloquies" },
            { "trilby", "trilbys" },
            { "crisis", "crises" },
            { "synopsis", "synopses" },
            { "rose", "roses" },
            { "gas", "gases" },
            { "bus", "buses" },
            { "axis", "axes" },
            { "memo", "memos" },
            { "casino", "casinos" },
            { "silo", "silos" },
            { "stereo", "stereos" },
            { "studio", "studios" },
            { "lens", "lenses" },
            { "alias", "aliases" },
            { "pie", "pies" },
            { "corpus", "corpora" },
            { "viscus", "viscera" },
            { "hippopotamus", "hippopotami" },
            { "trace", "traces" },
            { "person", "people" },
            { "chilli", "chillies" },
            { "analysis", "analyses" },
            { "basis", "bases" },
            { "neurosis", "neuroses" },
            { "oasis", "oases" },
            { "synthesis", "syntheses" },
            { "thesis", "theses" },
            { "pneumonoultramicroscopicsilicovolcanoconiosis", "pneumonoultramicroscopicsilicovolcanoconioses" },
            { "status", "statuses" },
            { "prospectus", "prospectuses" },
            { "change", "changes" },
            { "lie", "lies" },
            { "calorie", "calories" },
            { "freebie", "freebies" },
            { "case", "cases" },
            { "house", "houses" },
            { "valve", "valves" },
            { "cloth", "clothes" },
        };

        readonly IReadOnlyDictionary<string, string> _assimilatedClassicalInflectionList = new Dictionary<string, string>
                {
            { "alumna", "alumnae" },
            { "alga", "algae" },
            { "vertebra", "vertebrae" },
            { "codex", "codices" },
            { "murex", "murices" },
            { "silex", "silices" },
            { "aphelion", "aphelia" },
            { "hyperbaton", "hyperbata" },
            { "perihelion", "perihelia" },
            { "asyndeton", "asyndeta" },
            { "noumenon", "noumena" },
            { "phenomenon", "phenomena" },
            { "criterion", "criteria" },
            { "organon", "organa" },
            { "prolegomenon", "prolegomena" },
            { "agendum", "agenda" },
            { "datum", "data" },
            { "extremum", "extrema" },
            { "bacterium", "bacteria" },
            { "desideratum", "desiderata" },
            { "stratum", "strata" },
            { "candelabrum", "candelabra" },
            { "erratum", "errata" },
            { "ovum", "ova" },
            { "forum", "fora" },
            { "addendum", "addenda" },
            { "stadium", "stadia" },
            { "automaton", "automata" },
            { "polyhedron", "polyhedra" },
        };

        readonly IReadOnlyDictionary<string, string> _oSuffixList = new Dictionary<string, string>
        {
            { "albino", "albinos" },
            { "generalissimo", "generalissimos" },
            { "manifesto", "manifestos" },
            { "archipelago", "archipelagos" },
            { "ghetto", "ghettos" },
            { "medico", "medicos" },
            { "armadillo", "armadillos" },
            { "guano", "guanos" },
            { "octavo", "octavos" },
            { "commando", "commandos" },
            { "inferno", "infernos" },
            { "photo", "photos" },
            { "ditto", "dittos" },
            { "jumbo", "jumbos" },
            { "pro", "pros" },
            { "dynamo", "dynamos" },
            { "lingo", "lingos" },
            { "quarto", "quartos" },
            { "embryo", "embryos" },
            { "lumbago", "lumbagos" },
            { "rhino", "rhinos" },
            { "fiasco", "fiascos" },
            { "magneto", "magnetos" },
            { "stylo", "stylos" }
        };

        readonly IReadOnlyDictionary<string, string> _classicalInflectionList = new Dictionary<string, string>
        {
            { "stamen", "stamina" },
            { "foramen", "foramina" },
            { "lumen", "lumina" },
            { "anathema", "anathemata" },
            { "enema", "enemata" },
            { "oedema", "oedemata" },
            { "bema", "bemata" },
            { "enigma", "enigmata" },
            { "sarcoma", "sarcomata" },
            { "carcinoma", "carcinomata" },
            { "gumma", "gummata" },
            { "schema", "schemata" },
            { "charisma", "charismata" },
            { "lemma", "lemmata" },
            { "soma", "somata" },
            { "diploma", "diplomata" },
            { "lymphoma", "lymphomata" },
            { "stigma", "stigmata" },
            { "dogma", "dogmata" },
            { "magma", "magmata" },
            { "stoma", "stomata" },
            { "drama", "dramata" },
            { "melisma", "melismata" },
            { "trauma", "traumata" },
            { "edema", "edemata" },
            { "miasma", "miasmata" },
            { "abscissa", "abscissae" },
            { "formula", "formulae" },
            { "medusa", "medusae" },
            { "amoeba", "amoebae" },
            { "hydra", "hydrae" },
            { "nebula", "nebulae" },
            { "antenna", "antennae" },
            { "hyperbola", "hyperbolae" },
            { "nova", "novae" },
            { "aurora", "aurorae" },
            { "lacuna", "lacunae" },
            { "parabola", "parabolae" },
            { "apex", "apices" },
            { "latex", "latices" },
            { "vertex", "vertices" },
            { "cortex", "cortices" },
            { "pontifex", "pontifices" },
            { "vortex", "vortices" },
            { "index", "indices" },
            { "simplex", "simplices" },
            { "iris", "irides" },
            { "clitoris", "clitorides" },
            { "alto", "alti" },
            { "contralto", "contralti" },
            { "soprano", "soprani" },
            { "basso", "bassi" },
            { "crescendo", "crescendi" },
            { "tempo", "tempi" },
            { "canto", "canti" },
            { "solo", "soli" },
            { "aquarium", "aquaria" },
            { "interregnum", "interregna" },
            { "quantum", "quanta" },
            { "compendium", "compendia" },
            { "lustrum", "lustra" },
            { "rostrum", "rostra" },
            { "consortium", "consortia" },
            { "maximum", "maxima" },
            { "spectrum", "spectra" },
            { "cranium", "crania" },
            { "medium", "media" },
            { "speculum", "specula" },
            { "curriculum", "curricula" },
            { "memorandum", "memoranda" },
            { "stadium", "stadia" },
            { "dictum", "dicta" },
            { "millenium", "millenia" },
            { "trapezium", "trapezia" },
            { "emporium", "emporia" },
            { "minimum", "minima" },
            { "ultimatum", "ultimata" },
            { "enconium", "enconia" },
            { "momentum", "momenta" },
            { "vacuum", "vacua" },
            { "gymnasium", "gymnasia" },
            { "optimum", "optima" },
            { "velum", "vela" },
            { "honorarium", "honoraria" },
            { "phylum", "phyla" },
            { "focus", "foci" },
            { "nimbus", "nimbi" },
            { "succubus", "succubi" },
            { "fungus", "fungi" },
            { "nucleolus", "nucleoli" },
            { "torus", "tori" },
            { "genius", "genii" },
            { "radius", "radii" },
            { "umbilicus", "umbilici" },
            { "incubus", "incubi" },
            { "stylus", "styli" },
            { "uterus", "uteri" },
            { "stimulus", "stimuli" },
            { "apparatus", "apparatus" },
            { "impetus", "impetus" },
            { "prospectus", "prospectus" },
            { "cantus", "cantus" },
            { "nexus", "nexus" },
            { "sinus", "sinus" },
            { "coitus", "coitus" },
            { "plexus", "plexus" },
            { "status", "status" },
            { "hiatus", "hiatus" },
            { "afreet", "afreeti" },
            { "afrit", "afriti" },
            { "efreet", "efreeti" },
            { "cherub", "cherubim" },
            { "goy", "goyim" },
            { "seraph", "seraphim" },
            { "alumnus", "alumni" }
        };

        // this list contains all the plural words that being treated as singular form, for example, "they" -> "they"
        readonly IEnumerable<string> _knownConflictingPluralList = new HashSet<string>
        {
            "they", "them", "their", "have", "were", "yourself", "are"
        };

        // this list contains the words ending with "se" and we special case these words since we need to add a rule for "ses"
        // singularize to "s"
        readonly IReadOnlyDictionary<string, string> _wordsEndingWithSeReverseList = new Dictionary<string, string>
        {
            { "houses" , "house" },
            { "cases" , "case" },
            { "enterprises" , "enterprise" },
            { "purchases" , "purchase" },
            { "surprises" , "surprise" },
            { "releases" , "release" },
            { "diseases" , "disease" },
            { "promises" , "promise" },
            { "refuses" , "refuse" },
            { "whoses" , "whose" },
            { "phases" , "phase" },
            { "noises" , "noise" },
            { "nurses" , "nurse" },
            { "roses" , "rose" },
            { "franchises" , "franchise" },
            { "supervises" , "supervise" },
            { "farmhouses" , "farmhouse" },
            { "suitcases" , "suitcase" },
            { "recourses" , "recourse" },
            { "impulses" , "impulse" },
            { "licenses" , "license" },
            { "dioceses" , "diocese" },
            { "excises" , "excise" },
            { "demises" , "demise" },
            { "blouses" , "blouse" },
            { "bruises" , "bruise" },
            { "misuses" , "misuse" },
            { "curses" , "curse" },
            { "proses" , "prose" },
            { "purses" , "purse" },
            { "gooses" , "goose" },
            { "teases" , "tease" },
            { "poises" , "poise" },
            { "vases" , "vase" },
            { "fuses" , "fuse" },
            { "muses" , "muse" },
            { "slaughterhouses" , "slaughterhouse" },
            { "clearinghouses" , "clearinghouse" },
            { "endonucleases" , "endonuclease" },
            { "steeplechases" , "steeplechase" },
            { "metamorphoses" , "metamorphose" },
            { "intercourses" , "intercourse" },
            { "commonsenses" , "commonsense" },
            { "intersperses" , "intersperse" },
            { "merchandises" , "merchandise" },
            { "phosphatases" , "phosphatase" },
            { "summerhouses" , "summerhouse" },
            { "watercourses" , "watercourse" },
            { "catchphrases" , "catchphrase" },
            { "compromises" , "compromise" },
            { "greenhouses" , "greenhouse" },
            { "lighthouses" , "lighthouse" },
            { "paraphrases" , "paraphrase" },
            { "mayonnaises" , "mayonnaise" },
            { "racecourses" , "racecourse" },
            { "apocalypses" , "apocalypse" },
            { "courthouses" , "courthouse" },
            { "powerhouses" , "powerhouse" },
            { "storehouses" , "storehouse" },
            { "glasshouses" , "glasshouse" },
            { "hypotenuses" , "hypotenuse" },
            { "peroxidases" , "peroxidase" },
            { "pillowcases" , "pillowcase" },
            { "roundhouses" , "roundhouse" },
            { "streetwises" , "streetwise" },
            { "expertises" , "expertise" },
            { "discourses" , "discourse" },
            { "warehouses" , "warehouse" },
            { "staircases" , "staircase" },
            { "workhouses" , "workhouse" },
            { "briefcases" , "briefcase" },
            { "clubhouses" , "clubhouse" },
            { "clockwises" , "clockwise" },
            { "concourses" , "concourse" },
            { "playhouses" , "playhouse" },
            { "turquoises" , "turquoise" },
            { "boathouses" , "boathouse" },
            { "celluloses" , "cellulose" },
            { "epitomises" , "epitomise" },
            { "gatehouses" , "gatehouse" },
            { "grandioses" , "grandiose" },
            { "menopauses" , "menopause" },
            { "penthouses" , "penthouse" },
            { "racehorses" , "racehorse" },
            { "transposes" , "transpose" },
            { "almshouses" , "almshouse" },
            { "customises" , "customise" },
            { "footlooses" , "footloose" },
            { "galvanises" , "galvanise" },
            { "princesses" , "princesse" },
            { "universes" , "universe" },
            { "workhorses" , "workhorse" }
        };

        readonly IReadOnlyDictionary<string, string> _wordsEndingWithSisReverseList = new Dictionary<string, string>
        {
            { "analyses" , "analysis" },
            { "crises" , "crisis" },
            { "bases" , "basis" },
            { "atheroscleroses", "atherosclerosis" },
            { "electrophoreses", "electrophoresis" },
            { "psychoanalyses" , "psychoanalysis" },
            { "photosyntheses" , "photosynthesis" },
            { "amniocenteses" , "amniocentesis" },
            { "metamorphoses" , "metamorphosis" },
            { "toxoplasmoses" , "toxoplasmosis" },
            { "endometrioses" , "endometriosis" },
            { "tuberculoses" , "tuberculosis" },
            { "pathogeneses" , "pathogenesis" },
            { "osteoporoses" , "osteoporosis" },
            { "parentheses" , "parenthesis" },
            { "anastomoses" , "anastomosis" },
            { "peristalses" , "peristalsis" },
            { "hypotheses" , "hypothesis" },
            { "antitheses" , "antithesis" },
            { "apotheoses" , "apotheosis" },
            { "thromboses" , "thrombosis" },
            { "diagnoses" , "diagnosis" },
            { "syntheses" , "synthesis" },
            { "paralyses" , "paralysis" },
            { "prognoses" , "prognosis" },
            { "cirrhoses" , "cirrhosis" },
            { "scleroses" , "sclerosis" },
            { "psychoses" , "psychosis" },
            { "apoptoses" , "apoptosis" },
            { "symbioses" , "symbiosis" }
        };

        /// <summary>
        /// Constructs a new instance of default pluralization service used in Entity Framework.
        /// </summary>
        public Pluralizer()
        {
            _irregularPluralsReverseList = Reverse(_irregularPluralsList);
            _assimilatedClassicalInflectionReverseList = Reverse(_assimilatedClassicalInflectionList);
            _oSuffixReverseList = Reverse(_oSuffixList);
            _classicalInflectionReverseList = Reverse(_classicalInflectionList);
            _knownSingularWords = new HashSet<string>(
                _irregularPluralsList.Keys
                .Concat(_assimilatedClassicalInflectionList.Keys)
                .Concat(_oSuffixList.Keys)
                .Concat(_classicalInflectionList.Keys)
                .Concat(_irregularVerbReverseList.Values)
                .Concat(_uninflectiveWords)
                .Except(_knownConflictingPluralList)); // see the _knowConflictingPluralList comment above
            _knownPluralWords = new HashSet<string>(
                _irregularPluralsList.Values
                .Concat(_assimilatedClassicalInflectionList.Values)
                .Concat(_oSuffixList.Values)
                .Concat(_classicalInflectionList.Values)
                .Concat(_irregularVerbReverseList.Keys)
                .Concat(_uninflectiveWords));
        }

        // CONSIDER optimize the algorithm by collecting all the special cases to one single dictionary
        /// <summary>Returns the plural form of the specified word.</summary>
        /// <returns>The plural form of the input parameter.</returns>
        /// <param name="word">The word to be made plural.</param>
        public string Pluralize(string word)
            => Capitalize(word, InternalPluralize);

        /// <summary>Returns the singular form of the specified word.</summary>
        /// <returns>The singular form of the input parameter.</returns>
        /// <param name="word">The word to be made singular.</param>
        public string Singularize(string word)
            => Capitalize(word, InternalSingularize);

        string InternalPluralize(string word)
        {
            if (IsNoOpWord(word))
                return word;

            var suffixWord = GetSuffixWord(word, out var prefixWord);

            // by me -> by me
            if (IsNoOpWord(suffixWord))
                return prefixWord + suffixWord;

            // handle the word that do not inflect in the plural form
            if (IsUninflective(suffixWord))
                return prefixWord + suffixWord;

            // if word is one of the known plural forms, then just return
            if (_knownPluralWords.Contains(suffixWord.ToLowerInvariant()) || IsPlural(suffixWord))
                return prefixWord + suffixWord;

            // handle irregular plurals, e.g. "ox" -> "oxen"
            if (_irregularPluralsList.TryGetValue(suffixWord, out var plural))
                return prefixWord + plural;

            // handle irregular inflections for common suffixes, e.g. "mouse" -> "mice"
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "man" },
                (s) => s.Remove(s.Length - 2, 2) + "en",
                out var newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "louse", "mouse" },
                (s) => s.Remove(s.Length - 4, 4) + "ice",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "tooth" },
                (s) => s.Remove(s.Length - 4, 4) + "eeth",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "goose" },
                (s) => s.Remove(s.Length - 4, 4) + "eese",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "foot" },
                (s) => s.Remove(s.Length - 3, 3) + "eet",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "zoon" },
                (s) => s.Remove(s.Length - 3, 3) + "oa",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "cis", "sis", "xis" },
                (s) => s.Remove(s.Length - 2, 2) + "es",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // handle assimilated classical inflections, e.g. vertebra -> vertebrae
            if (_assimilatedClassicalInflectionList.TryGetValue(suffixWord, out plural))
                return prefixWord + plural;

            // Handle the classical variants of modern inflections
            // CONSIDER here is the only place we took the classical variants instead of the anglicized
            if (_classicalInflectionList.TryGetValue(suffixWord, out plural))
                return prefixWord + plural;

            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "trix" },
                (s) => s.Remove(s.Length - 1, 1) + "ces",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "eau", "ieu" },
                (s) => s + "x",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "inx", "anx", "ynx" },
                (s) => s.Remove(s.Length - 1, 1) + "ges",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // [cs]h and ss that take es as plural form
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "ch", "sh", "ss" },
                (s) => s + "es",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // f, fe that take ves as plural form
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "alf", "elf", "olf", "eaf", "arf" },
                (s) => s.EndsWith("deaf", true, _culture) ? s : s.Remove(s.Length - 1, 1) + "ves",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "nife", "life", "wife" },
                (s) => s.Remove(s.Length - 2, 2) + "ves",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // y takes ys as plural form if preceded by a vowel, but ies if preceded by a consonant, e.g. stays, skies
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "ay", "ey", "iy", "oy", "uy" },
                (s) => s + "s",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // CONSIDER proper noun handling, Marys, Tonys, ignore for now

            if (suffixWord.EndsWith("y", true, _culture))
                return prefixWord + suffixWord.Remove(suffixWord.Length - 1, 1) + "ies";

            // handle some of the words o -> os, and [vowel]o -> os, and the rest are o->oes
            if (_oSuffixList.TryGetValue(suffixWord, out plural))
                return prefixWord + plural;

            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "ao", "eo", "io", "oo", "uo" },
                (s) => s + "s",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (suffixWord.EndsWith("o", true, _culture))
                return prefixWord + suffixWord + "es";

            if (suffixWord.EndsWith("x", true, _culture))
                return prefixWord + suffixWord + "es";

            // cats, bags, hats, speakers
            return prefixWord + suffixWord + "s";
        }

        string InternalSingularize(string word)
        {
            if (IsNoOpWord(word))
                return word;

            var suffixWord = GetSuffixWord(word, out var prefixWord);

            if (IsNoOpWord(suffixWord))
                return prefixWord + suffixWord;

            // handle the word that is the same as the plural form
            if (IsUninflective(suffixWord))
                return prefixWord + suffixWord;

            // if word is one of the known singular words, then just return
            if (_knownSingularWords.Contains(suffixWord.ToLowerInvariant()))
                return prefixWord + suffixWord;

            // handle simple irregular verbs, e.g. was -> were
            if (_irregularVerbReverseList.TryGetValue(suffixWord, out var singular))
                return prefixWord + singular;

            // handle irregular plurals, e.g. "ox" -> "oxen"
            if (_irregularPluralsReverseList.TryGetValue(suffixWord, out singular))
                return prefixWord + singular;

            // handle singularization for words ending with sis and pluralized to ses,
            // e.g. "ses" -> "sis"
            if (_wordsEndingWithSisReverseList.TryGetValue(suffixWord, out singular))
                return prefixWord + singular;

            // handle words ending with se, e.g. "ses" -> "se"
            if (_wordsEndingWithSeReverseList.TryGetValue(suffixWord, out singular))
                return prefixWord + singular;

            // handle irregular inflections for common suffixes, e.g. "mouse" -> "mice"
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "men" },
                (s) => s.Remove(s.Length - 2, 2) + "an",
                out var newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "lice", "mice" },
                (s) => s.Remove(s.Length - 3, 3) + "ouse",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "teeth" },
                (s) => s.Remove(s.Length - 4, 4) + "ooth",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "geese" },
                (s) => s.Remove(s.Length - 4, 4) + "oose",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "feet" },
                (s) => s.Remove(s.Length - 3, 3) + "oot",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "zoa" },
                (s) => s.Remove(s.Length - 2, 2) + "oon",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // [cs]h and ss that take es as plural form, this is being moved up since the sses will be override by the ses
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "ches", "shes", "sses" },
                (s) => s.Remove(s.Length - 2, 2),
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // handle assimilated classical inflections, e.g. vertebra -> vertebrae
            if (_assimilatedClassicalInflectionReverseList.TryGetValue(suffixWord, out singular))
                return prefixWord + singular;

            // Handle the classical variants of modern inflections
            // CONSIDER here is the only place we took the classical variants instead of the anglicized
            if (_classicalInflectionReverseList.TryGetValue(suffixWord, out singular))
                return prefixWord + singular;

            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "trices" },
                (s) => s.Remove(s.Length - 3, 3) + "x",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "eaux", "ieux" },
                (s) => s.Remove(s.Length - 1, 1),
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "inges", "anges", "ynges" },
                (s) => s.Remove(s.Length - 3, 3) + "x",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // f, fe that take ves as plural form
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "alves", "elves", "olves", "eaves", "arves" },
                (s) => s.Remove(s.Length - 3, 3) + "f",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "nives", "lives", "wives" },
                (s) => s.Remove(s.Length - 3, 3) + "fe",
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // y takes ys as plural form if preceded by a vowel, but ies if preceded by a consonant, e.g. stays, skies
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "ays", "eys", "iys", "oys", "uys" },
                (s) => s.Remove(s.Length - 1, 1),
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // CONSIDER proper noun handling, Marys, Tonys, ignore for now

            if (suffixWord.EndsWith("ies", true, _culture))
                return prefixWord + suffixWord.Remove(suffixWord.Length - 3, 3) + "y";

            // handle some of the words o -> os, and [vowel]o -> os, and the rest are o->oes
            if (_oSuffixReverseList.TryGetValue(suffixWord, out singular))
                return prefixWord + singular;

            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "aos", "eos", "ios", "oos", "uos" },
                (s) => suffixWord.Remove(suffixWord.Length - 1, 1),
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // CONSIDER limitation on the lines below, e.g. crisis -> crises -> cris
            // all the word ending with sis, xis, cis, their plural form cannot be singularized correctly,
            // since words ending with c and cis both will get pluralized to ces
            // after searching the dictionary, the number of cis is just too small(7) that
            // we treat them as special case
            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "ces" },
                (s) => s.Remove(s.Length - 1, 1),
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (TryInflectOnSuffixInWord(
                suffixWord,
                new[] { "ces", "ses", "xes" },
                (s) => s.Remove(s.Length - 2, 2),
                out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (suffixWord.EndsWith("oes", true, _culture))
                return prefixWord + suffixWord.Remove(suffixWord.Length - 2, 2);

            if (suffixWord.EndsWith("ss", true, _culture))
                return prefixWord + suffixWord;

            if (suffixWord.EndsWith("s", true, _culture))
                return prefixWord + suffixWord.Remove(suffixWord.Length - 1, 1);

            // word is a singular
            return prefixWord + suffixWord;
        }

        bool IsPlural(string word)
            => IsUninflective(word) || _knownPluralWords.Contains(word.ToLower(_culture))
                ? true
                : !Singularize(word).Equals(word);

        // capitalize the return word if the parameter is capitalized if word is "Table", then return "Tables"
        static string Capitalize(string word, Func<string, string> action)
        {
            var result = action(word);

            return IsCapitalized(word) && result.Length != 0
                ? char.ToUpperInvariant(result[0]) + result.Substring(1)
                : result;
        }

        // separate one combine word in to two parts, prefix word and the last word(suffix word)
        static string GetSuffixWord(string word, out string prefixWord)
        {
            // use the last space to separate the words
            var lastSpaceIndex = word.LastIndexOf(' ');
            prefixWord = word.Substring(0, lastSpaceIndex + 1);

            // CONSIDER(leil): use capital letters to separate the words
            return word.Substring(lastSpaceIndex + 1);
        }

        static bool IsCapitalized(string word)
            => !string.IsNullOrEmpty(word) && char.IsUpper(word, 0);

        static bool IsAlphabets(string word)
            => !(string.IsNullOrEmpty(word.Trim()) || !word.Equals(word.Trim()) || Regex.IsMatch(word, "[^a-zA-Z\\s]"));

        bool IsUninflective(string word)
            => DoesWordContainSuffix(word, _uninflectiveSuffixes)
                || (!word.ToLower(_culture).Equals(word) && word.EndsWith("ese", false, _culture))
                || _uninflectiveWords.Contains(word.ToLowerInvariant());

        // return true when the word is "[\s]*" or leading or tailing with spaces or contains non alphabetical characters
        bool IsNoOpWord(string word)
            => !IsAlphabets(word) || word.Length <= 1 || _pronounList.Contains(word.ToLowerInvariant());

        bool DoesWordContainSuffix(string word, IEnumerable<string> suffixes)
             => suffixes.Any(s => word.EndsWith(s, true, _culture));

        bool TryInflectOnSuffixInWord(
           string word,
           IEnumerable<string> suffixes,
           Func<string, string> operationOnWord,
           out string newWord)
        {
            if (!DoesWordContainSuffix(word, suffixes))
            {
                newWord = null;

                return false;
            }

            newWord = operationOnWord(word);

            return true;
        }

        static Dictionary<string, string> Reverse(IEnumerable<KeyValuePair<string, string>> dictionary)
        {
            var result = new Dictionary<string, string>();
            foreach (var entry in dictionary)
                if (!result.ContainsKey(entry.Value))
                    result.Add(entry.Value, entry.Key);

            return result;
        }
    }
}