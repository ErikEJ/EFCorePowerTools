using NUnit.Framework;

namespace UnitTests.BLL
{
    using EFCorePowerTools.BLL;
    using EFCorePowerTools.Common.BLL;
    using EFCorePowerTools.Common.DAL;
    using EFCorePowerTools.Common.Models;
    using Moq;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class ExtensionVersionServiceTests
    {
        [Test]
        public void SetExtensionVersion_ProvideVersion()
        {
            // Arrange
            var version = "50.10.0.4";
            var dnaMock = new Mock<IDotNetAccess>();
            dnaMock.Setup(m => m.GetExtensionVersion()).Returns(version);
            var aem = new AboutExtensionModel();
            IExtensionVersionService evs = new ExtensionVersionService(dnaMock.Object);

            // Act
            evs.SetExtensionVersion(aem);

            // Assert
            ClassicAssert.AreSame(version, aem.ExtensionVersion);
        }
    }
}