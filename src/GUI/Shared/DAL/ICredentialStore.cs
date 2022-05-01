using EFCorePowerTools.Common.Models;
using RevEng.Common;
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