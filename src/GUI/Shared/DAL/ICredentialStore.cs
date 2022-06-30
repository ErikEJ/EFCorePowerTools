using System.Collections.Generic;
using EFCorePowerTools.Common.Models;
using RevEng.Common;

namespace EFCorePowerTools.DAL
{
    public interface ICredentialStore
    {
        bool DeleteCredential(string name);
        List<DatabaseConnectionModel> GetStoredDatabaseConnections();
        bool SaveCredential(DatabaseConnectionModel connectionModel);
    }
}