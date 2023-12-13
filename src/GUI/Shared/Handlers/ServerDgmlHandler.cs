using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
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
                var info = await ChooseDataBaseConnectionAsync();

                if (info.DatabaseModel == null)
                {
                    return;
                }

                var connectionString = info.DatabaseModel.ConnectionString;

                if (info.DatabaseModel.DatabaseType == DatabaseType.SQLServerDacpac)
                {
                    connectionString = await SqlProjHelper.BuildSqlProjAsync(info.DatabaseModel.FilePath);
                }

                if (info.DatabaseModel.DataConnection != null)
                {
                    info.DatabaseModel.DataConnection.Open();
                    connectionString = DataProtection.DecryptString(info.DatabaseModel.DataConnection.EncryptedConnectionString);
                }

                var dgmlPath = await GetDgmlAsync(connectionString, info.DatabaseModel.DatabaseType, info.Schemas);

                await ShowDgmlAsync(dgmlPath);

                Telemetry.TrackEvent("PowerTools.GenerateServerDgml");
            }
            catch (Exception exception)
            {
                package.LogError(new List<string>(), exception);
            }
        }

        private async Task<(DatabaseConnectionModel DatabaseModel, SchemaInfo[] Schemas)> ChooseDataBaseConnectionAsync()
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

            psd.PublishCodeGenerationMode(CodeGenerationMode.EFCore6, new List<CodeGenerationItem>());

            var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
            if (!pickDataSourceResult.ClosedByOK)
            {
                return (null, null);
            }

            return (pickDataSourceResult.Payload.Connection, pickDataSourceResult.Payload.Schemas);
        }

        private async Task<string> GetDgmlAsync(string connectionString, DatabaseType databaseType, SchemaInfo[] schemas)
        {
            var schemaList = Enumerable.Empty<string>().ToList();
            if (schemas != null)
            {
                schemaList = schemas.Select(s => s.Name).ToList();
            }

            var launcher = new EfRevEngLauncher(null, CodeGenerationMode.EFCore6);
            return await launcher.GetDgmlAsync(connectionString, databaseType, schemaList);
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
