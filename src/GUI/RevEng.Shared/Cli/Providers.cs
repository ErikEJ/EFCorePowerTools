using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RevEng.Common.Cli
{
    public static class Providers
    {
        public static DatabaseType ToDatabaseType(this string providerAlias, bool isDacpac)
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

        public static string CreateReadme(string provider, ReverseEngineerCommandOptions commandOptions, CodeGenerationMode codeGenerationMode)
        {
            if (commandOptions == null)
            {
                throw new ArgumentNullException(nameof(commandOptions));
            }

            var packages = GetNeededPackages(
                commandOptions.DatabaseType,
                commandOptions.UseSpatial,
                commandOptions.UseNodaTime,
                commandOptions.UseDateOnlyTimeOnly,
                commandOptions.UseHierarchyId,
                commandOptions.UseMultipleSprocResultSets,
                commandOptions.Tables?.Any(t => t.ObjectType == ObjectType.Procedure) ?? false,
                codeGenerationMode);

            var readmeName = "efcpt-readme.md";

            var template = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), readmeName), Encoding.UTF8);

            var finalText = GetReadMeText(provider, commandOptions, template, packages);

            var readmePath = Path.Combine(commandOptions.ProjectPath, readmeName);

            File.WriteAllText(readmePath, finalText, Encoding.UTF8);

            return readmePath;
        }

        public static List<NuGetPackage> GetNeededPackages(DatabaseType databaseType, bool useSpatial, bool useNodaTime, bool useDateOnlyTimeOnly, bool useHierarchyId, bool discoverMultipleResultSets, bool hasProcedures, CodeGenerationMode codeGenerationMode)
        {
            // TODO Update versions here when adding provider updates
            var packages = new List<NuGetPackage>();

            if (databaseType == DatabaseType.SQLServer || databaseType == DatabaseType.SQLServerDacpac)
            {
                var pkgVersion = "7.0.5";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.16";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Microsoft.EntityFrameworkCore.SqlServer",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                    IsMainProviderPackage = true,
                    UseMethodName = "SqlServer",
                });

                if (useSpatial)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                        IsMainProviderPackage = false,
                        UseMethodName = "NetTopologySuite",
                    });
                }

                if (useNodaTime)
                {
                    pkgVersion = "7.0.0";
                    switch (codeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "6.0.1";
                            break;
                    }

                    packages.Add(new NuGetPackage
                    {
                        PackageId = "SimplerSoftware.EntityFrameworkCore.SqlServer.NodaTime",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                        IsMainProviderPackage = false,
                        UseMethodName = "NodaTime",
                    });
                }

                if (useHierarchyId)
                {
                    pkgVersion = "4.0.0";
                    switch (codeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "3.0.1";
                            break;
                    }

                    packages.Add(new NuGetPackage
                    {
                        PackageId = "EntityFrameworkCore.SqlServer.HierarchyId",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                        IsMainProviderPackage = false,
                        UseMethodName = "HierarchyId",
                    });
                }

                if (useDateOnlyTimeOnly)
                {
                    pkgVersion = "7.0.3";
                    switch (codeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "6.0.3";
                            break;
                    }

                    if (codeGenerationMode == CodeGenerationMode.EFCore6
                        || codeGenerationMode == CodeGenerationMode.EFCore7)
                    {
                        packages.Add(new NuGetPackage
                        {
                            PackageId = "ErikEJ.EntityFrameworkCore.SqlServer.DateOnlyTimeOnly",
                            Version = pkgVersion,
                            DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                            IsMainProviderPackage = false,
                            UseMethodName = "DateOnlyTimeOnly",
                        });
                    }
                }

                if (hasProcedures
                    && (codeGenerationMode == CodeGenerationMode.EFCore6
                        || codeGenerationMode == CodeGenerationMode.EFCore7)
                    && discoverMultipleResultSets)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Dapper",
                        Version = "2.0.123",
                        DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                        IsMainProviderPackage = false,
                        UseMethodName = null,
                    });
                }
            }

            if (databaseType == DatabaseType.SQLite)
            {
                var pkgVersion = "7.0.5";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.16";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Microsoft.EntityFrameworkCore.Sqlite",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Sqlite",
                });

                if (useNodaTime)
                {
                    pkgVersion = "7.0.0";
                    switch (codeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "6.0.0";
                            break;
                    }

                    packages.Add(new NuGetPackage
                    {
                        PackageId = "EntityFrameworkCore.Sqlite.NodaTime",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { databaseType },
                        IsMainProviderPackage = false,
                        UseMethodName = "NodaTime",
                    });
                }
            }

            if (databaseType == DatabaseType.Npgsql)
            {
                var pkgVersion = "7.0.3";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.8";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Npgsql.EntityFrameworkCore.PostgreSQL",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Npgsql",
                });

                if (useSpatial)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { databaseType },
                        IsMainProviderPackage = false,
                        UseMethodName = "NetTopologySuite",
                    });
                }

                if (useNodaTime)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Npgsql.EntityFrameworkCore.PostgreSQL.NodaTime",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { databaseType },
                        IsMainProviderPackage = false,
                        UseMethodName = "NodaTime",
                    });
                }
            }

            if (databaseType == DatabaseType.Mysql)
            {
                var pkgVersion = "7.0.0";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.2";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Pomelo.EntityFrameworkCore.MySql",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Mysql",
                });

                if (useSpatial)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Pomelo.EntityFrameworkCore.MySql.NetTopologySuite",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { databaseType },
                        IsMainProviderPackage = false,
                        UseMethodName = "NetTopologySuite",
                    });
                }
            }

            if (databaseType == DatabaseType.Oracle)
            {
                var pkgVersion = "7.21.9";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.21.90";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Oracle.EntityFrameworkCore",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Oracle",
                });
            }

            if (databaseType == DatabaseType.Firebird && codeGenerationMode == CodeGenerationMode.EFCore6)
            {
                var pkgVersion = "9.1.1";

                packages.Add(new NuGetPackage
                {
                    PackageId = "FirebirdSql.EntityFrameworkCore.Firebird",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Firebird",
                });
            }

            return packages;
        }

        private static string GetReadMeText(string provider, ReverseEngineerCommandOptions options, string content, List<NuGetPackage> packages)
        {
            var extraPackages = packages.Where(p => !p.IsMainProviderPackage && p.UseMethodName != null)
                .Select(p => $"Use{p.UseMethodName}()").ToList();

            var useText = string.Empty;

            if (extraPackages.Count > 0)
            {
                useText = "," + Environment.NewLine + "           x => x." + string.Join(".", extraPackages);
            }

            var packageText = new StringBuilder();

            foreach (var package in packages)
            {
                packageText.AppendLine($"  <PackageReference Include=\"{package.PackageId}\" Version=\"{package.Version}\" />");
            }

            GetKnownProviders().TryGetValue(provider, out var providerName);

            return content.Replace("[ProviderName]", providerName)
                .Replace("[ConnectionString]", options.ConnectionString.Replace(@"\", @"\\"))
                .Replace("[UseList]", useText)
                .Replace("[PackageList]", packageText.ToString())
                .Replace("[ContextName]", options.ContextClassName);
        }

        private static Dictionary<string, string> GetKnownProviders()
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
                    new List<string> { "mssql", "sqlserver" }
                },
                {
                    "Microsoft.EntityFrameworkCore.Sqlite",
                    new List<string> { "sqlite" }
                },
                {
                    "Npgsql.EntityFrameworkCore.PostgreSQL",
                    new List<string> { "postgres", "postgresql" }
                },
                {
                    "Pomelo.EntityFrameworkCore.MySql",
                    new List<string> { "mysql" }
                },
                {
                    "Oracle.EntityFrameworkCore",
                    new List<string> { "oracle" }
                },
                {
                    "FirebirdSql.EntityFrameworkCore.Firebird",
                    new List<string> { "firebird" }
                },
            };
        }
    }
}
