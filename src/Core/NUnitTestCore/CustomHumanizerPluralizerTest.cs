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

            Assert.True(word == result);
        }

        private static void Singularize(string word)
        {
            Vocabularies.Default.AddUncountable(word);
            var sut = new HumanizerPluralizer();
            var result = sut.Singularize(word);

            Assert.True(word == result);
        }
    }
}
