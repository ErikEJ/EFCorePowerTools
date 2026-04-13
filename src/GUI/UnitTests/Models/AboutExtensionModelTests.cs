using Xunit;

namespace UnitTests.Models
{
    using EFCorePowerTools.Common.Models;
    public class AboutExtensionModelTests
    {
        [Fact]
        public void SourceCodeUrl_NotEmpty()
        {
            // Arrange
            // Act
            var sourceCodeUrl = AboutExtensionModel.SourceCodeUrl;

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(sourceCodeUrl));
        }

        [Fact]
        public void MarketplaceUrl_NotEmpty()
        {
            // Arrange
            // Act
            var marketplaceUrl = AboutExtensionModel.MarketplaceUrl;

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(marketplaceUrl));
        }
    }
}