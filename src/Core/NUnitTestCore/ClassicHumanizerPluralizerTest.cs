using NUnit.Framework;
using RevEng.Core;

namespace UnitTests
{
    public class ClassicHumanizerPluralizerTest
    {
        [Test]
        public void SingularizeUserData()
        {
            var sut = new HumanizerPluralizer();
            var result = sut.Singularize("UserData");

            Assert.AreEqual("UserDatum", result);
        }

        [Test]
        public void PluralizeUserStatus()
        {
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize("UserStatus");

            Assert.AreEqual("UserStatuses", result);
        }
    }
}
