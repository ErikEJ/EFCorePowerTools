using Xunit;

namespace UnitTests.Models
{
    using EFCorePowerTools.Common.Models;

    public class DatabaseDefinitionModelTests
    {
        [Fact]
        public void PropertyChanged_NotInvokedForEqualValues()
        {
            // Arrange
            var invokes = 0;
            var ddm = new DatabaseConnectionModel();
            ddm.PropertyChanged += (sender, args) => invokes++;

            // Act
            ddm.FilePath = null;

            // Assert
            Assert.Equal(0, invokes);
            Assert.Null(ddm.FilePath);
        }

        [Fact]
        public void PropertyChanged_InvokedForDifferentValues()
        {
            // Arrange
            var invokes = 0;
            var ddm = new DatabaseConnectionModel();
            ddm.PropertyChanged += (sender, args) => invokes++;
            const string filePath = @"C:\Temp\Test\Database.dacpac";

            // Act
            ddm.FilePath = filePath;

            // Assert
            Assert.Equal(1, invokes);
            Assert.Equal(filePath, ddm.FilePath);
        }
    }
}
