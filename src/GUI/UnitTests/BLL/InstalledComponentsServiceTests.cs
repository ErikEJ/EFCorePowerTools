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
            IFileSystemAccess fsa = null;
            IDotNetAccess dna = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new InstalledComponentsService(vsa, fsa, dna));
        }

        [Test]
        public void Constructor_ArgumentNullException_FileSystemAccess()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            IFileSystemAccess fsa = null;
            IDotNetAccess dna = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new InstalledComponentsService(vsa, fsa, dna));
        }

        [Test]
        public void Constructor_ArgumentNullException_DotNetAccess()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();
            IDotNetAccess dna = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new InstalledComponentsService(vsa, fsa, dna));
        }

        [Test]
        public void SetMissingComponentData_ArgumentNullException()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();
            var dna = Mock.Of<IDotNetAccess>();
            IInstalledComponentsService ics = new InstalledComponentsService(vsa, fsa, dna);

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
            var sqlCe40Version = new Version(30, 0, 14);
            var fsaMock = new Mock<IFileSystemAccess>();
            fsaMock.Setup(m => m.GetInstalledSqlCe40Version()).Returns(sqlCe40Version);
            var adoNetProviderVersion = new Version(50, 10);
            var dnaMock = new Mock<IDotNetAccess>();
            dnaMock.Setup(m => m.DoesDbProviderFactoryExist(It.IsNotNull<string>())).Returns(true);
            dnaMock.Setup(m => m.GetAssemblyVersion(It.IsNotNull<string>())).Returns(adoNetProviderVersion);
            IInstalledComponentsService ics = new InstalledComponentsService(vsaMock.Object, fsaMock.Object, dnaMock.Object);
            var aem = new AboutExtensionModel();

            // Act
            ics.SetMissingComponentData(aem);

            // Assert
            Assert.AreSame(sqlCe40Version, aem.SqlServerCompact40GacVersion);
            Assert.IsTrue(aem.SqlServerCompact40DbProviderInstalled);
            Assert.IsTrue(aem.SqlServerCompact40DdexProviderInstalled);
            Assert.IsTrue(aem.SqlServerCompact40SimpleDdexProviderInstalled);
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
            var sqlCe40Version = new Version(30, 0, 14);
            var fsaMock = new Mock<IFileSystemAccess>();
            fsaMock.Setup(m => m.GetInstalledSqlCe40Version()).Returns(sqlCe40Version);
            var adoNetProviderVersion = new Version(50, 10);
            var dnaMock = new Mock<IDotNetAccess>();
            dnaMock.Setup(m => m.DoesDbProviderFactoryExist(It.IsNotNull<string>())).Returns(true);
            dnaMock.Setup(m => m.GetAssemblyVersion(It.IsNotNull<string>())).Returns(adoNetProviderVersion);
            IInstalledComponentsService ics = new InstalledComponentsService(vsaMock.Object, fsaMock.Object, dnaMock.Object);
            var aem = new AboutExtensionModel
            {
                SqlServerCompact40GacVersion = new Version(1, 0, 1),
                SqlLiteSimpleDdexProviderInstalled = false,
                SqLiteDdexProviderInstalled = false,
                SqLiteEf6DbProviderInstalled = false,
                SqLiteAdoNetProviderVersion = new Version(4, 0, 1),
                SqlServerCompact40SimpleDdexProviderInstalled = false,
                SqlServerCompact40DbProviderInstalled = false,
                SqlServerCompact40DdexProviderInstalled = false
            };

            // Act
            ics.SetMissingComponentData(aem);

            // Assert
            Assert.AreNotSame(sqlCe40Version, aem.SqlServerCompact40GacVersion);
            Assert.IsFalse(aem.SqlServerCompact40DbProviderInstalled);
            Assert.IsFalse(aem.SqlServerCompact40DdexProviderInstalled);
            Assert.IsFalse(aem.SqlServerCompact40SimpleDdexProviderInstalled);
            Assert.AreNotSame(adoNetProviderVersion, aem.SqLiteAdoNetProviderVersion);
            Assert.IsFalse(aem.SqLiteEf6DbProviderInstalled);
            Assert.IsFalse(aem.SqLiteDdexProviderInstalled);
            Assert.IsFalse(aem.SqlLiteSimpleDdexProviderInstalled);
        }
    }
}