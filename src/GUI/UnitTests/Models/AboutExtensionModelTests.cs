using NUnit.Framework;

namespace UnitTests.Models
{
    using EFCorePowerTools.Common.Models;

    [TestFixture]
    public class AboutExtensionModelTests
    {
        [Test]
        public void SourceCodeUrl_NotEmpty()
        {
            // Arrange
            // Act
            var sourceCodeUrl = AboutExtensionModel.SourceCodeUrl;

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(sourceCodeUrl));
        }

        [Test]
        public void MarketplaceUrl_NotEmpty()
        {
            // Arrange
            // Act
            var marketplaceUrl = AboutExtensionModel.MarketplaceUrl;

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(marketplaceUrl));
        }
    }
}
