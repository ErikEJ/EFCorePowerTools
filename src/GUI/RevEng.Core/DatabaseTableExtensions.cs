using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RevEng.Shared;

namespace RevEng.Core
{
    public static class DatabaseTableExtensions
    {
        public static string GetFullName(this DatabaseTable databaseTable, DatabaseType databaseType)
        {
            if (databaseType == DatabaseType.SQLServer || databaseType == DatabaseType.SQLServerDacpac)
            {
                return $"[{databaseTable.Schema}].[{databaseTable.Name}]";
            }
            else
            {
                return string.IsNullOrEmpty(databaseTable.Schema)
                    ? databaseTable.Name
                    : $"{databaseTable.Schema}.{databaseTable.Name}";
            }
        }
    }
}
