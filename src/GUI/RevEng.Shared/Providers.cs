using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RevEng.Common
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

        public static HashSet<DatabaseType> GetDabProviders()
            {
            return new HashSet<DatabaseType>
            {
                DatabaseType.SQLServer,
                DatabaseType.SQLServerDacpac,
                DatabaseType.Npgsql,
                DatabaseType.Mysql,
            };
        }

        public static string CreateReadme(ReverseEngineerCommandOptions commandOptions, CodeGenerationMode codeGenerationMode, string redactedConnectionString)
        {
            if (commandOptions == null)
            {
                throw new ArgumentNullException(nameof(commandOptions));
            }

            if (string.IsNullOrEmpty(redactedConnectionString))
            {
                throw new ArgumentNullException(nameof(redactedConnectionString));
            }

            var packages = GetNeededPackages(
                commandOptions.DatabaseType,
                commandOptions.UseSpatial,
                commandOptions.UseNodaTime,
                commandOptions.UseDateOnlyTimeOnly,
                commandOptions.UseHierarchyId,
                commandOptions.UseMultipleSprocResultSets,
                commandOptions.Tables?.Exists(t => t.ObjectType == ObjectType.Procedure) ?? false,
                codeGenerationMode);

            var readmeName = "efcpt-readme.md";

            var template = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), readmeName), Encoding.UTF8);

            var finalText = GetReadMeText(commandOptions, template, packages, redactedConnectionString);

            var readmePath = Path.Combine(commandOptions.ProjectPath, readmeName);

            File.WriteAllText(readmePath, finalText, Encoding.UTF8);

            return readmePath;
        }

        public static List<NuGetPackage> GetNeededPackages(DatabaseType databaseType, bool useSpatial, bool useNodaTime, bool useDateOnlyTimeOnly, bool useHierarchyId, bool discoverMultipleResultSets, bool hasProcedures, CodeGenerationMode codeGenerationMode)
        {
            // Update versions here when adding provider and other updates
            var packages = new List<NuGetPackage>();

            if (databaseType == DatabaseType.SQLServer || databaseType == DatabaseType.SQLServerDacpac)
            {
                var pkgVersion = "7.0.20";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.33";
                        break;
                    case CodeGenerationMode.EFCore8:
                        pkgVersion = "8.0.8";
                        break;
                    case CodeGenerationMode.EFCore9:
                        pkgVersion = "9.0.0-rc.1.24451.1";
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
                    pkgVersion = "7.1.0";
                    switch (codeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "6.0.1";
                            break;

                        case CodeGenerationMode.EFCore8:
                            pkgVersion = "8.0.1";
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
                        case CodeGenerationMode.EFCore8:
                            pkgVersion = "8.0.0";
                            break;
                    }

                    if (codeGenerationMode == CodeGenerationMode.EFCore8
                        || codeGenerationMode == CodeGenerationMode.EFCore9)
                    {
                        packages.Add(new NuGetPackage
                        {
                            PackageId = "Microsoft.EntityFrameworkCore.SqlServer.HierarchyId",
                            Version = pkgVersion,
                            DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                            IsMainProviderPackage = false,
                            UseMethodName = "HierarchyId",
                        });
                    }
                    else
                    {
                        packages.Add(new NuGetPackage
                        {
                            PackageId = "EntityFrameworkCore.SqlServer.HierarchyId",
                            Version = pkgVersion,
                            DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                            IsMainProviderPackage = false,
                            UseMethodName = "HierarchyId",
                        });
                    }
                }

                if (useDateOnlyTimeOnly)
                {
                    pkgVersion = "7.0.10";
                    switch (codeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "6.0.10";
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

                if (hasProcedures && discoverMultipleResultSets)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Dapper",
                        Version = "2.1.35",
                        DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                        IsMainProviderPackage = false,
                        UseMethodName = null,
                    });
                }
            }

            if (databaseType == DatabaseType.SQLite)
            {
                var pkgVersion = "7.0.20";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.33";
                        break;
                    case CodeGenerationMode.EFCore8:
                        pkgVersion = "8.0.8";
                        break;
                    case CodeGenerationMode.EFCore9:
                        pkgVersion = "9.0.0-rc.1.24451.1";
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

                        case CodeGenerationMode.EFCore8:
                            pkgVersion = "8.0.0";
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
                var pkgVersion = "7.0.11";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.29";
                        break;
                    case CodeGenerationMode.EFCore8:
                        pkgVersion = "8.0.4";
                        break;
                    case CodeGenerationMode.EFCore9:
                        pkgVersion = "9.0.0-rc.1";
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
                        pkgVersion = "6.0.3";
                        break;

                    case CodeGenerationMode.EFCore8:
                        pkgVersion = "8.0.1";
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
                var pkgVersion = "7.21.13";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.21.150";
                        break;
                    case CodeGenerationMode.EFCore8:
                        pkgVersion = "8.23.50";
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

            if (databaseType == DatabaseType.Firebird)
            {
                var pkgVersion = "10.0.0";
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "9.1.1";
                        break;

                    case CodeGenerationMode.EFCore8:
                        pkgVersion = "11.0.0";
                        break;
                }

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

        private static string GetReadMeText(ReverseEngineerCommandOptions options, string content, List<NuGetPackage> packages, string redactedConnectionString)
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

            var mainPackage = packages.Find(p => p.IsMainProviderPackage);

            return content.Replace("[ProviderName]", mainPackage?.UseMethodName ?? string.Empty)
                .Replace("[ConnectionString]", redactedConnectionString.Replace(@"\", @"\\"))
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
