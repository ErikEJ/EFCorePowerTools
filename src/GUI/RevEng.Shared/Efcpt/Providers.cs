using System.Collections.Generic;

namespace RevEng.Common.Efcpt
{
    public static class Providers
    {
        public static DatabaseType GetDatabaseTypeFromProvider(string providerAlias, bool isDacpac)
        {
            if (!GetKnownProviders().TryGetValue(providerAlias, out var provider))
            {
                return DatabaseType.Undefined;
            }

            switch (provider)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    if (isDacpac)
                    {
                        return DatabaseType.SQLServerDacpac;
                    }

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
