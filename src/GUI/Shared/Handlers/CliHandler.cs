using System;
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
using Microsoft.VisualStudio.Shell;
using RevEng.Common;
using RevEng.Common.Dab;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    internal class CliHandler
    {
        private readonly EFCorePowerToolsPackage package;
        private readonly VsDataHelper vsDataHelper;

        public CliHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
            vsDataHelper = new VsDataHelper();
        }

        public async Task EditConfigAsync(Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                var optionsPath = Path.Combine(Path.GetDirectoryName(project.FullPath), "efcpt-config.json");
                if (!File.Exists(optionsPath))
                {
                    return;
                }

                await VS.Documents.OpenAsync(optionsPath);

                Telemetry.TrackEvent("PowerTools.CliEdit");
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

        public async Task RunCliAsync(Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (await VSHelper.IsDebugModeAsync())
                {
                    VSHelper.ShowError(ReverseEngineerLocale.CannotGenerateCodeWhileDebugging);
                    return;
                }

                var projectPath = Path.GetDirectoryName(project.FullPath);

                var optionsPath = Path.Combine(projectPath, "efcpt-config.json");

                if (!File.Exists(optionsPath))
                {
                    return;
                }

                var userOptions = ReverseEngineerUserOptionsExtensions.TryRead(optionsPath, projectPath);

                if (userOptions == null)
                {
                    userOptions = new ReverseEngineerUserOptions();
                }

                var options = new DataApiBuilderOptions();

                DatabaseConnectionModel dbInfo = null;

                if (!await ChooseDataBaseConnectionAsync(options, userOptions))
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

                SaveOptions(project, optionsPath, userOptions);

                LaunchCli(optionsPath, dbInfo);

                Telemetry.TrackEvent("PowerTools.CliRefresh");
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

        private async Task<bool> ChooseDataBaseConnectionAsync(DataApiBuilderOptions options, ReverseEngineerUserOptions userOptions)
        {
            var databaseList = await vsDataHelper.GetDataConnectionsAsync(package);

            databaseList = databaseList.Where(databaseList => Providers.GetDabProviders().Contains(databaseList.Value.DatabaseType))
                .ToDictionary(databaseList => databaseList.Key, databaseList => databaseList.Value);

            var dacpacList = await SqlProjHelper.GetDacpacFilesInActiveSolutionAsync();

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

            if (!string.IsNullOrEmpty(userOptions.UiHint))
            {
                psd.PublishUiHint(userOptions.UiHint);
            }

            psd.PublishSchemas(new List<SchemaInfo>());

            var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
            if (!pickDataSourceResult.ClosedByOK)
            {
                return false;
            }

            options.Dacpac = pickDataSourceResult.Payload.Connection?.FilePath;
            userOptions.UiHint = pickDataSourceResult.Payload.UiHint;

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

                options.Dacpac = await SqlProjHelper.BuildSqlProjAsync(options.Dacpac);
                if (string.IsNullOrEmpty(options.Dacpac))
                {
                    VSHelper.ShowMessage(ReverseEngineerLocale.UnableToBuildSelectedDatabaseProject);
                    return null;
                }

                dbInfo.FilePath = options.Dacpac;
            }

            if (dbInfo.DatabaseType == DatabaseType.Undefined)
            {
                VSHelper.ShowError($"{ReverseEngineerLocale.UnsupportedProvider}");
                return null;
            }

            return dbInfo;
        }

        private void SaveOptions(Project project, string optionsPath, ReverseEngineerUserOptions userOptions)
        {
            if (userOptions != null && !string.IsNullOrEmpty(userOptions.UiHint))
            {
                File.WriteAllText(optionsPath + ".user", userOptions.Write(Path.GetDirectoryName(project.FullPath)), Encoding.UTF8);
            }
        }

        private void LaunchCli(string configPath, DatabaseConnectionModel database)
        {
            var path = Path.GetDirectoryName(configPath);

            var proc = new Process();
            proc.StartInfo.FileName = "cmd";
            proc.StartInfo.Arguments = $" /k \"cd /d {path} && efcpt \"{database.FilePath ?? database.ConnectionString}\" {database.DatabaseType.ToDatabaseShortName()}\"";
            proc.Start();
        }
    }
}
