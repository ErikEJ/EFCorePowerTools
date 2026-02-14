using System;
using System.Collections.Generic;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
#if !CORE100
using MySqlConnector;
#endif
using Npgsql;
#if !CORE100
using Oracle.ManagedDataAccess.Client;
#endif

namespace RevEng.Core
{
    public class ConnectionStringResolver
    {
        private readonly string connectionString;
        private readonly bool isDacpac;

        public ConnectionStringResolver(string connectionString)
        {
            ArgumentNullException.ThrowIfNull(connectionString);

            this.connectionString = connectionString;

            if (connectionString.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase))
            {
                isDacpac = true;
                var database = Path.GetFileNameWithoutExtension(connectionString);
                this.connectionString = $"Data Source=(local);Initial Catalog={database};Integrated Security=true;Encrypt=false";
            }
        }

#pragma warning disable CA1031
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "Part of logic")]
        public IList<string> ResolveAlias()
        {
            var aliases = new List<string>();

            if (isDacpac)
            {
                aliases.Add("mssql");
                return aliases;
            }

            try
            {
                var a = new SqlConnectionStringBuilder(connectionString);
                aliases.Add("mssql");
            }
            catch
            {
                // Ignore
            }

            try
            {
                var a = new NpgsqlConnectionStringBuilder(connectionString);
                aliases.Add("postgres");
            }
            catch
            {
                // Ignore
            }

            try
            {
                var a = new SqliteConnectionStringBuilder(connectionString);
                aliases.Add("sqlite");
            }
            catch
            {
                // Ignore
            }

#if !CORE100
            try
            {
                var a = new OracleConnectionStringBuilder(connectionString);
                aliases.Add("oracle");
            }
            catch
            {
                // Ignore
            }

            try
            {
                var a = new MySqlConnectionStringBuilder(connectionString);
                aliases.Add("mysql");
            }
            catch
            {
                // Ignore
            }
#endif
            try
            {
                var a = new FbConnectionStringBuilder(connectionString);
                aliases.Add("firebird");
            }
            catch
            {
                // Ignore
            }

            return aliases;
        }
#pragma warning restore CA1031
    }
}
