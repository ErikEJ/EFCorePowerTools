using NUnit.Framework;
using NUnit.Framework.Legacy;
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

            ClassicAssert.AreEqual("UserDatum", result);
        }

        [Test]
        public void PluralizeUserStatus()
        {
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize("UserStatus");

            ClassicAssert.AreEqual("UserStatuses", result);
        }
    }
}
