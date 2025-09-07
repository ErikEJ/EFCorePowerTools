using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;
using RevEng.Common.Dab;

namespace EFCorePowerTools.Handlers
{
    internal class DatabaseDiagramHandler
    {
        private readonly EFCorePowerToolsPackage package;

        public DatabaseDiagramHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
        }

        public async Task GenerateAsync(Project project, string connectionName = null)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (await VSHelper.IsDebugModeAsync())
                {
                    VSHelper.ShowError(ReverseEngineerLocale.CannotGenerateCodeWhileDebugging);
                    return;
                }

                var optionsPath = Path.Combine(Path.GetTempPath(), "diagram-options.json");

                var options = new DataApiBuilderOptions();

                options.ProjectPath = Path.GetDirectoryName(project.FullPath);

                var info = await ChooseDataBaseConnectionAsync(options, connectionName);

                if (info.DatabaseModel == null)
                {
                    await VS.StatusBar.ClearAsync();
                    return;
                }

                await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.GettingReadyToConnect);

                var dbInfo = await GetDatabaseInfoAsync(options);

                if (dbInfo == null)
                {
                    await VS.StatusBar.ClearAsync();
                    return;
                }

                SaveOptions(optionsPath, options);

                var diagramPath = await GetDiagramAsync(optionsPath, dbInfo.ConnectionString, info.Schemas);

                await ShowDiagramAsync(diagramPath);

                Telemetry.TrackEvent("PowerTools.DgmlDiagramBuild");
            }
            catch (AggregateException ae)
            {
                foreach (var innerException in ae.Flatten().InnerExceptions)
                {
                    EFCorePowerToolsPackage.LogError(new List<string>(), innerException);
                }
            }
            catch (Exception exception)
            {
                EFCorePowerToolsPackage.LogError(new List<string>(), exception);
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

        private static async Task<string> GetDiagramAsync(string optionsPath, string connectionString, SchemaInfo[] schemas)
        {
            var launcher = new EfRevEngLauncher(null, CodeGenerationMode.EFCore8);
            return await launcher.GetDiagramAsync(connectionString, optionsPath, GetSchemas(schemas));
        }

        private static async Task ShowDiagramAsync(string path)
        {
            if (File.Exists(path))
            {
                await VS.Documents.OpenInPreviewTabAsync(path);
            }
        }

        private static void SaveOptions(string optionsPath, DataApiBuilderOptions options)
        {
            File.WriteAllText(optionsPath, options.Write(), Encoding.UTF8);
        }

        private static async Task<DatabaseConnectionModel> GetDatabaseInfoAsync(DataApiBuilderOptions options)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dbInfo = new DatabaseConnectionModel();

            if (!string.IsNullOrEmpty(options.ConnectionString))
            {
                dbInfo.ConnectionString = options.ConnectionString;
                dbInfo.DatabaseType = options.DatabaseType;
            }

            if (!string.IsNullOrEmpty(options.Dacpac))
            {
                dbInfo.DatabaseType = DatabaseType.SQLServerDacpac;
                dbInfo.ConnectionString = $"Data Source=(local);Initial Catalog={Path.GetFileNameWithoutExtension(options.Dacpac)};Integrated Security=true;";
                options.ConnectionString = dbInfo.ConnectionString;
                options.DatabaseType = dbInfo.DatabaseType;
                options.MergeDacpacs = AdvancedOptions.Instance.MergeDacpacs;

                options.Dacpac = await SqlProjHelper.BuildSqlProjectAsync(options.Dacpac);
                if (string.IsNullOrEmpty(options.Dacpac))
                {
                    VSHelper.ShowMessage(ReverseEngineerLocale.UnableToBuildSelectedDatabaseProject);
                    return null;
                }
            }

            if (dbInfo.DatabaseType == DatabaseType.Undefined)
            {
                VSHelper.ShowError($"{ReverseEngineerLocale.UnsupportedProvider}");
                return null;
            }

            return dbInfo;
        }

        private async Task<(DatabaseConnectionModel DatabaseModel, SchemaInfo[] Schemas)> ChooseDataBaseConnectionAsync(DataApiBuilderOptions options, string connectionName = null)
        {
            var vsDataHelper = new VsDataHelper();
            var databaseList = await vsDataHelper.GetDataConnectionsAsync(package);
            var dacpacList = await SqlProjHelper.GetDacpacProjectsInActiveSolutionAsync();

            if (!string.IsNullOrEmpty(connectionName) && databaseList != null && databaseList.Any())
            {
                var connection = databaseList.FirstOrDefault(m => m.Value.ConnectionName == connectionName);

                if (connection.Value != null)
                {
                    options.ConnectionString = connection.Value.ConnectionString;
                    options.DatabaseType = connection.Value.DatabaseType;

                    return (connection.Value, new SchemaInfo[] { });
                }
            }

            if (!string.IsNullOrEmpty(connectionName) && dacpacList != null && dacpacList.Any())
            {
                var path = Array.Find(dacpacList, m => m == connectionName);

                if (path != null)
                {
                    options.Dacpac = path;
                    options.ConnectionString = path;
                    options.DatabaseType = DatabaseType.SQLServerDacpac;

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

            psd.PublishCodeGenerationMode(CodeGenerationMode.EFCore8, new List<CodeGenerationItem>());

            if (!string.IsNullOrEmpty(connectionName))
            {
                psd.PublishUiHint(connectionName);
            }

            var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
            if (!pickDataSourceResult.ClosedByOK)
            {
                return (null, null);
            }

            options.Dacpac = pickDataSourceResult.Payload.Connection?.FilePath;

            if (pickDataSourceResult.Payload.Connection != null)
            {
                options.ConnectionString = pickDataSourceResult.Payload.Connection.ConnectionString;
                options.DatabaseType = pickDataSourceResult.Payload.Connection.DatabaseType;
            }

            return (pickDataSourceResult.Payload.Connection, pickDataSourceResult.Payload.Schemas);
        }
    }
}