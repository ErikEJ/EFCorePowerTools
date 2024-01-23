using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Helpers;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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

            ClassicAssert.IsTrue(saved);

            var connections = credentialStore.GetStoredDatabaseConnections();

            ClassicAssert.AreEqual(1, connections.Count);
            ClassicAssert.AreEqual("test", connections[0].ConnectionName);
            ClassicAssert.AreEqual("Data Source=test", connections[0].ConnectionString);
            ClassicAssert.AreEqual(DatabaseType.SQLite, connections[0].DatabaseType);

            var deleted = credentialStore.DeleteCredential("test");

            ClassicAssert.IsTrue(deleted);
        }
    }
}
