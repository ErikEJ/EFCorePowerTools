using EFCorePowerTools.Shared.Models;
using NUnit.Framework;
using ReverseEngineer20;

namespace UnitTests.Models
{
    [TestFixture]
    public class DatabaseConnectionModelTests
    {
        [Test]
        public void PropertyChanged_NotInvokedForEqualValues()
        {
            // Arrange
            var invokes = 0;
            var dcm = new DatabaseConnectionModel();
            dcm.PropertyChanged += (sender, args) => invokes++;

            // Act
            dcm.ConnectionName = null;
            dcm.ConnectionString = null;
            dcm.DatabaseType = DatabaseType.Undefined;

            // Assert
            Assert.AreEqual(0, invokes);
            Assert.IsNull(dcm.ConnectionName);
            Assert.IsNull(dcm.ConnectionString);
            Assert.AreEqual(DatabaseType.Undefined, dcm.DatabaseType);
        }

        [Test]
        public void PropertyChanged_InvokedForDifferentValues()
        {
            // Arrange
            var invokes = 0;
            var dcm = new DatabaseConnectionModel();
            dcm.PropertyChanged += (sender, args) => invokes++;
            const string connectionName = "foobar";
            const string connectionString = "Data Source=...;Initial Catalog=...;User=...;Password=...";
            const DatabaseType dbType = DatabaseType.SQLite;

            // Act
            dcm.ConnectionName = connectionName;
            dcm.ConnectionString = connectionString;
            dcm.DatabaseType = dbType;

            // Assert
            Assert.AreEqual(3, invokes);
            Assert.AreEqual(connectionName, dcm.ConnectionName);
            Assert.AreEqual(connectionString, dcm.ConnectionString);
            Assert.AreEqual(dbType, dcm.DatabaseType);
        }
    }
}