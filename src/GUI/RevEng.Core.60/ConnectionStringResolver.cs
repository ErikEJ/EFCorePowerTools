﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
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

            if (connectionString.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase))
            {
                var database = Path.GetFileNameWithoutExtension(connectionString);
                this.connectionString = $"Data Source=(local);Initial Catalog={database};Integrated Security=true";
            }
        }

#pragma warning disable CA1031
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "Part of logic")]
        public IList<string> ResolveAlias()
        {
            var aliases = new List<string>();

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

            try
            {
                var a = new MySqlConnectionStringBuilder(connectionString);
                aliases.Add("mysql");
            }
            catch
            {
                // Ignore
            }

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
                var a = new FbConnectionStringBuilder(connectionString);
                aliases.Add("oracle");
            }
            catch
            {
                // Ignore
            }

            return aliases;
        }

        public string Redact()
        {
            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connectionString;
            builder.Remove("Password");
            builder.Remove("User ID");
            builder.Remove("Username");

            return builder.ToString();
        }
#pragma warning restore CA1031
    }
}
