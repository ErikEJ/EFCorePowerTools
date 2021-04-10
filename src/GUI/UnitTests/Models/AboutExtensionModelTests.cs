using NUnit.Framework;

namespace UnitTests.Models
{
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
    }
}