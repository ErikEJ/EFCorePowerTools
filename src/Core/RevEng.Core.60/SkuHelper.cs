using System;
using Microsoft.Data.SqlClient;
using RevEng.Common;

namespace RevEng.Core
{
    internal static class SkuHelper
    {
        public static (int Edition, int Version, int Level) GetSkuInfo(string connectionString, DatabaseType databaseType)
        {
            var edition = 0;
            var version = 0;
            var compatibilityLevel = 0;

            switch (databaseType)
            {
                case DatabaseType.Undefined:
                    break;
                case DatabaseType.SQLServer:

                    try
                    {
                        var sql = @"
SELECT SERVERPROPERTY('EngineEdition') AS Edition, 
SERVERPROPERTY('ProductVersion') AS ProductVersion, 
compatibility_level As CompatibilityLevel 
FROM sys.databases WHERE name = @p1;";

                        var builder = new SqlConnectionStringBuilder(connectionString);

                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                            using (var command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.Add(new SqlParameter("@p1", builder.InitialCatalog));

                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        edition = reader.GetInt32(0);
                                        version = new Version(reader.GetString(1)).Major;
                                        compatibilityLevel = reader.GetByte(2);
                                    }
                                }
                            }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                        }
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch
                    {
                        // Ignore
                    }
#pragma warning restore CA1031 // Do not catch general exception types

                    break;
                case DatabaseType.SQLite:
                    break;
                case DatabaseType.Npgsql:
                    break;
                case DatabaseType.Mysql:
                    break;
                case DatabaseType.Oracle:
                    break;
                case DatabaseType.SQLServerDacpac:
                    break;
                case DatabaseType.Firebird:
                    break;
                default:
                    break;
            }

            return (edition, version, compatibilityLevel);
        }
    }
}
