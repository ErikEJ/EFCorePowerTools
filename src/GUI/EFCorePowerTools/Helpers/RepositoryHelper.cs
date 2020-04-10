using EFCorePowerTools;
using EFCorePowerTools.Shared.Models;
using ErikEJ.SqlCeScripting;
using ErikEJ.SQLiteScripting;
using ReverseEngineer20;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ErikEJ.SqlCeToolbox.Helpers
{
    internal static class RepositoryHelper
    {
        //TODO Update this when SQLite provider is updated!
        public static string SqliteEngineVersion = "3.28";

        public static List<TableInformationModel> GetTablesFromRepository(DatabaseInfo dbInfo, bool includeViews = false)
        {
            if (dbInfo.DatabaseType == DatabaseType.Npgsql)
            {
                return EnvDteHelper.GetNpgsqlTableNames(dbInfo.ConnectionString, includeViews);
            }

            if (dbInfo.DatabaseType == DatabaseType.Mysql)
            {
                return EnvDteHelper.GetMysqlTableNames(dbInfo.ConnectionString, includeViews);
            }

            using (var repository = RepositoryHelper.CreateRepository(dbInfo))
            {
                var allPks = repository.GetAllPrimaryKeys();
                var tableList = repository.GetTableNamesForExclusion();
                var tables = new List<TableInformationModel>();

                foreach (var table in tableList)
                {
                    var hasPrimaryKey = allPks.Any(m => m.TableName == table.TableName);
                    var name = string.IsNullOrEmpty(table.Schema)
                        ? table.TableName
                        : $"[{table.Schema}].[{table.Name}]";

                    var info = new TableInformationModel(name, includeViews ? true : hasPrimaryKey, includeViews ? !hasPrimaryKey : false);
                    tables.Add(info);
                }

                if (includeViews)
                {
                    var views = repository.GetAllViews();
                    foreach (var view in views)
                    {
                        var info = new TableInformationModel(view.ViewName, true, true);
                        tables.Add(info);
                    }
                }

                return tables.OrderBy(l => l.Name).ToList();
            }
        }

        internal static IRepository CreateRepository(DatabaseInfo databaseInfo)
        {
            switch (databaseInfo.DatabaseType)
            {
                case DatabaseType.SQLCE35:
                    return new DBRepository(databaseInfo.ConnectionString);
                case DatabaseType.SQLCE40:
                    return new DB4Repository(databaseInfo.ConnectionString);
                case DatabaseType.SQLServer:
                    return new ServerDBRepository(databaseInfo.ConnectionString, Settings.Default.KeepSchemaNames);
                case DatabaseType.SQLite:
                    return new SQLiteRepository(databaseInfo.ConnectionString);
                default:
                    return null;
            }
        }

        public static ISqlCeHelper CreateEngineHelper(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.SQLCE35:
                    return new SqlCeHelper();
                case DatabaseType.SQLCE40:
                    return new SqlCeHelper4();
                case DatabaseType.SQLServer:
                case DatabaseType.SQLite:
                    return new SqliteHelper();
                default:
                    return null;
            }
        }

        public static IGenerator CreateGenerator(IRepository repository, string outFile, DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.SQLServer:
                    return new Generator(repository, outFile, false, false);
                case DatabaseType.SQLCE40:
                    return string.IsNullOrEmpty(outFile)
                        ? new Generator4(repository)
                        : new Generator4(repository, outFile);
                case DatabaseType.SQLite:
                    return new Generator(repository, outFile, false, false, true);
                default:
                    return null;
            }
        }

        public static string GetClassBasis(string connectionString, DatabaseType dbType)
        {
            string classBasis;
            if (dbType == DatabaseType.SQLServer)
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                classBasis = builder.InitialCatalog;

                if (string.IsNullOrEmpty(classBasis) && !string.IsNullOrEmpty(builder.AttachDBFilename))
                {
                    classBasis = Path.GetFileNameWithoutExtension(builder.AttachDBFilename);
                }
            }
            else
            {
                var path = GetFilePath(connectionString, dbType);
                classBasis = Path.GetFileNameWithoutExtension(path);
            }
            return classBasis;
        }

        private static string GetFilePath(string connectionString, DatabaseType dbType)
        {
            var helper = CreateEngineHelper(dbType);

            if (dbType == DatabaseType.SQLServer)
            {
                return helper.PathFromConnectionString(connectionString);
            }
            return helper.PathFromConnectionString(connectionString);
        }

        public static bool IsMissing(DatabaseInfo info)
        {
            if (info.DatabaseType == DatabaseType.SQLServer)
            {
                return false;
            }
            try
            {
                var path = GetFilePath(info.ConnectionString, info.DatabaseType);
                return !File.Exists(path);
            }
            catch
            {
                return false;
            }
        }

        internal static bool IsV40Installed()
        {
            return new SqlCeHelper4().IsV40Installed() != null;
        }

        internal static bool IsV35Installed()
        {
            return new SqlCeHelper().IsV35Installed() != null;
        }

        internal static bool IsV40DbProviderInstalled()
        {
            return new SqlCeHelper4().IsV40DbProviderInstalled();
        }

        internal static bool IsV35DbProviderInstalled()
        {
            return new SqlCeHelper4().IsV35DbProviderInstalled();
        }
    }
}
