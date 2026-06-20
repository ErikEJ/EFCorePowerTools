using RevEng.Common;
using RevEng.Core;
using Xunit;

namespace UnitTests
{
    public class SqlServerHelperTests
    {
        [Fact]
        public void ApplyDatabaseTypeAddsExplicitEncryptAndTrustServerCertificate()
        {
            var connectionString = "Data Source=.;Initial Catalog=Test;Integrated Security=True";

            var result = connectionString.ApplyDatabaseType(DatabaseType.SQLServer);

            Assert.Contains("Encrypt=True", result);
            Assert.Contains("Trust Server Certificate=True", result);
            Assert.Contains("Command Timeout=300", result);
        }
    }
}
