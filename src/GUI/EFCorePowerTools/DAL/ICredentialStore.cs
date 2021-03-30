using EFCorePowerTools.Shared.Models;
using RevEng.Shared;
using System.Collections.Generic;

namespace EFCorePowerTools.DAL
{
    public interface ICredentialStore
    {
        bool DeleteCredential(string name);
        List<DatabaseConnectionModel> GetStoredDatabaseConnections();
        bool SaveCredential(DatabaseConnectionModel connectionModel);
    }
}