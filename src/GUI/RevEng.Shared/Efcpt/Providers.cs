using System.Collections.Generic;

namespace RevEng.Common.Efcpt
{
    internal static class Providers
    {
        public static Dictionary<string, string> GetKnownProviders()
        {
            var result = new Dictionary<string, string>();

            foreach (var provider in GetProvidersWithAliases())
            {
                result.Add(provider.Key, provider.Key);

                foreach (var item in provider.Value)
                {
                    result.Add(item, provider.Key);
                }
            }

            return result;
        }

        public static DatabaseType GetDatabaseTypeFromProvider(string provider)
        {
            switch (provider)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    return DatabaseType.SQLServer;

                case "Microsoft.EntityFrameworkCore.Sqlite":
                    return DatabaseType.SQLite;

                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    return DatabaseType.Npgsql;

                case "Pomelo.EntityFrameworkCore.MySql":
                    return DatabaseType.Mysql;

                case "Oracle.EntityFrameworkCore":
                    return DatabaseType.Oracle;

                case "FirebirdSql.EntityFrameworkCore.Firebird":
                    return DatabaseType.Firebird;

                default:
                    return DatabaseType.Undefined;
            }
        }

        private static Dictionary<string, List<string>> GetProvidersWithAliases()
        {
            return new Dictionary<string, List<string>>
            {
                {
                    "Microsoft.EntityFrameworkCore.SqlServer",
                    new List<string> { "mssql" }
                },
                {
                    "Microsoft.EntityFrameworkCore.Sqlite",
                    new List<string> { "sqlite" }
                },
                {
                    "Npgsql.EntityFrameworkCore.PostgreSQL",
                    new List<string> { "postgres" }
                },
                {
                    "Pomelo.EntityFrameworkCore.MySql",
                    new List<string> { "mysql" }
                },                {
                    "Oracle.EntityFrameworkCore",
                    new List<string> { "oracle" }
                },                {
                    "FirebirdSql.EntityFrameworkCore.Firebird",
                    new List<string> { "firebird" }
                },
            };
        }
    }
}
