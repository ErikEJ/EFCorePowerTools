using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.Helpers;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.Handlers
{
    internal class ServerDgmlHandler
    {
        private readonly EFCorePowerToolsPackage package;

        public ServerDgmlHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
        }

        public async Task GenerateAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                var connection = await ChooseDataBaseConnectionAsync();

                if (connection == null)
                {
                    return;
                }

                var connectionString = connection.ConnectionString;

                if (connection.DataConnection != null)
                {
                    connection.DataConnection.Open();
                    connectionString = DataProtection.DecryptString(connection.DataConnection.EncryptedConnectionString);
                }

                var dgmlPath = await GetDgmlAsync(connectionString, connection.DatabaseType);

                await ShowDgmlAsync(dgmlPath);

                Telemetry.TrackEvent("PowerTools.GenerateServerDgml");
            }
            catch (Exception exception)
            {
                package.LogError(new List<string>(), exception);
            }
        }

        private async Task<DatabaseConnectionModel> ChooseDataBaseConnectionAsync()
        {
            var vsDataHelper = new VsDataHelper();
            var databaseList = await vsDataHelper.GetDataConnectionsAsync(package);
            var dacpacList = await SqlProjHelper.GetDacpacFilesInActiveSolutionAsync();

            var psd = package.GetView<IPickServerDatabaseDialog>();

            if (databaseList != null && databaseList.Any())
            {
                psd.PublishConnections(databaseList.Select(m => new DatabaseConnectionModel
                {
                    ConnectionName = m.Value.ConnectionName,
                    ConnectionString = m.Value.ConnectionString,
                    DatabaseType = m.Value.DatabaseType,
                    DataConnection = m.Value.DataConnection,
                }));
            }

            if (dacpacList != null && dacpacList.Any())
            {
                psd.PublishDefinitions(dacpacList.Select(m => new DatabaseConnectionModel
                {
                    FilePath = m,
                    DatabaseType = DatabaseType.SQLServerDacpac,
                }));
            }

            var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
            if (!pickDataSourceResult.ClosedByOK)
            {
                return null;
            }

            return pickDataSourceResult.Payload.Connection;
        }

        private async Task<string> GetDgmlAsync(string connectionString, DatabaseType databaseType)
        {
            var launcher = new EfRevEngLauncher(null, CodeGenerationMode.EFCore6);
            return await launcher.GetDgmlAsync(connectionString, databaseType);
        }

        private async Task ShowDgmlAsync(string path)
        {
            if (File.Exists(path))
            {
                await VS.Documents.OpenInPreviewTabAsync(path);
            }
        }
    }
}
