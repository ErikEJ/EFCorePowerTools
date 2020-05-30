using EFCorePowerTools;
using EFCorePowerTools.Shared.Models;
using ErikEJ.SqlCeScripting;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Data.Core;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using ReverseEngineer20;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace ErikEJ.SqlCeToolbox.Helpers
{
    internal class EnvDteHelper
    {
        internal static Dictionary<string, DatabaseInfo> GetDataConnections(EFCorePowerToolsPackage package)
        {
            // http://www.mztools.com/articles/2007/MZ2007018.aspx
            Dictionary<string, DatabaseInfo> databaseList = new Dictionary<string, DatabaseInfo>();
            var dataExplorerConnectionManager = package.GetService<IVsDataExplorerConnectionManager>();
            Guid provider40 = new Guid(EFCorePowerTools.Shared.Resources.SqlCompact40Provider);
            Guid provider40Private = new Guid(EFCorePowerTools.Shared.Resources.SqlCompact40PrivateProvider);
            Guid providerSqLite = new Guid(EFCorePowerTools.Shared.Resources.SQLiteProvider);
            Guid providerSqlitePrivate = new Guid(EFCorePowerTools.Shared.Resources.SQLitePrivateProvider);
            Guid providerNpgsql = new Guid(Resources.NpgsqlProvider);
            Guid providerMysql = new Guid(Resources.MysqlVSProvider);
            Guid providerOracle = new Guid(Resources.OracleProvider);

            bool isV40Installed = RepositoryHelper.IsV40Installed() &&
                (DdexProviderIsInstalled(provider40) || DdexProviderIsInstalled(provider40Private));
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
                            ConnectionString = sConnectionString
                        };
                        var objProviderGuid = connection.Provider;

                        if ((objProviderGuid == provider40 && isV40Installed ||
                            objProviderGuid == provider40Private && isV40Installed)
                            && !sConnectionString.Contains("Mobile Device"))
                        {
                            info.DatabaseType = DatabaseType.SQLCE40;
                        }
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
            return info;
        }

        internal static string SaveDataConnection(EFCorePowerToolsPackage package, string encryptedConnectionString,
            DatabaseType dbType, Guid provider)
        {
            var dataExplorerConnectionManager = package.GetService<IVsDataExplorerConnectionManager>();
            var savedName = GetFileName(DataProtection.DecryptString(encryptedConnectionString), dbType);
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
                if (providerInvariant == "System.Data.SqlServerCe.4.0")
                {
                    dbType = DatabaseType.SQLCE40;
                    providerGuid = EFCorePowerTools.Shared.Resources.SqlCompact40PrivateProvider;
                }
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

        internal static string GetNpgsqlDatabaseName(string connectionString)
        {
            var pgBuilder = new NpgsqlConnectionStringBuilder(connectionString);
            return pgBuilder.Database;
        }

        internal static List<TableInformationModel> GetNpgsqlTableNames(string connectionString, bool includeViews)
        {
            var result = new List<TableInformationModel>();
            using (var npgsqlConn = new NpgsqlConnection(connectionString))
            {
                npgsqlConn.Open();

                var tablesDataTable = npgsqlConn.GetSchema("Tables");
                var constraints = npgsqlConn.GetSchema("Constraints");
                foreach (DataRow row in tablesDataTable.Rows)
                {
                    var primaryKey = constraints
                        .AsEnumerable()
                        .Where(myRow => myRow.Field<string>("table_name") == row["table_name"].ToString()
                        && myRow.Field<string>("table_schema") == row["table_schema"].ToString()
                        && myRow.Field<string>("constraint_type") == "PRIMARY KEY")
                        .FirstOrDefault();

                    var info = new TableInformationModel(row["table_schema"].ToString() + "." + row["table_name"].ToString(), includeViews ? true : primaryKey != null, includeViews ? primaryKey == null : false);
                    result.Add(info);
                }

                if (includeViews)
                {
                    var viewsDataTable = npgsqlConn.GetSchema("Views");
                    foreach (DataRow row in viewsDataTable.Rows)
                    {
                        var info = new TableInformationModel(row["table_schema"].ToString() + "." + row["table_name"].ToString(), true, true);
                        result.Add(info);
                    }
                }
            }

            return result.OrderBy(l => l.Name).ToList();
        }

        internal static string GetMysqlDatabaseName(string connectionString)
        {
            var myBuilder = new MySqlConnectionStringBuilder(connectionString);
            return myBuilder.Database;
        }

        internal static List<TableInformationModel> GetMysqlTableNames(string connectionString, bool includeViews)
        {
            var result = new List<TableInformationModel>();
            using (var mysqlConn = new MySqlConnection(connectionString))
            {
                mysqlConn.Open();

                var tables = GetMysqlTables(mysqlConn, mysqlConn.Database);
                string schema = mysqlConn.Database;
                if (schema != "information_schema")
                {
                    foreach (string table in tables)
                    {
                        bool hasPrimaryKey = HasMysqlPrimaryKey(schema, table, mysqlConn);
                        var info = new TableInformationModel(table, includeViews ? true : hasPrimaryKey, includeViews ? !hasPrimaryKey : false);
                        result.Add(info);
                    }
                }
            }

            return result.OrderBy(l => l.Name).ToList();
        }

        internal static string GetOracleDatabaseName(string connectionString)
        {
            var myBuilder = new OracleConnectionStringBuilder(connectionString);
            return myBuilder.DataSource;
        }

        internal static List<TableInformationModel> GetOracleTableNames(string connectionString, bool includeViews)
        {
            var result = new List<TableInformationModel>();
            using (var oracleConn = new OracleConnection(connectionString))
            {
                oracleConn.Open();
                string sql = $@"SELECT table_name, owner FROM user_tables ORDER BY owner, table_name";

                using (var cmd = new OracleCommand(sql, oracleConn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new TableInformationModel(reader.GetString(0), true));
                        }
                    }
                }

                return result;
            }
        }

        private static List<string> GetMysqlTables(MySqlConnection mysqlConn, string schema)
        {
            List<string> tables = new List<string>();
            string sql = $@"SHOW TABLE STATUS FROM `{schema}`";

            MySqlCommand cmd = new MySqlCommand(sql, mysqlConn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
            }

            return tables;
        }

        private static bool HasMysqlPrimaryKey(string schemaName, string tableName, MySqlConnection mysqlConn)
        {
            /**
             * A Unique index can unexpectedly be shown as a primary key here:             *
             * https://dev.mysql.com/doc/mysql-infoschema-excerpt/5.6/en/columns-table.html
             * "A UNIQUE index may be displayed as PRI if it cannot contain NULL values and there is no PRIMARY KEY in the table.
             * A UNIQUE index may display as MUL if several columns form a composite UNIQUE index; although the combination of
             * the columns is unique, each column can still hold multiple occurrences of a given value."
             */
            MySqlCommand cmd = mysqlConn.CreateCommand();
            cmd.CommandText = "SELECT EXISTS(SELECT 1 FROM information_schema.columns WHERE table_schema = @schema AND table_name = @table AND column_key = 'PRI') AS HasPrimaryKey";
            cmd.Parameters.AddWithValue("@schema", schemaName);
            cmd.Parameters.AddWithValue("@table", tableName);
            string value = cmd.ExecuteScalar().ToString();

            return value == "1";
        }

        private static string GetFilePath(string connectionString, DatabaseType dbType)
        {
            var helper = RepositoryHelper.CreateEngineHelper(dbType);
            return helper.PathFromConnectionString(connectionString);
        }

        private static string GetFileName(string connectionString, DatabaseType dbType)
        {
            if (dbType == DatabaseType.SQLServer)
            {
                var helper = new SqlServerHelper();
                return helper.PathFromConnectionString(connectionString);
            }
            
            if (dbType == DatabaseType.Npgsql)
                return GetNpgsqlDatabaseName(connectionString);
            
            if (dbType == DatabaseType.Mysql)
                return GetMysqlDatabaseName(connectionString);

            var filePath = GetFilePath(connectionString, dbType);
            return Path.GetFileName(filePath);
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

        internal static string[] GetProjectFilesInSolution(EFCorePowerToolsPackage package)
        {
            IVsSolution sol = package.GetService<IVsSolution>();
            uint numProjects;
            ErrorHandler.ThrowOnFailure(sol.GetProjectFilesInSolution((uint)__VSGETPROJFILESFLAGS.GPFF_SKIPUNLOADEDPROJECTS, 0, null, out numProjects));
            string[] projects = new string[numProjects];
            ErrorHandler.ThrowOnFailure(sol.GetProjectFilesInSolution((uint)__VSGETPROJFILESFLAGS.GPFF_SKIPUNLOADEDPROJECTS, numProjects, projects, out numProjects));
            //GetProjectFilesInSolution also returns solution folders, so we want to do some filtering
            //things that don't exist on disk certainly can't be project files
            return projects.Where(p => !string.IsNullOrEmpty(p) && File.Exists(p)).ToArray();
        }

        // <summary>
        //     Helper method to show an error message within the shell.  This should be used
        //     instead of MessageBox.Show();
        // </summary>
        // <param name="errorText">Text to display.</param>
        public static void ShowError(string errorText)
        {
            ShowMessageBox(
                errorText, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                OLEMSGICON.OLEMSGICON_CRITICAL);
        }

        public static DialogResult ShowMessage(string messageText)
        {
            return ShowMessageBox(messageText, null, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, OLEMSGICON.OLEMSGICON_INFO);
        }

        // <summary>
        //     Helper method to show a message box within the shell.
        // </summary>
        // <param name="messageText">Text to show.</param>
        // <param name="messageButtons">Buttons which should appear in the dialog.</param>
        // <param name="defaultButton">Default button (invoked when user presses return).</param>
        // <param name="messageIcon">Icon (warning, error, informational, etc.) to display</param>
        // <returns>result corresponding to the button clicked by the user.</returns>
        private static void ShowMessageBox(string messageText, OLEMSGBUTTON messageButtons, OLEMSGDEFBUTTON defaultButton, OLEMSGICON messageIcon)
        {
            ShowMessageBox(messageText, null, messageButtons, defaultButton, messageIcon);
        }

        // <summary>
        //     Helper method to show a message box within the shell.
        // </summary>
        // <param name="messageText">Text to show.</param>
        // <param name="f1Keyword">F1-keyword.</param>
        // <param name="messageButtons">Buttons which should appear in the dialog.</param>
        // <param name="defaultButton">Default button (invoked when user presses return).</param>
        // <param name="messageIcon">Icon (warning, error, informational, etc.) to display</param>
        // <returns>result corresponding to the button clicked by the user.</returns>
        private static DialogResult ShowMessageBox(
            string messageText, string f1Keyword, OLEMSGBUTTON messageButtons,
            OLEMSGDEFBUTTON defaultButton, OLEMSGICON messageIcon)
        {
            var result = 0;
            var uiShell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));

            if (uiShell != null)
            {
                var rclsidComp = Guid.Empty;
                uiShell.ShowMessageBox(
                        0, ref rclsidComp, "EF Core Power Tools", messageText, f1Keyword, 0, messageButtons, defaultButton, messageIcon, 0, out result);
            }

            return (DialogResult)result;
        }
    }
}
