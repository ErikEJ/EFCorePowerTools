using System.Collections.Generic;
using EFCorePowerTools.Common.Models;

namespace EFCorePowerTools.DAL
{
    public interface ICredentialStore
    {
        bool DeleteCredential(string name);

        List<DatabaseConnectionModel> GetStoredDatabaseConnections();

        bool SaveCredential(DatabaseConnectionModel connectionModel);
    }
}