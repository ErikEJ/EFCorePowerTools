using NUnit.Framework;

namespace UnitTests.Models
{
    using System;
    using System.Collections.Generic;
    using EFCorePowerTools.Shared.Models;

    [TestFixture]
    public class AboutExtensionModelTests
    {
        [Test]
        public void SourceCodeUrl_NotEmpty()
        {
            // Arrange
            var aem = new AboutExtensionModel();

            // Act
            var sourceCodeUrl = aem.SourceCodeUrl;

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(sourceCodeUrl));
        }

        [Test]
        public void MarketplaceUrl_NotEmpty()
        {
            // Arrange
            var aem = new AboutExtensionModel();

            // Act
            var marketplaceUrl = aem.MarketplaceUrl;

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(marketplaceUrl));
        }

        [Test]
        public void PropertyChanged_NotInvokedForEqualValues()
        {
            // Arrange
            var invokes = 0;
            var aem = new AboutExtensionModel();
            aem.PropertyChanged += (sender, args) => invokes++;

            // Act
            aem.SqlServerCompact40GacVersion = null;
            aem.SqlServerCompact40DbProviderInstalled = null;
            aem.SqlServerCompact40DdexProviderInstalled = null;
            aem.SqlServerCompact40SimpleDdexProviderInstalled = null;
            aem.SqLiteAdoNetProviderVersion = null;
            aem.SqLiteEf6DbProviderInstalled = null;
            aem.SqLiteDdexProviderInstalled = null;
            aem.SqlLiteSimpleDdexProviderInstalled = null;

            // Assert
            Assert.AreEqual(0, invokes);
            Assert.IsNull(aem.SqlServerCompact40GacVersion);
            Assert.IsNull(aem.SqlServerCompact40DbProviderInstalled);
            Assert.IsNull(aem.SqlServerCompact40DdexProviderInstalled);
            Assert.IsNull(aem.SqlServerCompact40SimpleDdexProviderInstalled);
            Assert.IsNull(aem.SqLiteAdoNetProviderVersion);
            Assert.IsNull(aem.SqLiteEf6DbProviderInstalled);
            Assert.IsNull(aem.SqLiteDdexProviderInstalled);
            Assert.IsNull(aem.SqlLiteSimpleDdexProviderInstalled);
        }

        [Test]
        public void PropertyChanged_InvokedForDifferentValues()
        {
            // Arrange
            var invokes = new List<string>();
            var aem = new AboutExtensionModel();
            aem.PropertyChanged += (sender, args) => invokes.Add(args.PropertyName);
            var v1 = new Version(5, 4, 3, 2);
            var v2 = new Version(6, 7, 10, 0);

            // Act
            aem.SqlServerCompact40GacVersion = v1;
            aem.SqlServerCompact40DbProviderInstalled = true;
            aem.SqlServerCompact40DdexProviderInstalled = true;
            aem.SqlServerCompact40SimpleDdexProviderInstalled = true;
            aem.SqLiteAdoNetProviderVersion = v2;
            aem.SqLiteEf6DbProviderInstalled = true;
            aem.SqLiteDdexProviderInstalled = true;
            aem.SqlLiteSimpleDdexProviderInstalled = true;

            // Assert
            Assert.AreEqual(8, invokes.Count);
            Assert.AreSame(v1, aem.SqlServerCompact40GacVersion);
            Assert.IsTrue(aem.SqlServerCompact40DbProviderInstalled);
            Assert.IsTrue(aem.SqlServerCompact40DdexProviderInstalled);
            Assert.IsTrue(aem.SqlServerCompact40SimpleDdexProviderInstalled);
            Assert.AreSame(v2, aem.SqLiteAdoNetProviderVersion);
            Assert.IsTrue(aem.SqLiteEf6DbProviderInstalled);
            Assert.IsTrue(aem.SqLiteDdexProviderInstalled);
            Assert.IsTrue(aem.SqlLiteSimpleDdexProviderInstalled);
            Assert.AreEqual(nameof(AboutExtensionModel.SqlServerCompact40GacVersion), invokes[0]);
            Assert.AreEqual(nameof(AboutExtensionModel.SqlServerCompact40DbProviderInstalled), invokes[1]);
            Assert.AreEqual(nameof(AboutExtensionModel.SqlServerCompact40DdexProviderInstalled), invokes[2]);
            Assert.AreEqual(nameof(AboutExtensionModel.SqlServerCompact40SimpleDdexProviderInstalled), invokes[3]);
            Assert.AreEqual(nameof(AboutExtensionModel.SqLiteAdoNetProviderVersion), invokes[4]);
            Assert.AreEqual(nameof(AboutExtensionModel.SqLiteEf6DbProviderInstalled), invokes[5]);
            Assert.AreEqual(nameof(AboutExtensionModel.SqLiteDdexProviderInstalled), invokes[6]);
            Assert.AreEqual(nameof(AboutExtensionModel.SqlLiteSimpleDdexProviderInstalled), invokes[7]);
        }
    }
}