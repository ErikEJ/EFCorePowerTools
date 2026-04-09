using Humanizer.Inflections;
using Xunit;

namespace UnitTests
{
    public class CustomHumanizerPluralizerTest
    {
        [Theory]
        [InlineData("UserStatus")]
        [InlineData("UserData")]
        [InlineData("Delta")]
        public void DoesNotPluralize(string word)
        {
            Pluralize(word);
        }

        [Theory]
        [InlineData("UserStatus")]
        [InlineData("UserData")]
        [InlineData("Delta")]
        public void DoesNotSinguralize(string word)
        {
            Singularize(word);
        }

        private static void Pluralize(string word)
        {
            var vocabulary = HumanizerVocabularyFactory.CreateDefault();
            vocabulary.AddUncountable(word);
            var result = vocabulary.Pluralize(word, inputIsKnownToBeSingular: false);
            Assert.Equal(word, result);
        }

        private static void Singularize(string word)
        {
            var vocabulary = HumanizerVocabularyFactory.CreateDefault();
            vocabulary.AddUncountable(word);
            var result = vocabulary.Singularize(word, inputIsKnownToBePlural: false, skipSimpleWords: false);
            Assert.Equal(word, result);
        }
        [Theory]
        [InlineData("Locus", "Loci")]
        [InlineData("Krone", "Kroner")]
        public void PluralizesAndSingularizesIrregularly(string singular, string plural)
        {
            var vocabulary = HumanizerVocabularyFactory.CreateDefault();

            // Verify inflection fails by default. If this assertion fails, then Humanizer's default logic has been enhanced and a better test case is needed.
            Assert.NotEqual(plural, vocabulary.Pluralize(singular, inputIsKnownToBeSingular: false));
            Assert.NotEqual(singular, vocabulary.Singularize(plural, inputIsKnownToBePlural: false, skipSimpleWords: false));

            vocabulary.AddIrregular(singular, plural);

            Assert.Equal(plural, vocabulary.Pluralize(singular, inputIsKnownToBeSingular: false));
            Assert.Equal(singular, vocabulary.Singularize(plural, inputIsKnownToBePlural: false, skipSimpleWords: false));
        }

        [Theory]
        [InlineData("(.*)mas$", "$1mases", "Christmas", "Christmases")]
        public void DoesPluralizeWithRule(string rule, string replacement, string singular, string plural)
        {
            var vocabulary = HumanizerVocabularyFactory.CreateDefault();

            // Verify inflection fails by default. If this assertion fails, then Humanizer's default logic has been enhanced and a better test case is needed.
            Assert.NotEqual(plural, vocabulary.Pluralize(singular, inputIsKnownToBeSingular: false));

            vocabulary.AddPlural(rule, replacement);

            Assert.Equal(plural, vocabulary.Pluralize(singular, inputIsKnownToBeSingular: false));
        }

        [Theory]
        [InlineData("(.*)mases$", "$1mas", "Christmas", "Christmases")]
        public void DoesSingularizeWithRule(string rule, string replacement, string singular, string plural)
        {
            var vocabulary = HumanizerVocabularyFactory.CreateDefault();

            // Verify inflection fails by default. If this assertion fails, then Humanizer's default logic has been enhanced and a better test case is needed.
            Assert.NotEqual(singular, vocabulary.Singularize(plural, inputIsKnownToBePlural: false, skipSimpleWords: false));

            vocabulary.AddSingular(rule, replacement);

            Assert.Equal(singular, vocabulary.Singularize(plural, inputIsKnownToBePlural: false, skipSimpleWords: false));
        }
    }
}
