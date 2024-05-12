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
    internal class DatabaseDiagramHandler
    {
        private readonly EFCorePowerToolsPackage package;

        public DatabaseDiagramHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
        }

        public async Task GenerateAsync(string connectionName = null, bool generateErDiagram = false)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                var info = await ChooseDataBaseConnectionAsync(connectionName);

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

                var diagramPath = await GetDiagramAsync(connectionString, info.DatabaseModel.DatabaseType, info.Schemas, generateErDiagram);

                await ShowDiagramAsync(diagramPath);

                Telemetry.TrackEvent("PowerTools.GenerateServerDgml");
            }
            catch (Exception exception)
            {
                package.LogError(new List<string>(), exception);
            }
        }

        private static List<string> GetSchemas(SchemaInfo[] schemas)
        {
            var schemaList = Enumerable.Empty<string>().ToList();
            if (schemas != null)
            {
                schemaList = schemas.Select(s => s.Name).ToList();
            }

            return schemaList;
        }

        private async Task<(DatabaseConnectionModel DatabaseModel, SchemaInfo[] Schemas)> ChooseDataBaseConnectionAsync(string connectionName = null)
        {
            var vsDataHelper = new VsDataHelper();
            var databaseList = await vsDataHelper.GetDataConnectionsAsync(package);
            var dacpacList = await SqlProjHelper.GetDacpacFilesInActiveSolutionAsync();

            if (!string.IsNullOrEmpty(connectionName) && databaseList != null && databaseList.Any())
            {
                var connection = databaseList.FirstOrDefault(m => m.Value.ConnectionName == connectionName);

                if (connection.Value != null)
                {
                    return (connection.Value, new SchemaInfo[] { });
                }
            }

            if (!string.IsNullOrEmpty(connectionName) && dacpacList != null && dacpacList.Any())
            {
                var path = Array.Find(dacpacList, m => m == connectionName);

                if (path != null)
                {
                    return (new DatabaseConnectionModel
                    {
                         FilePath = path,
                         DatabaseType = DatabaseType.SQLServerDacpac,
                    }, new SchemaInfo[] { });
                }
            }

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

            if (!string.IsNullOrEmpty(connectionName))
            {
                psd.PublishUiHint(connectionName);
            }

            var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
            if (!pickDataSourceResult.ClosedByOK)
            {
                return (null, null);
            }

            return (pickDataSourceResult.Payload.Connection, pickDataSourceResult.Payload.Schemas);
        }

        private async Task<string> GetDiagramAsync(string connectionString, DatabaseType databaseType, SchemaInfo[] schemas, bool erDiagram)
        {
            var launcher = new EfRevEngLauncher(null, CodeGenerationMode.EFCore6);
            return await launcher.GetDiagramAsync(connectionString, databaseType, GetSchemas(schemas), erDiagram);
        }

        private async Task ShowDiagramAsync(string path)
        {
            if (File.Exists(path))
            {
                await VS.Documents.OpenInPreviewTabAsync(path);
            }
        }
    }
}
