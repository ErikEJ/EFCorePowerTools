using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
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

        public CompareHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        public async System.Threading.Tasks.Task HandleComparisonAsync(string outputPath, Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            EnvDteHelper.ShowError("This feature is currently in early preview, please consult release notes for advice.");

            try
            {
                var dteH = new EnvDteHelper();

                if (_package.Dte2.Mode == vsIDEMode.vsIDEModeDebug)
                {
                    EnvDteHelper.ShowError("Cannot compare context to database while debugging");
                    return;
                }

                if (project == null)
                {
                    throw new ArgumentNullException(nameof(project));
                }

                if (project.Properties.Item("TargetFrameworkMoniker") == null)
                {
                    EnvDteHelper.ShowError("The selected project type has no TargetFrameworkMoniker");
                    return;
                }

                if (!project.IsNetCore30OrHigher())
                {
                    EnvDteHelper.ShowError("Only .NET Core 3.0+ projects are supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                    return;
                }

                var result = await project.ContainsEfCoreDesignReferenceAsync();

                if (!result.Item1)
                {
                    if (!Version.TryParse(result.Item2, out Version version))
                    {
                        EnvDteHelper.ShowError($"Cannot support version {result.Item2}, notice that previews have limited supported. You can try to manually install Microsoft.EntityFrameworkCore.Design preview.");
                        return;
                    }

                    if (version.Major != 5)
                    {
                        EnvDteHelper.ShowError($"Only EF Core 5 is supported.");
                        return;
                    }

                    //TODO Install EFSchemaCompare instead!
                    var nugetHelper = new NuGetHelper();
                    nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", project, version);
                    EnvDteHelper.ShowError($"Installing EFCore.Design version {version}, please retry the command");
                    return;
                }

                _package.Dte2.StatusBar.Text = "Loading data";
                object icon = (short)Microsoft.VisualStudio.Shell.Interop.Constants.SBAI_Build;
                _package.Dte2.StatusBar.Animate(true, icon);

                var databaseList = VsDataHelper.GetDataConnections(_package);
                var contextTypes = await GetDbContextTypesAsync(outputPath, project);

                var optionsDialog = _package.GetView<ICompareOptionsDialog>();

                if (databaseList != null && databaseList.Any())
                {
                    optionsDialog.AddConnections(databaseList.Select(m => new DatabaseConnectionModel
                    {
                        ConnectionName = m.Value.Caption,
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

                pickDataSourceResult.Payload.Connection.DataConnection.Open();
                pickDataSourceResult.Payload.Connection.ConnectionString = DataProtection.DecryptString(pickDataSourceResult.Payload.Connection.DataConnection.EncryptedConnectionString);

                _package.Dte2.StatusBar.Text = "Comparing database with context(s)...";
                _package.Dte2.StatusBar.Animate(true, icon);
                var timer = new Stopwatch();
                timer.Start();
                var comparisonResult = await GetComparisonResultAsync( 
                    outputPath,
                    project,
                    pickDataSourceResult.Payload.Connection, 
                    pickDataSourceResult.Payload.ContextTypes.First());
                timer.Stop();
                _package.Dte2.StatusBar.Animate(false, icon);
                _package.Dte2.StatusBar.Text = $"Compare completed in {timer.Elapsed:h\\:mm\\:ss}";
                
                if (comparisonResult.Any())
                {
                    var resultDialog = _package.GetView<ICompareResultDialog>();
                    resultDialog.AddComparisonResult(comparisonResult);
                    resultDialog.ShowAndAwaitUserResponse(true);
                }
                else
                {
                    EnvDteHelper.ShowMessage("Context(s) and database structure match");
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
                throw new ArgumentException("Unable to collect DbContext information", nameof(processResult));
            }

            if (processResult.StartsWith("Error:"))
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
            string contextName)
        {
            var processLauncher = new ProcessLauncher(project);

            var processResult = await processLauncher.GetOutputAsync(outputPath, GenerationType.DbContextCompare, contextName, connection.ConnectionString);

            if (string.IsNullOrEmpty(processResult))
            {
                throw new ArgumentException("Unable to collect SchemaCompare information", nameof(processResult));
            }

            if (processResult.StartsWith("Error:"))
            {
                throw new ArgumentException(processResult, nameof(processResult));
            }

            var modelResults = processLauncher.BuildModelResult(processResult);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CompareLogModel>>(modelResults.First().Item2);
        }
    }
}