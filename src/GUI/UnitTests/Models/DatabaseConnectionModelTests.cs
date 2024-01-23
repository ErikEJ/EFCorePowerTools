using EFCorePowerTools.Common.Models;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RevEng.Common;

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
            ClassicAssert.AreEqual(0, invokes);
            ClassicAssert.IsNull(dcm.ConnectionName);
            ClassicAssert.IsNull(dcm.ConnectionString);
            ClassicAssert.AreEqual(DatabaseType.Undefined, dcm.DatabaseType);
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
            ClassicAssert.AreEqual(3, invokes);
            ClassicAssert.AreEqual(connectionName, dcm.ConnectionName);
            ClassicAssert.AreEqual(connectionString, dcm.ConnectionString);
            ClassicAssert.AreEqual(dbType, dcm.DatabaseType);
        }
    }
}
