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
            connectionString = connectionString.Replace("Multiple Active Result Sets=", "MultipleActiveResultSets=");
            connectionString = connectionString.Replace("Trust Server Certificate=", "TrustServerCertificate=");
            return connectionString;
        }
    }
}
