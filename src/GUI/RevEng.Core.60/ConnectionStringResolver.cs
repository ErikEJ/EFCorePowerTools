using System;
using System.Collections.Generic;
#if CORE60ONLY
using FirebirdSql.Data.FirebirdClient;
#endif
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using RevEng.Common;

namespace RevEng.Core
{
    public class ConnectionStringResolver
    {
        private readonly string connectionString;

        public ConnectionStringResolver(string connectionString)
        {
            ArgumentNullException.ThrowIfNull(connectionString);

            this.connectionString = connectionString;
        }

#pragma warning disable CA1031
        public IList<string> ResolveAlias()
        {
            var aliases = new List<string>();

            try
            {
                var _ = new SqlConnectionStringBuilder(connectionString);
                aliases.Add("mssql");
            }
            catch
            {
                // Ignore
            }

            try
            {
                var _ = new NpgsqlConnectionStringBuilder(connectionString);
                aliases.Add("postgres");
            }
            catch
            {
                // Ignore
            }

            try
            {
                var _ = new SqliteConnectionStringBuilder(connectionString);
                aliases.Add("sqlite");
            }
            catch
            {
                // Ignore
            }

            try
            {
                var _ = new MySqlConnectionStringBuilder(connectionString);
                aliases.Add("mysql");
            }
            catch
            {
                // Ignore
            }

            try
            {
                var _ = new OracleConnectionStringBuilder(connectionString);
                aliases.Add("oracle");
            }
            catch
            {
                // Ignore
            }

#if CORE60ONLY
            try
            {
                var _ = new FbConnectionStringBuilder(connectionString);
                aliases.Add("oracle");
            }
            catch
            {
                // Ignore
            }
#endif

            return aliases;
        }

        public string Redact(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.Undefined:
                    return connectionString;
                case DatabaseType.SQLServer:
                    var sqlBuilder = new SqlConnectionStringBuilder(connectionString);
                    sqlBuilder.Remove(nameof(SqlConnectionStringBuilder.Password));
                    sqlBuilder.Remove("User ID");

                    return sqlBuilder.ToString();
                case DatabaseType.SQLite:
                    return connectionString;
                case DatabaseType.Npgsql:
                    var builder = new NpgsqlConnectionStringBuilder(connectionString);
                    builder.Remove(nameof(NpgsqlConnectionStringBuilder.Password));
                    builder.Remove(nameof(NpgsqlConnectionStringBuilder.Username));

                    return builder.ToString();
                case DatabaseType.Mysql:
                    var myBuilder = new MySqlConnectionStringBuilder(connectionString);
                    myBuilder.Remove(nameof(MySqlConnectionStringBuilder.Password));
                    myBuilder.Remove(nameof(NpgsqlConnectionStringBuilder.Username));

                    return myBuilder.ToString();
                case DatabaseType.Oracle:
                    var oraBuilder = new OracleConnectionStringBuilder(connectionString);
                    oraBuilder.Remove(nameof(OracleConnectionStringBuilder.Password));
                    oraBuilder.Remove("User ID");

                    return oraBuilder.ToString();
                case DatabaseType.SQLServerDacpac:
                    return connectionString;
#if CORE60ONLY
                case DatabaseType.Firebird:
                    var fireBuilder = new FbConnectionStringBuilder(connectionString);
                    fireBuilder.Remove(nameof(FbConnectionStringBuilder.Password));
                    fireBuilder.Remove("User ID");

                    return fireBuilder.ToString();
#endif
                default:
                    return connectionString;
            }
        }
#pragma warning restore CA1031
    }
}
