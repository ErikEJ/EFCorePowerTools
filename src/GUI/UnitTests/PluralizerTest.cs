using Bricelam.EntityFrameworkCore.Design;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class PluralizerTest
    {

        [Test]
        [TestCase("bison")]
        [TestCase("Djinn")]
        [TestCase("tobacco")]
        public void CanPlurarizeUninflectedWord(string word)
        {
            // Act
            var result = new Pluralizer().Pluralize(word);

            // Assert
            Assert.That(result, Is.EqualTo(word));
        }

        [Test]
        [TestCase("bison")]
        [TestCase("Djinn")]
        [TestCase("tobacco")]
        public void CanSingularizeUninflectedWord(string word)
        {
            // Act
            var result = new Pluralizer().Singularize(word);

            // Assert
            Assert.That(result, Is.EqualTo(word));
        }

        [Test]
        [TestCase("am", "are")]
        [TestCase("is", "are")]
        [TestCase("was", "were")]
        [TestCase("has", "have")]
        public void CanPlurarizeIrregularVerb(string word, string expectedResult)
        {
            var result = new Pluralizer().Pluralize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("are", "am")]
        [TestCase("were", "was")]
        [TestCase("have", "has")]
        public void CanSingularizeIrregularVerb(string word, string expectedResult)
        {
            var result = new Pluralizer().Singularize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase("child", "children")]
        [TestCase("corpus", "corpora")]
        [TestCase("pie", "pies")]
        public void CanPlurarizeIrregularWord(string word, string expectedResult)
        {
            var result = new Pluralizer().Pluralize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("children", "child")]
        [TestCase("corpora", "corpus")]
        [TestCase("pies", "pie")]
        public void CanSingularizeIrregularWord(string word, string expectedResult)
        {
            var result = new Pluralizer().Singularize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CanPluralizeCourseStatusWithSpace()
        {
            // Act
            var result = new Pluralizer().Pluralize("Course Status");

            // Assert
            Assert.AreEqual("Course Statuses", result);
        }

        [Test]
        public void CanSingularizeGames()
        {
            // Act
            var result = new Pluralizer().Singularize("Games");

            // Assert
            Assert.AreEqual("Game", result);
        }

        [Test]
        public void CanPluralizeGame()
        {
            // Act
            var result = new Pluralizer().Pluralize("Game");

            // Assert
            Assert.AreEqual("Games", result);
        }

        [Test]
        public void CanPluralizeWordWithSpaceAndUpperCase()
        {
            // Act
            var result = new Pluralizer().Pluralize("Fun CourseStatus");

            // Assert
            Assert.AreEqual("Fun CourseStatuses", result);
        }

        [Test]
        public void CanPluralizeStatus()
        {
            // Act
            var result = new Pluralizer().Pluralize("Status");

            // Assert
            Assert.AreEqual("Statuses", result);
        }

        [Test]
        public void CanPluralizeStatusLowerCase()
        {
            // Act
            var result = new Pluralizer().Pluralize("status");

            // Assert
            Assert.AreEqual("statuses", result);
        }

        [Test]
        [TestCase("AxResponses", "AxResponse", Description = "Ending on -se")]
        [TestCase("CourseStatuses", "CourseStatus", Description = "Ending on -s")]
        [TestCase("HugeOxen", "HugeOx", Description = "Irregular")]
        public void CanSingularizePascalCaseCompoundWords(string word, string expectedResult)
        {
            // Act
            var result = new Pluralizer().Singularize(word);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("AxResponse", "AxResponses", Description = "Ending on -se")]
        [TestCase("CourseStatus", "CourseStatuses", Description = "Ending on -s")]
        [TestCase("HugeOx", "HugeOxen", Description = "Irregular")]
        public void CanPlurarizePascalCaseCompoundWords(string word, string expectedResult)
        {
            // Act
            var result = new Pluralizer().Pluralize(word);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
