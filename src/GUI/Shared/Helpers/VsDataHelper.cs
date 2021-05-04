using EFCorePowerTools.Shared.Models;
using Microsoft.VisualStudio.Data.Core;
using Microsoft.VisualStudio.Data.Services;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace EFCorePowerTools.Helpers
{
    internal class VsDataHelper
    {
        internal Dictionary<string, DatabaseConnectionModel> GetDataConnections(EFCorePowerToolsPackage package)
        {
            var credentialStore = new CredentialStore();

            // http://www.mztools.com/articles/2007/MZ2007018.aspx
            Dictionary<string, DatabaseConnectionModel> databaseList = new Dictionary<string, DatabaseConnectionModel>();
            var dataExplorerConnectionManager = package.GetService<IVsDataExplorerConnectionManager>();
            Guid providerSqLite = new Guid(EFCorePowerTools.Shared.Resources.SQLiteProvider);
            Guid providerSqlitePrivate = new Guid(EFCorePowerTools.Shared.Resources.SQLitePrivateProvider);
            Guid providerNpgsql = new Guid(Resources.NpgsqlProvider);
            Guid providerMysql = new Guid(Resources.MysqlVSProvider);
            Guid providerOracle = new Guid(Resources.OracleProvider);

            if (dataExplorerConnectionManager?.Connections?.Values != null)
            {
                foreach (var connection in dataExplorerConnectionManager.Connections.Values)
                {
                    try
                    {
                        var sConnectionString = DataProtection.DecryptString(connection.EncryptedConnectionString);
                        var info = new DatabaseConnectionModel()
                        {
                            ConnectionName = connection.DisplayName,
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
                    catch (Exception ex)
                    {
                        package.LogError(new List<string> { ex.Message }, ex);
                    }
                }
            }

            try
            {
                foreach (var connection in credentialStore.GetStoredDatabaseConnections())
                {
                    databaseList.Add(connection.ConnectionName, connection);
                }
            }
            catch (Exception ex)
            {
                package.LogError(new List<string> { ex.Message }, ex);
            }

            return databaseList;
        }

        internal static DatabaseConnectionModel PromptForInfo(EFCorePowerToolsPackage package)
        {
            // Show dialog with SqlClient selected by default
            var dialogFactory = package.GetService<IVsDataConnectionDialogFactory>();
            var dialog = dialogFactory.CreateConnectionDialog();
            dialog.AddAllSources();
            dialog.SelectedSource = new Guid("067ea0d9-ba62-43f7-9106-34930c60c528");
            var dialogResult = dialog.ShowDialog(connect: true);

            if (dialogResult == null) return new DatabaseConnectionModel {DatabaseType = DatabaseType.Undefined};

            var info = GetDatabaseInfo(package, dialogResult.Provider, DataProtection.DecryptString(dialog.EncryptedConnectionString));
            if (info.ConnectionName == Guid.Empty.ToString()) return new DatabaseConnectionModel { DatabaseType = DatabaseType.Undefined };
            
            var savedName = SaveDataConnection(package, dialog.EncryptedConnectionString, info.DatabaseType, new Guid(info.ConnectionName));
            info.ConnectionName = savedName;
            info.DataConnection = dialogResult;
            return info;
        }

        internal static string PromptForDacpac()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "SQL Server Database Project|*.dacpac|EDMX file|*.edmx",
                CheckFileExists = true,
                Multiselect = false,
                ValidateNames = true,
                Title = "Select .dacpac/.edmx File"
            };
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

        internal static void RemoveDataConnection(EFCorePowerToolsPackage package, IVsDataConnection dataConnection)
        {
            var dataExplorerConnectionManager = package.GetService<IVsDataExplorerConnectionManager>();

            foreach (var connection in dataExplorerConnectionManager.Connections.Values)
            {
                if (connection.Connection == dataConnection)
                {
                    dataExplorerConnectionManager.RemoveConnection(connection);
                }
            }
        }

        private static DatabaseConnectionModel GetDatabaseInfo(EFCorePowerToolsPackage package, Guid provider, string connectionString)
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

            return new DatabaseConnectionModel
            {
                DatabaseType = dbType,
                ConnectionString = connectionString,
                ConnectionName = providerGuid,
            };
        }

        public static string GetSavedConnectionName(string connectionString, DatabaseType dbType)
        {
            if (dbType == DatabaseType.SQLServer)
            {
                return PathFromConnectionString(connectionString);
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

        private static string PathFromConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            var database = builder.InitialCatalog;
            if (string.IsNullOrEmpty(database))
            {
                if (!string.IsNullOrEmpty(builder.AttachDBFilename))
                {
                    database = Path.GetFileName(builder.AttachDBFilename);
                }
            }

            string server;
            if (builder.DataSource.StartsWith("(localdb)", StringComparison.OrdinalIgnoreCase))
            {
                server = builder.DataSource;
            }
            else
            {
                using (var cmd = new SqlCommand(connectionString))
                {
                    using (var conn = new SqlConnection(connectionString))
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT SERVERPROPERTY('ServerName')";
                        conn.Open();
                        server = (string)cmd.ExecuteScalar();
                    }
                }
            }

            return server + "." + database;
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
    }
}
