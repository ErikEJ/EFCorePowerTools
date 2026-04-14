using Humanizer.Inflections;
using RevEng.Core;
using Xunit;

namespace UnitTests
{
    public class CustomHumanizerPluralizerTest
    {
        [Theory]
        [InlineData("AuditStatus")]
        [InlineData("TelemetryData")]
        [InlineData("Delta")]
        public void DoesNotPluralizeCustomUncountableWords(string word)
        {
            Pluralize(word);
        }

        [Theory]
        [InlineData("AuditStatus")]
        [InlineData("TelemetryData")]
        [InlineData("Delta")]
        public void DoesNotSingularizeCustomUncountableWords(string word)
        {
            Singularize(word);
        }

        private static void Pluralize(string word)
        {
            Vocabularies.Default.AddUncountable(word);
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize(word);

            Assert.Equal(word, result);
        }

        private static void Singularize(string word)
        {
            Vocabularies.Default.AddUncountable(word);
            var sut = new HumanizerPluralizer();
            var result = sut.Singularize(word);

            Assert.Equal(word, result);
        }

        [Theory]
        [InlineData("Locus", "Loci")]
        [InlineData("Krone", "Kroner")]
        public void PluralizesAndSingularizesIrregularly(string singular, string plural)
        {
            // Verify inflection fails by default. If this assertion fails, then Humanizer's default logic has been enhanced and a better test case is needed.
            var sut = new HumanizerPluralizer();
            Assert.NotEqual(plural, sut.Pluralize(singular));
            Assert.NotEqual(singular, sut.Singularize(plural));

            Vocabularies.Default.AddIrregular(singular, plural);

            Assert.Equal(plural, sut.Pluralize(singular));
            Assert.Equal(singular, sut.Singularize(plural));
        }

        [Theory]
        [InlineData("(.*)mas$", "$1mases", "Christmas", "Christmases")]
        public void DoesPluralizeWithRule(string rule, string replacement, string singular, string plural)
        {
            // Verify inflection fails by default. If this assertion fails, then Humanizer's default logic has been enhanced and a better test case is needed.
            var sut = new HumanizerPluralizer();
            Assert.NotEqual(plural, sut.Pluralize(singular));

            Vocabularies.Default.AddPlural(rule, replacement);

            Assert.Equal(plural, sut.Pluralize(singular));
        }

        [Theory]
        [InlineData("(.*)mases$", "$1mas", "Christmas", "Christmases")]
        public void DoesSingularizeWithRule(string rule, string replacement, string singular, string plural)
        {
            // Verify inflection fails by default. If this assertion fails, then Humanizer's default logic has been enhanced and a better test case is needed.
            var sut = new HumanizerPluralizer();
            Assert.NotEqual(singular, sut.Singularize(plural));

            Vocabularies.Default.AddSingular(rule, replacement);

            Assert.Equal(singular, sut.Singularize(plural));
        }

    }
}
