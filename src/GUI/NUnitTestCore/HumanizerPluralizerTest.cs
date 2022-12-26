using System.Collections.Generic;
using NUnit.Framework;
using RevEng.Core;

namespace UnitTests
{
    [TestFixture]
    public class HumanizerPluralizerTest
    {
        [Test]
        [TestCase("Cat")]
        [TestCase("CatDatum")]
        public void NotPluralizeAWordWhenExcluded(string word)
        {
            HumanizerPluralizer.SetWordsNotToAlter(new List<string> { word });
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize(word);

            Assert.True(word == result);
        }

        [Test]
        [TestCase("Turtle", "Turtles")]
        [TestCase("TurtleDatum", "TurtleData")]
        public void PluralizeAWordWhenNotExcluded(string word, string expected)
        {
            HumanizerPluralizer.SetWordsNotToAlter(new List<string>());
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize(word);

            Assert.True(expected == result);
        }

        [Test]
        [TestCase("Users")]
        [TestCase("UserData")]
        public void NotSingularizeAWordWhenExcluded(string word)
        {
            HumanizerPluralizer.SetWordsNotToAlter(new List<string> { word });
            var sut = new HumanizerPluralizer();
            var result = sut.Singularize(word);

            Assert.True(word == result);
        }

        [Test]
        [TestCase("Tortoises", "Tortoise")]
        [TestCase("TortoiseData", "TortoiseDatum")]
        public void SingularizeAWordWhenNotExcluded(string word, string expected)
        {
            HumanizerPluralizer.SetWordsNotToAlter(new List<string>());
            var sut = new HumanizerPluralizer();
            var result = sut.Singularize(word);

            Assert.True(expected == result);
        }
    }
}
