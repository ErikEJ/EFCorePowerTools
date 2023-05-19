using Microsoft.Data.SqlClient;
using RevEng.Common;

namespace RevEng.Core
{
    public static class SqlServerHelper
    {
        public static string ApplyDatabaseType(this string connectionString, DatabaseType databaseType)
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
