using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Helpers;
using NUnit.Framework;
using RevEng.Common;

namespace UnitTests.BLL
{
    [TestFixture]
    public class CredentialStoreTest
    {
        [Test]
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

            Assert.IsTrue(saved);

            var connections = credentialStore.GetStoredDatabaseConnections();

            Assert.AreEqual(1, connections.Count);
            Assert.AreEqual("test", connections[0].ConnectionName);
            Assert.AreEqual("Data Source=test", connections[0].ConnectionString);
            Assert.AreEqual(DatabaseType.SQLite, connections[0].DatabaseType);

            var deleted = credentialStore.DeleteCredential("test");

            Assert.IsTrue(deleted);
        }
    }
}
