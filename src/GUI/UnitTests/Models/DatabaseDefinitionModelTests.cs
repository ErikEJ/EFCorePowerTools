using NUnit.Framework;

namespace UnitTests.Models
{
    using EFCorePowerTools.Common.Models;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class DatabaseDefinitionModelTests
    {
        [Test]
        public void PropertyChanged_NotInvokedForEqualValues()
        {
            // Arrange
            var invokes = 0;
            var ddm = new DatabaseConnectionModel();
            ddm.PropertyChanged += (sender, args) => invokes++;

            // Act
            ddm.FilePath = null;

            // Assert
            ClassicAssert.AreEqual(0, invokes);
            ClassicAssert.IsNull(ddm.FilePath);
        }

        [Test]
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
            ClassicAssert.AreEqual(1, invokes);
            ClassicAssert.AreEqual(filePath, ddm.FilePath);
        }
    }
}
