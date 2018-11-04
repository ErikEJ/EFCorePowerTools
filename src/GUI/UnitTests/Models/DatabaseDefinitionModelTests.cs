using NUnit.Framework;

namespace UnitTests.Models
{
    using EFCorePowerTools.Shared.Models;

    [TestFixture]
    public class DatabaseDefinitionModelTests
    {
        [Test]
        public void PropertyChanged_NotInvokedForEqualValues()
        {
            // Arrange
            var invokes = 0;
            var ddm = new DatabaseDefinitionModel();
            ddm.PropertyChanged += (sender, args) => invokes++;

            // Act
            ddm.FilePath = null;

            // Assert
            Assert.AreEqual(0, invokes);
            Assert.IsNull(ddm.FilePath);
        }

        [Test]
        public void PropertyChanged_InvokedForDifferentValues()
        {
            // Arrange
            var invokes = 0;
            var ddm = new DatabaseDefinitionModel();
            ddm.PropertyChanged += (sender, args) => invokes++;
            const string filePath = @"C:\Temp\Test\Database.dacpac";

            // Act
            ddm.FilePath = filePath;

            // Assert
            Assert.AreEqual(1, invokes);
            Assert.AreEqual(filePath, ddm.FilePath);
        }
    }
}