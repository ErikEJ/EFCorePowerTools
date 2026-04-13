using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Helpers;
using Xunit;
using RevEng.Common;

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

            Assert.Equal(1, connections.Count);
            Assert.Equal("test", connections[0].ConnectionName);
            Assert.Equal("Data Source=test", connections[0].ConnectionString);
            Assert.Equal(DatabaseType.SQLite, connections[0].DatabaseType);

            var deleted = credentialStore.DeleteCredential("test");

            Assert.True(deleted);
        }
    }
}