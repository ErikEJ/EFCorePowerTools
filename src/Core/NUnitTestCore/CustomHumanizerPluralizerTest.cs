using Humanizer.Inflections;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RevEng.Core;

namespace UnitTests
{
    [TestFixture]
    public class CustomHumanizerPluralizerTest
    {
        [Test]
        public void DoesNotPluralizeUserStatus()
        {
            var word = "UserStatus";
            Pluralize(word);
        }

        [Test]
        public void DoesNotPluralizeUserData()
        {
            var word = "UserData";
            Pluralize(word);
        }

        [Test]
        public void DoesNotSinguralizeUserStatus()
        {
            var word = "UserStatus";
            Singularize(word);
        }

        [Test]
        public void DoesNotSinguralizeUserData()
        {
            var word = "UserData";
            Singularize(word);
        }

        private static void Pluralize(string word)
        {
            Vocabularies.Default.AddUncountable(word);
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize(word);

            ClassicAssert.True(word == result);
        }

        private static void Singularize(string word)
        {
            Vocabularies.Default.AddUncountable(word);
            var sut = new HumanizerPluralizer();
            var result = sut.Singularize(word);

            ClassicAssert.True(word == result);
        }
    }
}
