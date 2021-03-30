using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Shared.Models;
using EnvDTE;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePowerTools.Handlers.Compare
{
    internal class CompareHandler
    {
        private readonly EFCorePowerToolsPackage _package;
        private readonly VsDataHelper vsDataHelper;

        public CompareHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
            vsDataHelper = new VsDataHelper();
        }

        public async System.Threading.Tasks.Task HandleComparisonAsync(string outputPath, Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (_package.Dte2.Mode == vsIDEMode.vsIDEModeDebug)
                {
                    EnvDteHelper.ShowError(CompareLocale.CannotCompareContextToDatabaseWhileDebugging);
                    return;
                }

                if (project == null)
                {
                    throw new ArgumentNullException(nameof(project));
                }

                if (project.Properties.Item("TargetFrameworkMoniker") == null)
                {
                    EnvDteHelper.ShowError(SharedLocale.SelectedProjectTypeNoTargetFrameworkMoniker);
                    return;
                }

                if (!project.IsNetCore31OrHigher())
                {
                    EnvDteHelper.ShowError($"{SharedLocale.SupportedFramework}: {project.Properties.Item("TargetFrameworkMoniker").Value}");
                    return;
                }

                var result = await project.ContainsEfSchemaCompareReferenceAsync();

                if (!result.Item1)
                {
                    if (!Version.TryParse(result.Item2, out Version version))
                    {
                        EnvDteHelper.ShowError(String.Format(CompareLocale.CannotSupportVersion, result.Item2));
                        return;
                    }

                    if (version.Major != 5)
                    {
                        EnvDteHelper.ShowError(CompareLocale.VersionSupported);
                        return;
                    }

                    var nugetHelper = new NuGetHelper();
                    nugetHelper.InstallPackage("EfCore.SchemaCompare", project, new Version(5, 1, 3));
                    EnvDteHelper.ShowError(CompareLocale.InstallingEfCoreSchemaCompare);
                    return;
                }

                _package.Dte2.StatusBar.Text = CompareLocale.LoadingData;
                object icon = (short)Microsoft.VisualStudio.Shell.Interop.Constants.SBAI_Build;
                _package.Dte2.StatusBar.Animate(true, icon);

                var databaseList = vsDataHelper.GetDataConnections(_package);
                var contextTypes = await GetDbContextTypesAsync(outputPath, project);

                var optionsDialog = _package.GetView<ICompareOptionsDialog>();

                if (databaseList != null && databaseList.Any())
                {
                    optionsDialog.AddConnections(databaseList.Select(m => new DatabaseConnectionModel
                    {
                        ConnectionName = m.Value.ConnectionName,
                        ConnectionString = m.Value.ConnectionString,
                        DatabaseType = m.Value.DatabaseType,
                        DataConnection = m.Value.DataConnection,
                    }));
                }

                optionsDialog.AddContextTypes(contextTypes);

                _package.Dte2.StatusBar.Animate(false, icon);
                _package.Dte2.StatusBar.Clear();
                var pickDataSourceResult = optionsDialog.ShowAndAwaitUserResponse(true);
                if (!pickDataSourceResult.ClosedByOK)
                    return;

                if (!pickDataSourceResult.Payload.ContextTypes.Any())
                {
                    EnvDteHelper.ShowError(CompareLocale.NoContextsSelected);
                    return;
                }

                pickDataSourceResult.Payload.Connection.DataConnection.Open();
                pickDataSourceResult.Payload.Connection.ConnectionString = DataProtection.DecryptString(pickDataSourceResult.Payload.Connection.DataConnection.EncryptedConnectionString);

                _package.Dte2.StatusBar.Text = CompareLocale.ComparingDatabaseWithContexts;
                _package.Dte2.StatusBar.Animate(true, icon);
                var timer = new Stopwatch();
                timer.Start();
                var comparisonResult = await GetComparisonResultAsync( 
                    outputPath,
                    project,
                    pickDataSourceResult.Payload.Connection, 
                    pickDataSourceResult.Payload.ContextTypes.ToArray());
                timer.Stop();
                _package.Dte2.StatusBar.Animate(false, icon);
                _package.Dte2.StatusBar.Text = String.Format(CompareLocale.CompareCompletedIn, timer.Elapsed.ToString("h\\:mm\\:ss"));
                
                if (comparisonResult.Any())
                {
                    var resultDialog = _package.GetView<ICompareResultDialog>();
                    resultDialog.AddComparisonResult(comparisonResult);
                    resultDialog.ShowAndAwaitUserResponse(true);
                }
                else
                {
                    EnvDteHelper.ShowMessage(CompareLocale.ContextDatabaseMatch);
                }

                _package.Dte2.StatusBar.Clear();

                Telemetry.TrackEvent("PowerTools.Compare");
            }
            catch (AggregateException ae)
            {
                foreach (var innerException in ae.Flatten().InnerExceptions)
                {
                    _package.LogError(new List<string>(), innerException);
                }
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }

        private async Task<IEnumerable<string>> GetDbContextTypesAsync(string outputPath, Project project)
        {
            var processLauncher = new ProcessLauncher(project);

            var processResult = await processLauncher.GetOutputAsync(outputPath, GenerationType.DbContextList, null);

            if (string.IsNullOrEmpty(processResult))
            {
                throw new ArgumentException(CompareLocale.UnableToCollectDbContextInformation, nameof(processResult));
            }

            if (processResult.Contains("Error:"))
            {
                throw new ArgumentException(processResult, nameof(processResult));
            }

            var modelResults = processLauncher.BuildModelResult(processResult);
            var result = new List<string>();

            foreach (var modelResult in modelResults)
            {
                result.Add(modelResult.Item1);
            }

            return result;
        }

        private async Task<IEnumerable<CompareLogModel>> GetComparisonResultAsync(string outputPath, 
            Project project, 
            DatabaseConnectionModel connection, 
            string[] contextNames)
        {
            var processLauncher = new ProcessLauncher(project);

            var processResult = await processLauncher.GetOutputAsync(outputPath, GenerationType.DbContextCompare, string.Join(",", contextNames), connection.ConnectionString);

            if (string.IsNullOrEmpty(processResult))
            {
                throw new ArgumentException(CompareLocale.UnableToCollectSchemaCompareInformation, nameof(processResult));
            }

            if (processResult.Contains("Error:"))
            {
                throw new ArgumentException(processResult, nameof(processResult));
            }

            var modelResults = processLauncher.BuildModelResult(processResult);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CompareLogModel>>(modelResults.First().Item2);
        }
    }
}