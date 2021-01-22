using Microsoft.Data.SqlClient;
using RevEng.Shared;

namespace RevEng.Core
{
    public static class SqlServerHelper
    {
        public static string SetConnectionString(DatabaseType databaseType, string connectionString)
        {
            if (databaseType == DatabaseType.SQLServer)
            {
                var builder = new SqlConnectionStringBuilder(connectionString)
                {
                    CommandTimeout = 300,
                    TrustServerCertificate = true,
                };
                return builder.ConnectionString;
            }

            return connectionString;
        }
    }
}
