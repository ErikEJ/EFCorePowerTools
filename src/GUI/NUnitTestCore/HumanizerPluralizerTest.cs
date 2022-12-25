using System.Collections.Generic;
using NUnit.Framework;
using RevEng.Core;

namespace UnitTests
{
    [TestFixture]
    public class HumanizerPluralizerTest
    {
        [Test]
        public void NotPluralizeAWordWhenExcluded()
        {
            HumanizerPluralizer.SetWordsNotToAlter(new List<string> { "Dog" });
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize("Dog");

            Assert.True("Dog" == result);
        }

        [Test]
        public void PluralizeAWordWhenNotExcluded()
        {
            HumanizerPluralizer.SetWordsNotToAlter(new List<string> { "Cat" });
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize("Dog");

            Assert.True("Dogs" == result);
        }

        [Test]
        public void NotPluralizeUserData()
        {
            HumanizerPluralizer.SetWordsNotToAlter(new List<string> { "UserData" });
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize("UserData");

            Assert.True("UserData" == result);
        }

        [Test]
        public void NotSingularizeUserData()
        {
            HumanizerPluralizer.SetWordsNotToAlter(new List<string> { "UserData" });
            var sut = new HumanizerPluralizer();
            var result = sut.Singularize("UserData");

            Assert.True("UserData" == result);
        }
    }
}
