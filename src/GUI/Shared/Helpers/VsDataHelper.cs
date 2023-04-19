// ReSharper disable once CheckNamespace
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using Microsoft.VisualStudio.Data.Core;
using Microsoft.VisualStudio.Data.Services;
using RevEng.Common;

namespace EFCorePowerTools.Helpers
{
    internal class VsDataHelper
    {
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

        public static string GetSavedConnectionName(string connectionString, DatabaseType dbType)
        {
            if (dbType == DatabaseType.SQLServer && !connectionString.Contains(";Authentication="))
            {
                return PathFromConnectionString(connectionString);
            }

            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connectionString;

            var result = string.Empty;

            if (builder.TryGetValue("Data Source", out object dataSource))
            {
                result += dataSource.ToString().Replace(".database.windows.net", string.Empty);
            }

            if (builder.TryGetValue("DataSource", out object dataSource2))
            {
                result += dataSource2.ToString();
            }

            if (builder.TryGetValue("Database", out object database))
            {
                result += "." + database.ToString();
            }

            if (builder.TryGetValue("Initial Catalog", out object catalog))
            {
                result += "." + catalog.ToString();
            }

            return result;
        }

        internal static async Task<DatabaseConnectionModel> PromptForInfoAsync()
        {
            // Show dialog with modern SqlClient selected by default
            var dialogFactory = await VS.GetServiceAsync<IVsDataConnectionDialogFactory, IVsDataConnectionDialogFactory>();
            var dialog = dialogFactory.CreateConnectionDialog();
            dialog.AddAllSources();

            dialog.SelectedSource = new Guid("067ea0d9-ba62-43f7-9106-34930c60c528");
            dialog.SelectedProvider = await IsDdexProviderInstalledAsync(new Guid(Resources.MicrosoftSqlServerDotNetProvider))
                ? new Guid(Resources.MicrosoftSqlServerDotNetProvider)
                : new Guid(Resources.SqlServerDotNetProvider);

            var dialogResult = dialog.ShowDialog(connect: true);

            if (dialogResult == null)
            {
                return new DatabaseConnectionModel { DatabaseType = DatabaseType.Undefined };
            }

            var info = await GetDatabaseInfoAsync(dialogResult.Provider, DataProtection.DecryptString(dialog.EncryptedConnectionString));
            if (info.ConnectionName == Guid.Empty.ToString())
            {
                return new DatabaseConnectionModel { DatabaseType = DatabaseType.Undefined };
            }

            var savedName = await SaveDataConnectionAsync(dialog.EncryptedConnectionString, info.DatabaseType, new Guid(info.ConnectionName));
            info.ConnectionName = savedName;
            info.DataConnection = dialogResult;
            return info;
        }

        internal static string PromptForDacpac()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "SQL Server Database Project|*.dacpac",
                CheckFileExists = true,
                Multiselect = false,
                ValidateNames = true,
                Title = "Select .dacpac File",
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            return ofd.FileName;
        }

        internal static async Task<string> SaveDataConnectionAsync(
            string encryptedConnectionString,
            DatabaseType dbType,
            Guid provider)
        {
            var dataExplorerConnectionManager = await VS.GetServiceAsync<IVsDataExplorerConnectionManager, IVsDataExplorerConnectionManager>();
            var savedName = GetSavedConnectionName(DataProtection.DecryptString(encryptedConnectionString), dbType);
            dataExplorerConnectionManager.AddConnection(savedName, provider, encryptedConnectionString, true);
            return savedName;
        }

        internal static async Task RemoveDataConnectionAsync(IVsDataConnection dataConnection)
        {
            var dataExplorerConnectionManager = await VS.GetServiceAsync<IVsDataExplorerConnectionManager, IVsDataExplorerConnectionManager>();

            foreach (var connection in dataExplorerConnectionManager.Connections.Values)
            {
                if (connection.Connection == dataConnection)
                {
                    dataExplorerConnectionManager.RemoveConnection(connection);
                }
            }
        }

        internal static async System.Threading.Tasks.Task<bool> IsDdexProviderInstalledAsync(Guid id)
        {
            try
            {
                var providerManager = await VS.GetServiceAsync<IVsDataProviderManager, IVsDataProviderManager>();
                return providerManager != null &&
                    providerManager.Providers.TryGetValue(id, out IVsDataProvider provider);
            }
            catch
            {
                // Ignored
            }

            return false;
        }

        internal async Task<Dictionary<string, DatabaseConnectionModel>> GetDataConnectionsAsync(EFCorePowerToolsPackage package)
        {
            var credentialStore = new CredentialStore();

            // http://www.mztools.com/articles/2007/MZ2007018.aspx
            Dictionary<string, DatabaseConnectionModel> databaseList = new Dictionary<string, DatabaseConnectionModel>();
            var dataExplorerConnectionManager = await VS.GetServiceAsync<IVsDataExplorerConnectionManager, IVsDataExplorerConnectionManager>();
            Guid providerSQLite = new Guid(Resources.SQLiteProvider);
            Guid providerMicrosoftSQLite = new Guid(Resources.MicrosoftSQLiteProvider);
            Guid providerSQLitePrivate = new Guid(Resources.SQLitePrivateProvider);
            Guid providerNpgsql = new Guid(Resources.NpgsqlProvider);
            Guid providerMysql = new Guid(Resources.MysqlVSProvider);
            Guid providerOracle = new Guid(Resources.OracleProvider);

            try
            {
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
                                DatabaseType = DatabaseType.Undefined,
                                ConnectionString = sConnectionString,
                                DataConnection = connection.Connection,
                            };

                            var objProviderGuid = connection.Provider;

                            if (objProviderGuid == providerSQLite
                                || objProviderGuid == providerMicrosoftSQLite
                                || objProviderGuid == providerSQLitePrivate)
                            {
                                info.DatabaseType = DatabaseType.SQLite;
                            }

                            if (objProviderGuid == new Guid(Resources.SqlServerDotNetProvider)
                                || objProviderGuid == new Guid(Resources.MicrosoftSqlServerDotNetProvider))
                            {
                                info.DatabaseType = DatabaseType.SQLServer;
                            }

                            if (objProviderGuid == providerNpgsql)
                            {
                                info.DatabaseType = DatabaseType.Npgsql;
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

                            if (info.DatabaseType != DatabaseType.Undefined
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
            }
            catch (Exception ex)
            {
                package.LogError(new List<string> { ex.Message }, ex);
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

        private static async Task<DatabaseConnectionModel> GetDatabaseInfoAsync(Guid provider, string connectionString)
        {
            var dbType = DatabaseType.Undefined;
            var providerGuid = Guid.Empty.ToString();

            // Find provider
            var providerManager = await VS.GetServiceAsync<IVsDataProviderManager, IVsDataProviderManager>();
            IVsDataProvider dp;
            providerManager.Providers.TryGetValue(provider, out dp);
            if (dp != null)
            {
                var providerInvariant = (string)dp.GetProperty("InvariantName");
                dbType = DatabaseType.Undefined;

                if (providerInvariant == "System.Data.SQLite.EF6")
                {
                    dbType = DatabaseType.SQLite;
                    providerGuid = Resources.SQLitePrivateProvider;
                }

                if (providerInvariant == "Microsoft.Data.Sqlite")
                {
                    dbType = DatabaseType.SQLite;
                    providerGuid = Resources.MicrosoftSQLiteProvider;
                }

                if (providerInvariant == "System.Data.SqlClient")
                {
                    dbType = DatabaseType.SQLServer;
                    providerGuid = Resources.SqlServerDotNetProvider;
                }

                if (providerInvariant == "Microsoft.Data.SqlClient")
                {
                    dbType = DatabaseType.SQLServer;
                    providerGuid = Resources.MicrosoftSqlServerDotNetProvider;
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

        private static string PathFromConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilderHelper().GetBuilder(connectionString);

            var database = builder.InitialCatalog;
            if (string.IsNullOrEmpty(database) && !string.IsNullOrEmpty(builder.AttachDBFilename))
            {
                return Path.GetFileName(builder.AttachDBFilename);
            }

            if (builder.DataSource.StartsWith("(localdb)", StringComparison.OrdinalIgnoreCase))
            {
                return builder.DataSource + "." + database;
            }
            else
            {
                using (var cmd = new SqlCommand())
                {
                    using (var conn = new SqlConnection(builder.ConnectionString))
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT LOWER(@@servername) + '.' + DB_NAME() + '.' + SCHEMA_NAME()";
                        conn.Open();

                        object res = cmd.ExecuteScalar();

                        if (res != null)
                        {
                            return (string)res;
                        }

                        return builder.DataSource + "." + database;
                    }
                }
            }
        }
    }
}
