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
        public void CanPluralizeCourseStatus()
        {
            // Act
            var result = new Pluralizer().Pluralize("CourseStatus");

            // Assert
            Assert.AreEqual("CourseStatuses", result);
        }
    }
}
