using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Helpers;
using RevEng.Common;
using Xunit;

namespace UnitTests.BLL
{
    public class CredentialStoreTest
    {
        [Fact]
        public void SaveCredential_Get_DeleteCredential()
        {
            CredentialStore credentialStore = new CredentialStore();

            var databaseConnection = new DatabaseConnectionModel
            {
                ConnectionName = "test",
                ConnectionString = "Data Source=test",
                DatabaseType = DatabaseType.SQLite,
                DataConnection = null,
            };

            var saved = credentialStore.SaveCredential(databaseConnection);

            Assert.True(saved);

            var connections = credentialStore.GetStoredDatabaseConnections();

            var connection = Assert.Single(connections);
            Assert.Equal("test", connection.ConnectionName);
            Assert.Equal("Data Source=test", connection.ConnectionString);
            Assert.Equal(DatabaseType.SQLite, connection.DatabaseType);

            var deleted = credentialStore.DeleteCredential("test");

            Assert.True(deleted);
        }
    }
}
