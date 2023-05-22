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
#pragma warning restore CA1031
    }
}
