using NUnit.Framework;

namespace UnitTests.BLL
{
    using System;
    using EFCorePowerTools.BLL;
    using EFCorePowerTools.Shared.BLL;
    using EFCorePowerTools.Shared.DAL;
    using EFCorePowerTools.Shared.Models;
    using Moq;

    [TestFixture]
    public class ExtensionVersionServiceTests
    {
        [Test]
        public void Constructor_ArgumentNullException()
        {
            // Arrange
            IDotNetAccess dna = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ExtensionVersionService(dna));
        }

        [Test]
        public void SetExtensionVersion_ArgumentNullException()
        {
            // Arrange
            var dna = Mock.Of<IDotNetAccess>();
            IExtensionVersionService evs = new ExtensionVersionService(dna);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => evs.SetExtensionVersion(null));
        }

        [Test]
        public void SetExtensionVersion_ProvideVersion()
        {
            // Arrange
            var version = new Version(50, 10, 0, 4);
            var dnaMock = new Mock<IDotNetAccess>();
            dnaMock.Setup(m => m.GetExtensionVersion()).Returns(version);
            var aem = new AboutExtensionModel();
            IExtensionVersionService evs = new ExtensionVersionService(dnaMock.Object);

            // Act
            evs.SetExtensionVersion(aem);

            // Assert
            Assert.AreSame(version, aem.ExtensionVersion);
        }
    }
}