using FirebirdSql.Data.FirebirdClient;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using Npgsql;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Oracle.ManagedDataAccess.Client;
using RevEng.Core;

namespace UnitTests
{
    [TestFixture]
    public class ConnectionStringResolverTest
    {
        [Test]
        public void CanRedactSql()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ".";
            builder.Password = "password";
            builder.UserID = "userId";

            var resolver = new ConnectionStringResolver(builder.ConnectionString);

            var result = resolver.Redact();

            ClassicAssert.AreEqual("data source=.", result);
        }


        [Test]
        public void CanRedactNpgsql()
        {
            var builder = new NpgsqlConnectionStringBuilder();
            builder.Host = "localhost";
            builder.Password = "password";
            builder.Username = "userId";

            var resolver = new ConnectionStringResolver(builder.ConnectionString);

            var result = resolver.Redact();

            ClassicAssert.AreEqual("host=localhost", result);
        }

        [Test]
        public void CanRedactMysql()
        {
            var builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.Password = "password";
            builder.UserID = "userId";

            var resolver = new ConnectionStringResolver(builder.ConnectionString);

            var result = resolver.Redact();

            ClassicAssert.AreEqual("server=localhost", result);
        }

        [Test]
        public void CanRedactOracle()
        {
            var builder = new OracleConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.Password = "password";
            builder.UserID = "userId";

            var resolver = new ConnectionStringResolver(builder.ConnectionString);

            var result = resolver.Redact();

            ClassicAssert.AreEqual("data source=localhost", result);
        }

        [Test]
        public void CanRedactFirebird()
        {
            var builder = new FbConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.Password = "password";
            builder.UserID = "userId";

            var resolver = new ConnectionStringResolver(builder.ConnectionString);

            var result = resolver.Redact();

            ClassicAssert.AreEqual("data source=localhost", result);
        }
    }
}
