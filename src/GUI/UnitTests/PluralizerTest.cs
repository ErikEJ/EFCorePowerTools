using Bricelam.EntityFrameworkCore.Design;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class PluralizerTest
    {

        [Test]
        public void CanPluralizePeople()
        {
            // Act
            var result = new Pluralizer().Pluralize("People");

            // Assert
            Assert.AreEqual("People", result);
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
