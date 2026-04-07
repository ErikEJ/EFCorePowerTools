using Humanizer.Inflections;
using NUnit.Framework;
using RevEng.Core;

namespace UnitTests
{
    [TestFixture]
    public class CustomHumanizerPluralizerTest
    {
        [Test]
        [TestCase("UserStatus")]
        [TestCase("UserData")]
        [TestCase("Delta")]
        public void DoesNotPluralize(string word)
        {
            Pluralize(word);
        }

        [Test]
        [TestCase("UserStatus")]
        [TestCase("UserData")]
        [TestCase("Delta")]
        public void DoesNotSinguralize(string word)
        {
            Singularize(word);
        }

        private static void Pluralize(string word)
        {
            Vocabularies.Default.AddUncountable(word);
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize(word);

            Assert.That(result, Is.EqualTo(word));
        }

        private static void Singularize(string word)
        {
            Vocabularies.Default.AddUncountable(word);
            var sut = new HumanizerPluralizer();
            var result = sut.Singularize(word);

            Assert.That(result, Is.EqualTo(word));
        }

        [Test]
        [TestCase("Locus", "Loci")]
        [TestCase("Krone", "Kroner")]
        public void PluralizesAndSingularizesIrregularly(string singular, string plural)
        {
            // Verify inflection fails by default. If this assertion fails, then Humanizer's default logic has been enhanced and a better test case is needed.
            var sut = new HumanizerPluralizer();
            Assert.That(sut.Pluralize(singular), Is.Not.EqualTo(plural), "New test case needed.");
            Assert.That(sut.Singularize(plural), Is.Not.EqualTo(singular), "New test case needed.");

            Vocabularies.Default.AddIrregular(singular, plural);

            Assert.That(sut.Pluralize(singular), Is.EqualTo(plural));
            Assert.That(sut.Singularize(plural), Is.EqualTo(singular));
        }

        [Test]
        [TestCase("(.*)mas$", "$1mases", "Christmas", "Christmases")]
        public void DoesPluralizeWithRule(string rule, string replacement, string singular, string plural)
        {
            // Verify inflection fails by default. If this assertion fails, then Humanizer's default logic has been enhanced and a better test case is needed.
            var sut = new HumanizerPluralizer();
            Assert.That(sut.Pluralize(singular), Is.Not.EqualTo(plural), "New test case needed.");

            Vocabularies.Default.AddPlural(rule, replacement);

            Assert.That(sut.Pluralize(singular), Is.EqualTo(plural));
        }

        [Test]
        [TestCase("(.*)mases$", "$1mas", "Christmas", "Christmases")]
        public void DoesSingularizeWithRule(string rule, string replacement, string singular, string plural)
        {
            // Verify inflection fails by default. If this assertion fails, then Humanizer's default logic has been enhanced and a better test case is needed.
            var sut = new HumanizerPluralizer();
            Assert.That(sut.Singularize(plural), Is.Not.EqualTo(singular), "New test case needed.");

            Vocabularies.Default.AddSingular(rule, replacement);

            Assert.That(sut.Singularize(plural), Is.EqualTo(singular));
        }

    }
}
