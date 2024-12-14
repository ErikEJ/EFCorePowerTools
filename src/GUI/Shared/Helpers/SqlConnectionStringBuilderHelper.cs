using System.Data.SqlClient;

namespace EFCorePowerTools.Helpers
{
    internal class SqlConnectionStringBuilderHelper
    {
        public SqlConnectionStringBuilder GetBuilder(string connectionString)
        {
            connectionString = ReplaceMdsKeywords(connectionString);
            return new SqlConnectionStringBuilder(connectionString);
        }

        private string ReplaceMdsKeywords(string connectionString)
        {
            // https://github.com/dotnet/SqlClient/blob/main/release-notes/2.0/2.0.0.md#new-connection-string-property-synonyms
            connectionString = connectionString.Replace("Application Intent=", "ApplicationIntent=");
            connectionString = connectionString.Replace("Connect Retry Count=", "ConnectRetryCount=");
            connectionString = connectionString.Replace("Connect Retry Interval=", "ConnectRetryInterval=");
            connectionString = connectionString.Replace("Pool Blocking Period=", "PoolBlockingPeriod=");
            connectionString = connectionString.Replace("Multiple Active Result Sets=", "MultipleActiveResultSets=");
            connectionString = connectionString.Replace("Multi Subnet Failover=", "MultiSubnetFailover=");
            connectionString = connectionString.Replace("Transparent Network IP Resolution=", "TransparentNetworkIPResolution=");
            connectionString = connectionString.Replace("Trust Server Certificate=", "TrustServerCertificate=");

            return connectionString;
        }
    }
}
