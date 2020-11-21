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
    public class InstalledComponentsServiceTests
    {
        [Test]
        public void Constructor_ArgumentNullException_VisualStudioAccess()
        {
            // Arrange
            IVisualStudioAccess vsa = null;
            IDotNetAccess dna = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new InstalledComponentsService(vsa, dna));
        }

        [Test]
        public void Constructor_ArgumentNullException_FileSystemAccess()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            IDotNetAccess dna = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new InstalledComponentsService(vsa, dna));
        }

        [Test]
        public void Constructor_ArgumentNullException_DotNetAccess()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            IDotNetAccess dna = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new InstalledComponentsService(vsa, dna));
        }

        [Test]
        public void SetMissingComponentData_ArgumentNullException()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var dna = Mock.Of<IDotNetAccess>();
            IInstalledComponentsService ics = new InstalledComponentsService(vsa, dna);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ics.SetMissingComponentData(null));
        }

        [Test]
        public void SetMissingComponentData_SetMissingValues()
        {
            // Arrange
            var vsaMock = new Mock<IVisualStudioAccess>();
            vsaMock.Setup(m => m.IsDdexProviderInstalled(It.IsAny<Guid>())).Returns(true);
            vsaMock.Setup(m => m.IsSqLiteDbProviderInstalled()).Returns(true);
            var adoNetProviderVersion = new Version(50, 10);
            var dnaMock = new Mock<IDotNetAccess>();
            dnaMock.Setup(m => m.DoesDbProviderFactoryExist(It.IsNotNull<string>())).Returns(true);
            dnaMock.Setup(m => m.GetAssemblyVersion(It.IsNotNull<string>())).Returns(adoNetProviderVersion);
            IInstalledComponentsService ics = new InstalledComponentsService(vsaMock.Object, dnaMock.Object);
            var aem = new AboutExtensionModel();

            // Act
            ics.SetMissingComponentData(aem);

            // Assert
            Assert.AreSame(adoNetProviderVersion, aem.SqLiteAdoNetProviderVersion);
            Assert.IsTrue(aem.SqLiteEf6DbProviderInstalled);
            Assert.IsTrue(aem.SqLiteDdexProviderInstalled);
            Assert.IsTrue(aem.SqlLiteSimpleDdexProviderInstalled);
        }

        [Test]
        public void SetMissingComponentData_DontOverrideExistingValues()
        {
            // Arrange
            var vsaMock = new Mock<IVisualStudioAccess>();
            vsaMock.Setup(m => m.IsDdexProviderInstalled(It.IsAny<Guid>())).Returns(true);
            vsaMock.Setup(m => m.IsSqLiteDbProviderInstalled()).Returns(true);
            var adoNetProviderVersion = new Version(50, 10);
            var dnaMock = new Mock<IDotNetAccess>();
            dnaMock.Setup(m => m.DoesDbProviderFactoryExist(It.IsNotNull<string>())).Returns(true);
            dnaMock.Setup(m => m.GetAssemblyVersion(It.IsNotNull<string>())).Returns(adoNetProviderVersion);
            IInstalledComponentsService ics = new InstalledComponentsService(vsaMock.Object, dnaMock.Object);
            var aem = new AboutExtensionModel
            {
                SqlLiteSimpleDdexProviderInstalled = false,
                SqLiteDdexProviderInstalled = false,
                SqLiteEf6DbProviderInstalled = false,
                SqLiteAdoNetProviderVersion = new Version(4, 0, 1),
            };

            // Act
            ics.SetMissingComponentData(aem);

            // Assert
            Assert.AreNotSame(adoNetProviderVersion, aem.SqLiteAdoNetProviderVersion);
            Assert.IsFalse(aem.SqLiteEf6DbProviderInstalled);
            Assert.IsFalse(aem.SqLiteDdexProviderInstalled);
            Assert.IsFalse(aem.SqlLiteSimpleDdexProviderInstalled);
        }
    }
}