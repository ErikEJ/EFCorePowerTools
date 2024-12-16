﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;
using RevEng.Common.Dab;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    internal class ErDiagramHandler
    {
        private readonly EFCorePowerToolsPackage package;
        private readonly ReverseEngineerHelper reverseEngineerHelper;
        private readonly VsDataHelper vsDataHelper;

        public ErDiagramHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
            reverseEngineerHelper = new ReverseEngineerHelper();
            vsDataHelper = new VsDataHelper();
        }

        public async System.Threading.Tasks.Task BuildErDiagramAsync(Project project, string uiHint)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (await VSHelper.IsDebugModeAsync())
                {
                    VSHelper.ShowError(ReverseEngineerLocale.CannotGenerateCodeWhileDebugging);
                    return;
                }

                var optionsPath = Path.Combine(Path.GetTempPath(), "erdiagram-options.json");

                var options = new DataApiBuilderOptions();

                options.ProjectPath = Path.GetDirectoryName(project.FullPath);

                DatabaseConnectionModel dbInfo = null;

                if (!await ChooseDataBaseConnectionAsync(options, uiHint))
                {
                    await VS.StatusBar.ClearAsync();
                    return;
                }

                await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.GettingReadyToConnect);

                dbInfo = await GetDatabaseInfoAsync(options);

                if (dbInfo == null)
                {
                    await VS.StatusBar.ClearAsync();
                    return;
                }

                await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.LoadingDatabaseObjects);

                if (!await LoadDataBaseObjectsAsync(options, dbInfo))
                {
                    await VS.StatusBar.ClearAsync();
                    return;
                }

                SaveOptions(optionsPath, options);

                await GenerateFilesAsync(optionsPath, dbInfo.ConnectionString);

                options.ConnectionString = null;

                Telemetry.TrackEvent("PowerTools.ErDiagramBuild");
            }
            catch (AggregateException ae)
            {
                foreach (var innerException in ae.Flatten().InnerExceptions)
                {
                    package.LogError(new List<string>(), innerException);
                }
            }
            catch (Exception exception)
            {
                package.LogError(new List<string>(), exception);
            }
        }

        private async Task<bool> ChooseDataBaseConnectionAsync(DataApiBuilderOptions options, string uiHint)
        {
            var databaseList = await vsDataHelper.GetDataConnectionsAsync(package);

            databaseList = databaseList.Where(databaseList => Providers.GetDabProviders().Contains(databaseList.Value.DatabaseType))
                .ToDictionary(databaseList => databaseList.Key, databaseList => databaseList.Value);

            var dacpacList = await SqlProjHelper.GetDacpacProjectsInActiveSolutionAsync();

            var psd = package.GetView<IPickServerDatabaseDialog>();

            if (databaseList.Any())
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

            psd.PublishCodeGenerationMode(CodeGenerationMode.EFCore6, new List<CodeGenerationItem>
            {
                new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore8, Value = "DAB" },
            });

            psd.PublishUiHint(uiHint);

            psd.PublishSchemas(new List<SchemaInfo>());

            var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
            if (!pickDataSourceResult.ClosedByOK)
            {
                return false;
            }

            options.Dacpac = pickDataSourceResult.Payload.Connection?.FilePath;

            if (pickDataSourceResult.Payload.Connection != null)
            {
                options.ConnectionString = pickDataSourceResult.Payload.Connection.ConnectionString;
                options.DatabaseType = pickDataSourceResult.Payload.Connection.DatabaseType;
            }

            return true;
        }

        private async Task<DatabaseConnectionModel> GetDatabaseInfoAsync(DataApiBuilderOptions options)
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

        private async Task<bool> LoadDataBaseObjectsAsync(DataApiBuilderOptions options, DatabaseConnectionModel dbInfo)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IEnumerable<TableModel> predefinedTables = null;

            try
            {
                await VS.StatusBar.StartAnimationAsync(StatusAnimation.Build);

                predefinedTables = !string.IsNullOrEmpty(options.Dacpac)
                                           ? await GetDacpacTablesAsync(options.Dacpac, CodeGenerationMode.EFCore8)
                                           : await GetTablesAsync(dbInfo, CodeGenerationMode.EFCore8);

                predefinedTables = predefinedTables
                    .Where(t => t.ObjectType == ObjectType.Table
                    || t.ObjectType == ObjectType.View);
            }
            catch (InvalidOperationException ex)
            {
                VSHelper.ShowError($"{ex.Message}");
                return false;
            }
            finally
            {
                await VS.StatusBar.EndAnimationAsync(StatusAnimation.Build);
            }

            var preselectedTables = new List<SerializationTableModel>();

            if (options.Tables?.Count > 0)
            {
                var normalizedTables = reverseEngineerHelper.NormalizeTables(options.Tables, dbInfo.DatabaseType == DatabaseType.SQLServer);
                preselectedTables.AddRange(normalizedTables);
            }

            await VS.StatusBar.ClearAsync();

            var ptd = package.GetView<IPickTablesDialog>()
                              .AddTables(predefinedTables, new List<Schema>())
                              .PreselectTables(preselectedTables)
                              .SqliteToolboxInstall(true);

            var pickTablesResult = ptd.ShowAndAwaitUserResponse(true);

            options.Tables = pickTablesResult.Payload.Objects.ToList();
            return pickTablesResult.ClosedByOK;
        }

        private async System.Threading.Tasks.Task GenerateFilesAsync(string optionsPath, string connectionString)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 1, 3);

            var stopWatch = Stopwatch.StartNew();

            await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 2, 3);

            var revEngRunner = new EfRevEngLauncher(new ReverseEngineerCommandOptions(), CodeGenerationMode.EFCore8);
            var cmdPath = await revEngRunner.GetErDiagramAsync(optionsPath, connectionString);

            await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 3, 3);

            stopWatch.Stop();

            if (File.Exists(cmdPath))
            {
                await VS.Documents.OpenAsync(cmdPath);
            }

            await VS.StatusBar.ShowMessageAsync(string.Format(ReverseEngineerLocale.ReverseEngineerCompleted, stopWatch.Elapsed.ToString(@"mm\:ss")));

            Telemetry.TrackFrameworkUse(nameof(ErDiagramHandler), CodeGenerationMode.EFCore8);
        }

        private void SaveOptions(string optionsPath, DataApiBuilderOptions options)
        {
            File.WriteAllText(optionsPath, options.Write(), Encoding.UTF8);
        }

        private async Task<List<TableModel>> GetDacpacTablesAsync(string dacpacPath, CodeGenerationMode codeGenerationMode)
        {
            var builder = new TableListBuilder(dacpacPath, DatabaseType.SQLServerDacpac, null);

            return await builder.GetTableDefinitionsAsync(codeGenerationMode);
        }

        private async Task<List<TableModel>> GetTablesAsync(DatabaseConnectionModel dbInfo, CodeGenerationMode codeGenerationMode)
        {
            if (dbInfo.DataConnection != null)
            {
                dbInfo.DataConnection.Open();
                dbInfo.ConnectionString = DataProtection.DecryptString(dbInfo.DataConnection.EncryptedConnectionString);
            }

            var builder = new TableListBuilder(dbInfo.ConnectionString, dbInfo.DatabaseType, null);
            return await builder.GetTableDefinitionsAsync(codeGenerationMode);
        }
    }
}
