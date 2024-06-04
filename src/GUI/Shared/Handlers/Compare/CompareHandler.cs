using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using NuGet.Versioning;

namespace EFCorePowerTools.Handlers.Compare
{
    internal class CompareHandler
    {
        private readonly EFCorePowerToolsPackage package;
        private readonly VsDataHelper vsDataHelper;

        public CompareHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
            vsDataHelper = new VsDataHelper();
        }

        public async System.Threading.Tasks.Task HandleComparisonAsync(string outputPath, Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (await VSHelper.IsDebugModeAsync())
                {
                    VSHelper.ShowError(CompareLocale.CannotCompareContextToDatabaseWhileDebugging);
                    return;
                }

                if (project == null)
                {
                    throw new ArgumentNullException(nameof(project));
                }

                if (await project.GetAttributeAsync("TargetFrameworkMoniker") == null)
                {
                    VSHelper.ShowError(SharedLocale.SelectedProjectTypeNoTargetFrameworkMoniker);
                    return;
                }

                if (!await project.IsNet60OrHigherAsync())
                {
                    VSHelper.ShowError($"{SharedLocale.SupportedFramework}: {await project.GetAttributeAsync("TargetFrameworkMoniker")}");
                    return;
                }

                var result = await project.ContainsEfSchemaCompareReferenceAsync();

                if (!result.Item1)
                {
                    if (!Version.TryParse(result.Item2, out Version version))
                    {
                        VSHelper.ShowError(string.Format(CultureInfo.InvariantCulture, CompareLocale.CannotSupportVersion, result.Item2));
                        return;
                    }

                    if (version.Major != 6 && version.Major != 7 && version.Major != 8)
                    {
                        VSHelper.ShowError(CompareLocale.VersionSupported);
                        return;
                    }

                    var nugetHelper = new NuGetHelper();
                    if (version.Major == 6)
                    {
                        nugetHelper.InstallPackage("EfCore.SchemaCompare", project, new NuGetVersion(6, 0, 0));
                    }
                    else if (version.Major == 7)
                    {
                        nugetHelper.InstallPackage("EfCore.SchemaCompare", project, new NuGetVersion(7, 0, 0));
                        nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", project, new NuGetVersion(7, 0, 0));
                    }
                    else if (version.Major == 8)
                    {
                        nugetHelper.InstallPackage("EfCore.SchemaCompare", project, new NuGetVersion(8, 0, 4));
                        nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", project, new NuGetVersion(8, 0, 0));
                    }

                    VSHelper.ShowError(CompareLocale.InstallingEfCoreSchemaCompare);
                    return;
                }

                await VS.StatusBar.ShowMessageAsync(CompareLocale.LoadingData);

                await VS.StatusBar.StartAnimationAsync(StatusAnimation.Build);

                var databaseList = await vsDataHelper.GetDataConnectionsAsync(package);
                var contextTypes = await GetDbContextTypesAsync(outputPath, project);

                var optionsDialog = package.GetView<ICompareOptionsDialog>();

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

                await VS.StatusBar.EndAnimationAsync(StatusAnimation.Build);
                await VS.StatusBar.ClearAsync();

                var pickDataSourceResult = optionsDialog.ShowAndAwaitUserResponse(true);
                if (!pickDataSourceResult.ClosedByOK)
                {
                    return;
                }

                if (!pickDataSourceResult.Payload.ContextTypes.Any())
                {
                    VSHelper.ShowError(CompareLocale.NoContextsSelected);
                    return;
                }

                pickDataSourceResult.Payload.Connection.DataConnection.Open();
                pickDataSourceResult.Payload.Connection.ConnectionString = DataProtection.DecryptString(pickDataSourceResult.Payload.Connection.DataConnection.EncryptedConnectionString);

                await VS.StatusBar.ShowMessageAsync(CompareLocale.ComparingDatabaseWithContexts);
                await VS.StatusBar.StartAnimationAsync(StatusAnimation.Build);

                var timer = new Stopwatch();
                timer.Start();
                var comparisonResult = await GetComparisonResultAsync(
                    outputPath,
                    project,
                    pickDataSourceResult.Payload.Connection,
                    pickDataSourceResult.Payload.ContextTypes.ToArray());
                timer.Stop();

                await VS.StatusBar.EndAnimationAsync(StatusAnimation.Build);
                await VS.StatusBar.ShowMessageAsync(string.Format(CompareLocale.CompareCompletedIn, timer.Elapsed.ToString("h\\:mm\\:ss")));

                if (comparisonResult.Any())
                {
                    var resultDialog = package.GetView<ICompareResultDialog>();
                    resultDialog.AddComparisonResult(comparisonResult);
                    resultDialog.ShowAndAwaitUserResponse(true);
                }
                else
                {
                    VSHelper.ShowMessage(CompareLocale.ContextDatabaseMatch);
                }

                await VS.StatusBar.ClearAsync();

                Telemetry.TrackEvent("PowerTools.Compare");
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

        private async Task<IEnumerable<string>> GetDbContextTypesAsync(string outputPath, Project project)
        {
            var processLauncher = new ProcessLauncher(project);

            var processResult = await processLauncher.GetOutputAsync(outputPath, GenerationType.DbContextList, null);

            if (string.IsNullOrEmpty(processResult))
            {
                throw new InvalidOperationException(CompareLocale.UnableToCollectDbContextInformation);
            }

            if (processResult.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new InvalidOperationException(processResult);
            }

            var modelResults = processLauncher.BuildModelResult(processResult);
            var result = new List<string>();

            foreach (var modelResult in modelResults)
            {
                result.Add(modelResult.Item1);
            }

            return result;
        }

        private async Task<IEnumerable<CompareLogModel>> GetComparisonResultAsync(
            string outputPath,
            Project project,
            DatabaseConnectionModel connection,
            string[] contextNames)
        {
            var processLauncher = new ProcessLauncher(project);

            var processResult = await processLauncher.GetOutputAsync(outputPath, GenerationType.DbContextCompare, string.Join(",", contextNames), connection.ConnectionString);

            if (string.IsNullOrEmpty(processResult))
            {
                throw new InvalidOperationException(CompareLocale.UnableToCollectSchemaCompareInformation);
            }

            if (processResult.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new InvalidOperationException(processResult);
            }

            var modelResults = processLauncher.BuildModelResult(processResult);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CompareLogModel>>(modelResults[0].Item2);
        }
    }
}
