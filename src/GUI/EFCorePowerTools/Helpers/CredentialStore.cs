using AdysTech.CredentialManager;
using EFCorePowerTools.DAL;
using EFCorePowerTools.Shared.Models;
using RevEng.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace EFCorePowerTools.Helpers
{
    public class CredentialStore : ICredentialStore
    {
        private const string Root = "efpt:";

        public List<DatabaseConnectionModel> GetStoredDatabaseConnections()
        {
            var result = new List<DatabaseConnectionModel>();

            var list = CredentialManager.EnumerateICredentials();

            if (list == null)
            {
                return result;
            }
            
            var credentials = list.Where(c => c.TargetName.StartsWith($"{Root}", System.StringComparison.Ordinal)).ToList();

            foreach (var credential in credentials)
            {
                var storedCredential = GetDatabaseConnectionModel(credential);
                if (storedCredential != null)
                {
                    result.Add(storedCredential);
                }
            }

            return result.OrderBy(c => c.ConnectionName).ToList();
        }


        public bool DeleteCredential(string name)
        {
            var target = Root + name;

            return CredentialManager.RemoveCredentials(target);
        }

        public bool SaveCredential(DatabaseConnectionModel databaseConnection)
        {
            var cred = (new NetworkCredential(databaseConnection.ConnectionName, databaseConnection.ConnectionString)).ToICredential();
            cred.TargetName = Root + databaseConnection.ConnectionName;
            cred.Attributes = new Dictionary<string, object>
            {
                { nameof(DatabaseType), (int)databaseConnection.DatabaseType }
            };
            cred.Persistance = Persistance.LocalMachine;
            cred.Type = CredentialType.Generic;
            return cred.SaveCredential();
        }

        private DatabaseConnectionModel GetDatabaseConnectionModel(ICredential credential)
        {
            try
            {
                var result = new DatabaseConnectionModel
                {
                    ConnectionName = credential.TargetName.Substring(5),
                    ConnectionString = credential.ToNetworkCredential().Password,
                    DatabaseType = (DatabaseType)credential.Attributes[nameof(DatabaseType)],
                };

                return result;
            }
            catch
            {
                // Ignore
            }

            return null;
        }
    }
}
