using Xunit;
using RevEng.Core;

namespace UnitTests
{
    public class ClassicHumanizerPluralizerTest
    {
        [Fact]
        public void SingularizeUserData()
        {
            var sut = new HumanizerPluralizer();
            var result = sut.Singularize("UserData");

            Assert.Equal("UserDatum", result);
        }

        [Fact]
        public void PluralizeUserStatus()
        {
            var sut = new HumanizerPluralizer();
            var result = sut.Pluralize("UserStatus");

            Assert.Equal("UserStatuses", result);
        }
    }
}
