using Xunit;

namespace UnitTests.BLL
{
    using System;
    using EFCorePowerTools.BLL;
    using EFCorePowerTools.Common.BLL;
    using EFCorePowerTools.Common.DAL;
    using EFCorePowerTools.Common.Models;
    using Moq;

    public class ExtensionVersionServiceTests
    {
        [Fact]
        public void Constructor_ArgumentNullException()
        {
            // Arrange
            IDotNetAccess dna = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ExtensionVersionService(dna));
        }

        [Fact]
        public void SetExtensionVersion_ArgumentNullException()
        {
            // Arrange
            var dna = Mock.Of<IDotNetAccess>();
            IExtensionVersionService evs = new ExtensionVersionService(dna);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => evs.SetExtensionVersion(null));
        }

        [Fact]
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
            Assert.Equal(version, aem.ExtensionVersion);
        }
    }
}
