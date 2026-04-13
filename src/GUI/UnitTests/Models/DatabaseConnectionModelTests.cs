using EFCorePowerTools.Common.Models;
using Xunit;
using RevEng.Common;

namespace UnitTests.Models
{
    public class DatabaseConnectionModelTests
    {
        [Fact]
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
            Assert.Equal(0, invokes);
            Assert.Null(dcm.ConnectionName);
            Assert.Null(dcm.ConnectionString);
            Assert.Equal(DatabaseType.Undefined, dcm.DatabaseType);
        }

        [Fact]
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
            Assert.Equal(3, invokes);
            Assert.Equal(connectionName, dcm.ConnectionName);
            Assert.Equal(connectionString, dcm.ConnectionString);
            Assert.Equal(dbType, dcm.DatabaseType);
        }
    }
}