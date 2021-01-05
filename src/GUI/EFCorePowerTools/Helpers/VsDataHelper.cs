using EFCorePowerTools;
using ErikEJ.SqlCeScripting;
using Microsoft.VisualStudio.Data.Core;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using ReverseEngineer20;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace EFCorePowerTools.Helpers
{
    internal class VsDataHelper
    {
        internal static Dictionary<string, DatabaseInfo> GetDataConnections(EFCorePowerToolsPackage package)
        {
            // http://www.mztools.com/articles/2007/MZ2007018.aspx
            Dictionary<string, DatabaseInfo> databaseList = new Dictionary<string, DatabaseInfo>();
            var dataExplorerConnectionManager = package.GetService<IVsDataExplorerConnectionManager>();
            Guid providerSqLite = new Guid(EFCorePowerTools.Shared.Resources.SQLiteProvider);
            Guid providerSqlitePrivate = new Guid(EFCorePowerTools.Shared.Resources.SQLitePrivateProvider);
            Guid providerNpgsql = new Guid(Resources.NpgsqlProvider);
            Guid providerMysql = new Guid(Resources.MysqlVSProvider);
            Guid providerOracle = new Guid(Resources.OracleProvider);

            if (dataExplorerConnectionManager != null)
            {
                foreach (var connection in dataExplorerConnectionManager.Connections.Values)
                {
                    try
                    {
                        var sConnectionString = DataProtection.DecryptString(connection.EncryptedConnectionString);
                        var info = new DatabaseInfo()
                        {
                            Caption = connection.DisplayName,
                            FromServerExplorer = true,
                            DatabaseType = DatabaseType.SQLCE35,
                            ConnectionString = sConnectionString,
                            DataConnection = connection.Connection,
                        };

                        var objProviderGuid = connection.Provider;

                        if (objProviderGuid == providerSqLite
                            || objProviderGuid == providerSqlitePrivate)
                        {
                            info.DatabaseType = DatabaseType.SQLite;
                        }
                        if (objProviderGuid == new Guid(Resources.SqlServerDotNetProvider)
                            || objProviderGuid == providerNpgsql)
                        {
                            info.DatabaseType = objProviderGuid == providerNpgsql ? DatabaseType.Npgsql : DatabaseType.SQLServer;
                        }

                        // This provider depends on https://dev.mysql.com/downloads/windows/visualstudio/
                        if (objProviderGuid == providerMysql)
                        {
                            info.DatabaseType = DatabaseType.Mysql;
                        }

                        if (objProviderGuid == providerOracle)
                        {
                            info.DatabaseType = DatabaseType.Oracle;
                        }

                        if (info.DatabaseType != DatabaseType.SQLCE35
                            && !databaseList.ContainsKey(sConnectionString))
                        {
                            databaseList.Add(sConnectionString, info);
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
            }
            return databaseList;
        }

        internal static bool DdexProviderIsInstalled(Guid id)
        {
            try
            {
                var objIVsDataProviderManager =
                    Package.GetGlobalService(typeof(IVsDataProviderManager)) as IVsDataProviderManager;
                return objIVsDataProviderManager != null &&
                    objIVsDataProviderManager.Providers.TryGetValue(id, out IVsDataProvider _);
            }
            catch
            {
                //Ignored
            }
            return false;
        }

        internal static DatabaseInfo PromptForInfo(EFCorePowerToolsPackage package)
        {
            // Show dialog with SqlClient selected by default
            var dialogFactory = package.GetService<IVsDataConnectionDialogFactory>();
            var dialog = dialogFactory.CreateConnectionDialog();
            dialog.AddAllSources();
            dialog.SelectedSource = new Guid("067ea0d9-ba62-43f7-9106-34930c60c528");
            var dialogResult = dialog.ShowDialog(connect: true);

            if (dialogResult == null) return new DatabaseInfo {DatabaseType = DatabaseType.Undefined};

            var info = GetDatabaseInfo(package, dialogResult.Provider, DataProtection.DecryptString(dialog.EncryptedConnectionString));
            if (info.Size == Guid.Empty.ToString()) return new DatabaseInfo { DatabaseType = DatabaseType.Undefined };

            var savedName = SaveDataConnection(package, dialog.EncryptedConnectionString, info.DatabaseType, new Guid(info.Size));
            info.Caption = savedName;
            info.DataConnection = dialogResult;
            return info;
        }

        internal static string PromptForDacpac()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "SQL Server Database Project|*.dacpac";
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;
            ofd.ValidateNames = true;
            ofd.Title = "Select .dacpac File";
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return ofd.FileName;
        }

        internal static string SaveDataConnection(EFCorePowerToolsPackage package, string encryptedConnectionString,
            DatabaseType dbType, Guid provider)
        {
            var dataExplorerConnectionManager = package.GetService<IVsDataExplorerConnectionManager>();
            var savedName = GetSavedConnectionName(DataProtection.DecryptString(encryptedConnectionString), dbType);
            dataExplorerConnectionManager.AddConnection(savedName, provider, encryptedConnectionString, true);
            return savedName;
        }

        private static DatabaseInfo GetDatabaseInfo(EFCorePowerToolsPackage package, Guid provider, string connectionString)
        {
            var dbType = DatabaseType.SQLCE35;
            var providerInvariant = "N/A";
            var providerGuid = Guid.Empty.ToString();
            // Find provider
            var providerManager = package.GetService<IVsDataProviderManager>();
            IVsDataProvider dp;
            providerManager.Providers.TryGetValue(provider, out dp);
            if (dp != null)
            {
                providerInvariant = (string)dp.GetProperty("InvariantName");
                dbType = DatabaseType.SQLCE35;

                if (providerInvariant == "System.Data.SQLite.EF6")
                {
                    dbType = DatabaseType.SQLite;
                    providerGuid = EFCorePowerTools.Shared.Resources.SQLitePrivateProvider;
                }

                if (providerInvariant == "System.Data.SqlClient")
                {
                    dbType = DatabaseType.SQLServer;
                    providerGuid = Resources.SqlServerDotNetProvider;
                }

                if (providerInvariant == "Npgsql")
                {
                    dbType = DatabaseType.Npgsql;
                    providerGuid = Resources.NpgsqlProvider;
                }

                if (providerInvariant == "Oracle.ManagedDataAccess.Client")
                {
                    dbType = DatabaseType.Oracle;
                    providerGuid = Resources.OracleProvider;
                }

                if (providerInvariant == "Mysql" || providerInvariant == "MySql.Data.MySqlClient")
                {
                    dbType = DatabaseType.Mysql;
                    providerGuid = Resources.MysqlVSProvider;
                }
            }

            return new DatabaseInfo
            {
                DatabaseType = dbType,
                ConnectionString = connectionString,
                ServerVersion = providerInvariant,
                Size = providerGuid
            };
        }

        public static string GetSavedConnectionName(string connectionString, DatabaseType dbType)
        {
            if (dbType == DatabaseType.SQLServer)
            {
                var helper = new SqlServerHelper();
                return helper.PathFromConnectionString(connectionString);
            }

            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connectionString;

            var result = string.Empty;

            if (builder.TryGetValue("Data Source", out object dataSource))
            {
                result += dataSource.ToString();
            }

            if (builder.TryGetValue("DataSource", out object dataSource2))
            {
                result +=  dataSource2.ToString();
            }

            if (builder.TryGetValue("Database", out object database))
            {
                result+=  "." + database.ToString();
            }

            return result;
        }

        public static string GetDatabaseName(string connectionString, DatabaseType dbType)
        {
            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connectionString;

            if (builder.TryGetValue("Initial Catalog", out object catalog))
            {
                return catalog.ToString();
            }

            if (builder.TryGetValue("Database", out object database))
            {
                return database.ToString();
            }

            if (builder.TryGetValue("Data Source", out object dataSource))
            {
                return dataSource.ToString();
            }

            if (builder.TryGetValue("DataSource", out object dataSource2))
            {
                return dataSource2.ToString();
            }
            return dbType.ToString();
        }

        internal static bool IsSqLiteDbProviderInstalled()
        {
            try
            {
                System.Data.Common.DbProviderFactories.GetFactory("System.Data.SQLite.EF6");
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
