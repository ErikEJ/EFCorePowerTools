using System;
using RevEng.Common;

namespace RevEng.Core
{
    internal static class SkuHelper
    {
        public static (int Edition, int Version) GetSkuInfo(string connectionString, DatabaseType databaseType)
        {
            var edition = 0;
            var version = 0;

            switch (databaseType)
            {
                case DatabaseType.Undefined:
                    break;
                case DatabaseType.SQLServer:

                    try
                    {
                        var sql = @"SELECT SERVERPROPERTY('EngineEdition') AS Edition, SERVERPROPERTY('ProductVersion') AS ProductVersion";

                        using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                        {
                            connection.Open();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                            using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        edition = reader.GetInt32(0);
                                        version = new Version(reader.GetString(1)).Major;
                                    }
                                }
                            }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                        }
                    }
                    catch
                    {
                        // Ignore
                    }

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

            return (edition, version);
        }
    }
}
