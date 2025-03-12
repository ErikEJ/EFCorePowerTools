using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.BLL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.Models;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Wizard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using NuGet.Versioning;
using RevEng.Common;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class RevEngWizardHandler : IReverseEngineerBll
    {
        private readonly EFCorePowerToolsPackage package;
        private readonly ReverseEngineerHelper reverseEngineerHelper;
        private readonly VsDataHelper vsDataHelper;
        private List<string> legacyDiscoveryObjects = new List<string>();
        private Dictionary<string, string> mappedTypes = new Dictionary<string, string>();

        public static bool WizardIsRunning { get; set; }

        public RevEngWizardHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
            reverseEngineerHelper = new ReverseEngineerHelper();
            vsDataHelper = new VsDataHelper();
        }

        public async System.Threading.Tasks.Task<(string OptionsPath, Project Project)> DropSqlprojOptionsAsync(List<Project> candidateProjects, string sqlProjectPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var project = candidateProjects[0];

            if (candidateProjects.Count > 1)
            {
                var pcd = package.GetView<IPickProjectDialog>();
                pcd.PublishProjects(candidateProjects.Select(m => new ProjectModel
                {
                    Project = m,
                }));

                var pickProjectResult = pcd.ShowAndAwaitUserResponse(true);
                if (!pickProjectResult.ClosedByOK)
                {
                    return (null, null);
                }

                project = pickProjectResult.Payload.Project;
            }

            var optionsPaths = project.GetConfigFiles();
            var optionsPath = optionsPaths[0];

            if (File.Exists(optionsPath))
            {
                return (null, project);
            }

            var options = new ReverseEngineerOptions
            {
                Tables = new List<SerializationTableModel>(),
                CodeGenerationMode = CodeGenerationMode.EFCore8,
                DatabaseType = DatabaseType.SQLServerDacpac,
                UiHint = sqlProjectPath,
                ProjectRootNamespace = await project.GetAttributeAsync("RootNamespace"),
                OutputPath = "Models",
            };

            await SaveOptionsAsync(project, optionsPath, options, null, new Tuple<List<Schema>, string>(null, null));

            return (optionsPath, project);
        }

        // Note: entry point for launching wizard [experimental] menu options
        public async Task ReverseEngineerCodeFirstLaunchWizardAsync(WizardEventArgs wizardArgs)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                var project = await VS.Solutions.GetActiveProjectAsync();

                if (project == null)
                {
                    await VS.StatusBar.ShowMessageAsync($"Unable to find active project");
                    return;
                }

                if (await VSHelper.IsDebugModeAsync())
                {
                    VSHelper.ShowError(ReverseEngineerLocale.CannotGenerateCodeWhileDebugging);
                    return;
                }

                var wizardViewModel = wizardArgs.ServiceProvider.GetService<IWizardViewModel>();
                wizardArgs.Project = project; // we'll need this downstream

                // WizardDialogBox constructor is expecting instance of IReverseEngineerBll
                // which this class implements.  The wizard pages will use the BLL to process
                // data using existing business logic; to simplify wizard refactor this handler
                // serves as the BLL (and DAL)
                if (!WizardIsRunning)
                {
                    WizardIsRunning = true;
                    var wizard = new WizardDialogBox(this, wizardArgs, wizardViewModel);
                    wizard.Owner = Application.Current.MainWindow;
                    wizard.Show();
                }

                await Task.Yield();
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

        // Note: invoked by wizard page 1 (Wiz1_PickServerDatabaseDialog)
        public async Task ReverseEngineerCodeFirstAsync(string uiHint = null, WizardEventArgs wizardArgs = null)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                var project = await VS.Solutions.GetActiveProjectAsync();

                if (project == null)
                {
                    await VS.StatusBar.ShowMessageAsync($"Unable to find active project");
                    return;
                }

                if (await VSHelper.IsDebugModeAsync())
                {
                    VSHelper.ShowError(ReverseEngineerLocale.CannotGenerateCodeWhileDebugging);
                    return;
                }

                var projectPath = project.FullPath;
                var optionsPaths = project.GetConfigFiles();
                var optionsPath = optionsPaths[0];

                if (optionsPaths.Count > 1)
                {
                    var pcd = package.GetView<IPickConfigDialog>();
                    pcd.PublishConfigurations(optionsPaths.Select(m => new ConfigModel
                    {
                        ConfigPath = m,
                        ProjectPath = projectPath,
                    }));

                    var pickConfigResult = pcd.ShowAndAwaitUserResponse(true);
                    if (!pickConfigResult.ClosedByOK)
                    {
                        return;
                    }

                    optionsPath = pickConfigResult.Payload.ConfigPath;
                }

                if (wizardArgs != null)
                {
                    wizardArgs.Project = project;
                    wizardArgs.OptionsPath = optionsPath;
                    wizardArgs.OnlyGenerate = false;
                    wizardArgs.FromSqlProject = false;
                    wizardArgs.UiHint = uiHint;

                    wizardArgs.Configurations.Add(new ConfigModel
                    {
                        ConfigPath = optionsPath,
                        ProjectPath = projectPath,
                    });
                }

                await ReverseEngineerCodeFirstAsync(project, optionsPath, false, false, uiHint, wizardArgs);
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

        public async System.Threading.Tasks.Task ReverseEngineerCodeFirstAsync(Project project, string optionsPath, bool onlyGenerate, bool fromSqlProj = false, string uiHint = null, WizardEventArgs wizardArgs = null)
        {
            // Ensure that there is an instance of wizardArgs so that we don't have to
            // have extra logic checks for null downstream
            wizardArgs = wizardArgs ?? new WizardEventArgs();

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (await VSHelper.IsDebugModeAsync())
                {
                    wizardArgs.StatusbarMessage = ReverseEngineerLocale.CannotGenerateCodeWhileDebugging;

                    if (!wizardArgs.IsInvokedByWizard)
                    {
                        VSHelper.ShowError(wizardArgs.StatusbarMessage);
                    }

                    return;
                }

                var renamingPath = project.GetRenamingPath(optionsPath);
                var referenceRenamingPath = project.GetRenamingPath(optionsPath, true);
                if (!string.IsNullOrEmpty(referenceRenamingPath) && File.Exists(referenceRenamingPath))
                {
                    wizardArgs.StatusbarMessage = "Property renaming (experimental) is no longer available. See GitHub issue #2171.";

                    if (!wizardArgs.IsInvokedByWizard)
                    {
                        VSHelper.ShowMessage(wizardArgs.StatusbarMessage);
                    }
                }

                var namingOptionsAndPath = CustomNameOptionsExtensions.TryRead(renamingPath, optionsPath);

                var options = ReverseEngineerOptionsExtensions.TryRead(optionsPath);

                var userOptions = ReverseEngineerUserOptionsExtensions.TryRead(optionsPath, Path.GetDirectoryName(project.FullPath));

                var newOptions = false;

                if (options == null)
                {
                    options = new ReverseEngineerOptions
                    {
                        ProjectRootNamespace = await project.GetAttributeAsync("RootNamespace"),
                        OutputPath = "Models",
                    };
                    newOptions = true;
                }

                if (userOptions == null)
                {
                    userOptions = new ReverseEngineerUserOptions
                    {
                        UiHint = uiHint ?? options.UiHint,
                    };
                }

                options.UiHint = uiHint ?? userOptions.UiHint;

                legacyDiscoveryObjects = options.Tables?.Where(t => t.UseLegacyResultSetDiscovery).Select(t => t.Name).ToList() ?? new List<string>();
                mappedTypes = options.Tables?
                    .Where(t => !string.IsNullOrEmpty(t.MappedType) && t.ObjectType == ObjectType.Procedure)
                    .Select(m => new { m.Name, m.MappedType }).ToDictionary(m => m.Name, m => m.MappedType) ?? new Dictionary<string, string>();

                options.ProjectPath = Path.GetDirectoryName(project.FullPath);
                options.OptionsPath = Path.GetDirectoryName(optionsPath);

                bool forceEdit = false;

                var neededPackages = new List<NuGetPackage>();

                DatabaseConnectionModel dbInfo = null;
                wizardArgs.NewOptions = newOptions;  // update wizard args with new options state

                if (onlyGenerate || fromSqlProj)
                {
                    forceEdit = !await ChooseDataBaseConnectionByUiHintAsync(options);

                    if (forceEdit)
                    {
                        wizardArgs.StatusbarMessage = ReverseEngineerLocale.DatabaseConnectionNotFoundCannotRefresh;

                        if (!wizardArgs.IsInvokedByWizard)
                        {
                            await VS.StatusBar.ShowMessageAsync(wizardArgs.StatusbarMessage);
                        }
                    }
                    else
                    {
                        dbInfo = await GetDatabaseInfoAsync(options);

                        if (dbInfo == null)
                        {
                            return;
                        }

                        options.CustomReplacers = namingOptionsAndPath.Item1;
                        options.InstallNuGetPackage = !onlyGenerate;
                    }
                }

                if (!onlyGenerate || forceEdit)
                {
                    if (!fromSqlProj)
                    {
                        // #1 load database connections (IPickServerDatabaseDialog)
                        if (!await ChooseDataBaseConnectionAsync(options, project, wizardArgs))
                        {
                            if (!wizardArgs.IsInvokedByWizard)
                            {
                                await VS.StatusBar.ClearAsync();
                            }

                            return;
                        }

                        if (newOptions)
                        {
                            options.UseDateOnlyTimeOnly = options.CodeGenerationMode == CodeGenerationMode.EFCore8;
                        }

                        if (!wizardArgs.IsInvokedByWizard)
                        {
                            await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.GettingReadyToConnect);
                        }

                        // If no connection string is set then there is no need to get database info
                        // as it will result in a dialog complaining about the provider
                        if (options.ConnectionString != null)
                        {
                            dbInfo = await GetDatabaseInfoAsync(options);
                        }

                        if (dbInfo == null || wizardArgs.PickServerDatabaseComplete)
                        {
                            // If being invoked by the wizard and PickServerDatabaseComplete then we'll update
                            // the wizard args state so that it can continue processing.  This handler is no
                            // longer driving the logic flow - the wizard pages are.
                            wizardArgs.DbInfo = dbInfo;
                            wizardArgs.UserOptions = userOptions;
                            wizardArgs.Options = options;
                            wizardArgs.NamingOptionsAndPath = namingOptionsAndPath;
                            return;
                        }
                    }

                    //------ WIZARD STOPS HERE ---- INVOKES METHODS DIRECTLY --------//

                    // #2 load tables (IPickTablesDialog)
                    await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.LoadingDatabaseObjects);

                    if (!await LoadDataBaseObjectsAsync(options, dbInfo, namingOptionsAndPath))
                    {
                        await VS.StatusBar.ClearAsync();
                        return;
                    }

                    // #3 Load modeling options (IModelingOptionsDialog)
                    await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.LoadingOptions);

                    neededPackages = await project.GetNeededPackagesAsync(options);
                    options.InstallNuGetPackage = neededPackages
                        .Exists(p => p.DatabaseTypes.Contains(options.DatabaseType) && !p.Installed);

                    if (!await GetModelOptionsAsync(options, project.Name))
                    {
                        await VS.StatusBar.ClearAsync();
                        return;
                    }

                    if (newOptions)
                    {
                        // HACK Work around for issue with web app project system on initial run
                        userOptions = null;
                    }

                    await SaveOptionsAsync(project, optionsPath, options, userOptions, new Tuple<List<Schema>, string>(options.CustomReplacers, namingOptionsAndPath.Item2));
                }

                await InstallNuGetPackagesAsync(project, onlyGenerate, options, forceEdit);

                var missingProviderPackage = neededPackages.Find(p => p.DatabaseTypes.Contains(options.DatabaseType) && p.IsMainProviderPackage && !p.Installed)?.PackageId;
                if (options.InstallNuGetPackage || options.SelectedToBeGenerated == 2)
                {
                    missingProviderPackage = null;
                }

                await GenerateFilesAsync(project, options, missingProviderPackage, onlyGenerate, neededPackages);

                var postRunFile = Path.Combine(Path.GetDirectoryName(optionsPath), "efpt.postrun.cmd");
                if (File.Exists(postRunFile))
                {
                    Process.Start($"\"{postRunFile}\"");
                }

                Telemetry.TrackEvent("PowerTools.ReverseEngineer");
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

#pragma warning disable SA1204 // Static elements should appear before instance elements
        public static async Task InstallNuGetPackagesAsync(Project project, bool onlyGenerate, ReverseEngineerOptions options, bool forceEdit, WizardEventArgs wizardArgs = null)
#pragma warning restore SA1204 // Static elements should appear before instance elements
        {
            var nuGetHelper = new NuGetHelper();

            if (options.InstallNuGetPackage
                && (!onlyGenerate || forceEdit)
                && (await project.IsNet60OrHigherAsync() || await project.IsNetStandardAsync()))
            {
                var packages = await project.GetNeededPackagesAsync(options);

                var packagesToInstall = packages.Where(p => p.DatabaseTypes.Contains(options.DatabaseType) && !p.Installed).ToList();

                if (!packagesToInstall.Any())
                {
                    return;
                }

                if (!wizardArgs.IsInvokedByWizard)
                {
                    await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.InstallingEFCoreProviderPackage);
                }

                foreach (var nuGetPackage in packagesToInstall)
                {
                    nuGetHelper.InstallPackage(nuGetPackage.PackageId, project, new NuGetVersion(nuGetPackage.Version));
                }
            }
        }

        // Note: invoked by page 3 of wizard (Wiz3_EfCoreModelDialog)
        public async System.Threading.Tasks.Task<string> GenerateFilesAsync(Project project, ReverseEngineerOptions options, string missingProviderPackage, bool onlyGenerate, List<NuGetPackage> packages, bool isCalledByWizard = false)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (!isCalledByWizard)
            {
                await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 1, 4);
            }

            var stopWatch = Stopwatch.StartNew();

            if (options.UseHandleBars || ((options.UseT4 || options.UseT4Split) && string.IsNullOrEmpty(options.T4TemplatePath)))
            {
                var result = ReverseEngineerHelper.DropTemplates(options.OptionsPath, options.ProjectPath, options.CodeGenerationMode, options.UseHandleBars, options.SelectedHandlebarsLanguage);
                if (!string.IsNullOrEmpty(result))
                {
                    await VS.MessageBox.ShowAsync(
                        "EF Core Power Tools",
                        result,
                        icon: Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_WARNING,
                        buttons: Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK);
                }
            }

            options.UseNullableReferences = !await project.IsNetStandardAsync() && options.UseNullableReferences;

            if (!isCalledByWizard)
            {
                await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 2, 4);
            }

            var revEngResult = await EfRevEngLauncher.LaunchExternalRunnerAsync(options, options.CodeGenerationMode);

            if (!isCalledByWizard)
            {
                await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 3, 4);
            }

            var readmePath = string.Empty;
            var finalText = string.Empty;
            if ((options.SelectedToBeGenerated == 0 || options.SelectedToBeGenerated == 1)
                && AdvancedOptions.Instance.OpenGeneratedDbContext && !onlyGenerate)
            {
                var readmeName = "PowerToolsReadMe.md";
                var template = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), readmeName), Encoding.UTF8);

                if (packages.Any())
                {
                    finalText = ReverseEngineerHelper.GetReadMeText(options, template, packages);
                }
                else
                {
                    finalText = ReverseEngineerHelper.GetReadMeText(options, template);
                }

                readmePath = Path.Combine(Path.GetTempPath(), readmeName);

                finalText = ReverseEngineerHelper.AddResultToFinalText(finalText, revEngResult);

                File.WriteAllText(readmePath, finalText, Encoding.UTF8);

                if (revEngResult.HasIssues)
                {
                    if (!string.IsNullOrEmpty(revEngResult.ContextFilePath))
                    {
                        await VS.Documents.OpenAsync(revEngResult.ContextFilePath);
                    }

                    await VS.Documents.OpenInPreviewTabAsync(readmePath);
                }
                else
                {
                    await VS.Documents.OpenInPreviewTabAsync(readmePath);

                    if (!string.IsNullOrEmpty(revEngResult.ContextFilePath))
                    {
                        await VS.Documents.OpenAsync(revEngResult.ContextFilePath);
                    }
                }
            }

            await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 4, 4);

            stopWatch.Stop();

            var errors = ReverseEngineerHelper.ReportRevEngErrors(revEngResult, missingProviderPackage);

            var completedMessage = string.Format(ReverseEngineerLocale.ReverseEngineerCompleted, stopWatch.Elapsed.ToString(@"mm\:ss"));
            await VS.StatusBar.ShowMessageAsync(completedMessage);

            var statusMessage = new StringBuilder();

            if (errors != ReverseEngineerLocale.ModelGeneratedSuccesfully + Environment.NewLine)
            {
                if (isCalledByWizard)
                {
                    foreach (var warning in revEngResult.EntityWarnings)
                    {
                        statusMessage.AppendLine("⚠️ " + warning);
                    }

                    foreach (var error in revEngResult.EntityErrors)
                    {
                        statusMessage.AppendLine("❌ " + error);
                    }
                }
                else
                {
                    VSHelper.ShowMessage(errors);
                }
            }
            else
            {
                if (isCalledByWizard)
                {
                    statusMessage.AppendLine("✅ " + ReverseEngineerLocale.ModelGeneratedSuccesfully);
                }
            }

            if (revEngResult.EntityErrors.Any())
            {
                EFCorePowerToolsPackage.LogError(revEngResult.EntityErrors, null);
            }

            if (revEngResult.EntityWarnings.Any())
            {
                EFCorePowerToolsPackage.LogError(revEngResult.EntityWarnings, null);
            }

            Telemetry.TrackFrameworkUse(nameof(ReverseEngineerHandler), options.CodeGenerationMode);
            Telemetry.TrackEngineUse(options.DatabaseType, revEngResult.DatabaseEdition, revEngResult.DatabaseVersion, revEngResult.DatabaseLevel, revEngResult.DatabaseEditionId);

            var messageForStatusPage = statusMessage.ToString();
            return string.IsNullOrEmpty(messageForStatusPage) ? "Successfully completed" : messageForStatusPage;
        }

        private static async Task<List<TableModel>> GetDacpacTablesAsync(string dacpacPath, CodeGenerationMode codeGenerationMode)
        {
            var builder = new TableListBuilder(dacpacPath, DatabaseType.SQLServerDacpac, null);

            return await builder.GetTableDefinitionsAsync(codeGenerationMode);
        }

        private static async Task<List<TableModel>> GetTablesAsync(DatabaseConnectionModel dbInfo, CodeGenerationMode codeGenerationMode, SchemaInfo[] schemas)
        {
            if (dbInfo.DataConnection != null)
            {
                dbInfo.DataConnection.Open();
                dbInfo.ConnectionString = DataProtection.DecryptString(dbInfo.DataConnection.EncryptedConnectionString);
            }

            var builder = new TableListBuilder(dbInfo.ConnectionString, dbInfo.DatabaseType, schemas);
            return await builder.GetTableDefinitionsAsync(codeGenerationMode);
        }

#pragma warning disable SA1202 // Elements should be ordered by access
        public static async Task<DatabaseConnectionModel> GetDatabaseInfoAsync(ReverseEngineerOptions options)
#pragma warning restore SA1202 // Elements should be ordered by access
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

        private async Task<bool> ChooseDataBaseConnectionByUiHintAsync(ReverseEngineerOptions options)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var databaseList = await vsDataHelper.GetDataConnectionsAsync(package);
            if (databaseList != null && databaseList.Any())
            {
                var dataBaseInfo = databaseList.Values.FirstOrDefault(m => m.ConnectionName == options.UiHint);
                if (dataBaseInfo != null)
                {
                    options.ConnectionString = dataBaseInfo.ConnectionString;
                    options.DatabaseType = dataBaseInfo.DatabaseType;
                    return true;
                }
            }

            var dacpacList = await SqlProjHelper.GetDacpacProjectsInActiveSolutionAsync();
            if (dacpacList != null && dacpacList.Any() && !string.IsNullOrEmpty(options.UiHint))
            {
                var candidate = dacpacList
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .FirstOrDefault(m => m.Equals(options.UiHint, StringComparison.OrdinalIgnoreCase));

                if (candidate != null)
                {
                    options.Dacpac = candidate;
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> ChooseDataBaseConnectionAsync(ReverseEngineerOptions options, Project project, WizardEventArgs wizardArgs = null)
        {
            var databaseList = await vsDataHelper.GetDataConnectionsAsync(package);
            var dacpacList = await SqlProjHelper.GetDacpacProjectsInActiveSolutionAsync();

            // If the wizard is driving then it's implementation of the interface will be used.
            var psd = wizardArgs?.PickServerDatabaseDialog ?? package.GetView<IPickServerDatabaseDialog>();

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

            if (options.FilterSchemas && options.Schemas != null && options.Schemas.Any())
            {
                psd.PublishSchemas(options.Schemas);
            }

            var (usedMode, allowedVersions) = reverseEngineerHelper.CalculateAllowedVersions(options.CodeGenerationMode, await project.GetEFCoreVersionHintAsync());

            if (!allowedVersions.Any())
            {
                wizardArgs.StatusbarMessage = $".NET 5 and earlier is not supported.";
                if (!wizardArgs.IsInvokedByWizard)
                {
                    VSHelper.ShowError(wizardArgs.StatusbarMessage);
                }

                return false;
            }

            psd.PublishCodeGenerationMode(usedMode, allowedVersions);

            if (!string.IsNullOrEmpty(options.UiHint))
            {
                psd.PublishUiHint(options.UiHint);
            }

            var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
            if (!pickDataSourceResult.ClosedByOK)
            {
                return false;
            }

            options.CodeGenerationMode = pickDataSourceResult.Payload.CodeGenerationMode;
            options.FilterSchemas = pickDataSourceResult.Payload.FilterSchemas;
            options.Schemas = options.FilterSchemas ? pickDataSourceResult.Payload.Schemas?.ToList() : null;
            options.UiHint = pickDataSourceResult.Payload.UiHint;
            options.Dacpac = pickDataSourceResult.Payload.Connection?.FilePath;

            if (pickDataSourceResult.Payload.Connection != null)
            {
                options.ConnectionString = pickDataSourceResult.Payload.Connection.ConnectionString;
                options.DatabaseType = pickDataSourceResult.Payload.Connection.DatabaseType;
            }

            return true;
        }

        // Note: invoked by page 2 of the wizard (Wiz2_PickTablesDialog)
#pragma warning disable SA1202 // Elements should be ordered by access
        public async Task<bool> LoadDataBaseObjectsAsync(ReverseEngineerOptions options, DatabaseConnectionModel dbInfo, Tuple<List<Schema>, string> namingOptionsAndPath, WizardEventArgs wizardArgs = null)
#pragma warning restore SA1202 // Elements should be ordered by access
        {
            IEnumerable<TableModel> predefinedTables = null;

            try
            {
                if (!wizardArgs.IsInvokedByWizard)
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    await VS.StatusBar.StartAnimationAsync(StatusAnimation.Build);
                }

                predefinedTables = !string.IsNullOrEmpty(options.Dacpac)
                                           ? await GetDacpacTablesAsync(options.Dacpac, options.CodeGenerationMode)
                                           : await GetTablesAsync(dbInfo, options.CodeGenerationMode, options.Schemas?.ToArray());
            }
            catch (InvalidOperationException ex)
            {
                if (wizardArgs.IsInvokedByWizard)
                {
                    wizardArgs.StatusbarMessage = ex.Message;
                }
                else
                {
                    VSHelper.ShowError($"{ex.Message}");
                }

                return false;
            }
            finally
            {
                if (!wizardArgs.IsInvokedByWizard)
                {
                    await VS.StatusBar.EndAnimationAsync(StatusAnimation.Build);
                }
            }

            var isSqliteToolboxInstalled = options.DatabaseType != DatabaseType.SQLite;

            if (!wizardArgs.IsInvokedByWizard)
            {
                await VS.StatusBar.EndAnimationAsync(StatusAnimation.Build);
            }

            var preselectedTables = new List<SerializationTableModel>();

            if (options.Tables?.Count > 0)
            {
                var normalizedTables = ReverseEngineerHelper.NormalizeTables(options.Tables, dbInfo.DatabaseType == DatabaseType.SQLServer);
                preselectedTables.AddRange(normalizedTables);
            }

            if (wizardArgs.IsInvokedByWizard)
            {
                await Task.Yield();
            }
            else
            {
                await VS.StatusBar.ClearAsync();
            }

            // If the wizard is driving then it's implementation of the interface will be used.
            var ptd = wizardArgs?.PickTablesDialog ?? package.GetView<IPickTablesDialog>();
            ptd.AddTables(predefinedTables, namingOptionsAndPath.Item1)
               .PreselectTables(preselectedTables)
               .SqliteToolboxInstall(isSqliteToolboxInstalled);

            var pickTablesResult = ptd.ShowAndAwaitUserResponse(true);

            options.Tables = pickTablesResult.Payload.Objects.ToList();
            options.CustomReplacers = pickTablesResult.Payload.CustomReplacers.ToList();

            return pickTablesResult.ClosedByOK;
        }

        // Note: invoked by page 3 of the wizard (Wiz3_EfCoreModelDiagram)
        public async Task<bool> GetModelOptionsAsync(ReverseEngineerOptions options, string projectName, WizardEventArgs wizardArgs = null)
        {
            var isInvokedByWizard = wizardArgs.PickTablesDialogComplete;

            // If this is being invoked by wizard then get fresh list of selected files for processing
            // (developer can select/deselect other objects).
            if (isInvokedByWizard)
            {
                // If the wizard is driving then it's implementation of the interface will be used.
                var ptd = wizardArgs.PickTablesDialog ?? package.GetView<IPickTablesDialog>();
                var pickTablesResult = ptd.ShowAndAwaitUserResponse(true);

                options.Tables = pickTablesResult.Payload.Objects.ToList();
                options.CustomReplacers = pickTablesResult.Payload.CustomReplacers.ToList();
            }

            var classBasis = DbContextNamer.GetDatabaseName(options.ConnectionString, options.DatabaseType);

            if (string.IsNullOrEmpty(options.ContextClassName))
            {
                options.ContextClassName = classBasis + "Context";
            }

            var presets = new ModelingOptionsModel
            {
                InstallNuGetPackage = options.InstallNuGetPackage,
                ModelName = options.ContextClassName,
                ProjectName = projectName,
                Namespace = options.ProjectRootNamespace,
                DacpacPath = options.Dacpac,
                UseDataAnnotations = !options.UseFluentApiOnly,
                UseDatabaseNames = options.UseDatabaseNames,
                UsePluralizer = options.UseInflector,
                UseDbContextSplitting = options.UseDbContextSplitting,
                UseHandlebars = options.UseHandleBars || options.UseT4 || options.UseT4Split,
                SelectedHandlebarsLanguage = options.SelectedHandlebarsLanguage,
                IncludeConnectionString = options.IncludeConnectionString,
                OutputPath = options.OutputPath,
                OutputContextPath = options.OutputContextPath,
                UseSchemaFolders = options.UseSchemaFolders,
                ModelNamespace = options.ModelNamespace,
                ContextNamespace = options.ContextNamespace,
                SelectedToBeGenerated = options.SelectedToBeGenerated,
                UseEf6Pluralizer = options.UseLegacyPluralizer,
                MapSpatialTypes = options.UseSpatial,
                MapHierarchyId = options.UseHierarchyId,
                MapNodaTimeTypes = options.UseNodaTime,
                UseBoolPropertiesWithoutDefaultSql = options.UseBoolPropertiesWithoutDefaultSql,
                UseNullableReferences = options.UseNullableReferences,
                UseNoObjectFilter = options.UseNoObjectFilter,
                UseNoNavigations = options.UseNoNavigations,
                UseNoDefaultConstructor = options.UseNoDefaultConstructor,
                UseManyToManyEntity = options.UseManyToManyEntity,
                UseDateOnlyTimeOnly = options.UseDateOnlyTimeOnly,
                UseSchemaNamespaces = options.UseSchemaNamespaces,
                T4TemplatePath = options.T4TemplatePath,
            };

            // If the wizard is driving then it's implementation of the interface will be used.
            var modelDialog = wizardArgs.ModelingOptionsDialog ?? package.GetView<IModelingOptionsDialog>();
            modelDialog.ApplyPresets(presets);

            var allowedTemplates = reverseEngineerHelper.CalculateAllowedTemplates(options.CodeGenerationMode);

            modelDialog.PublishTemplateTypes(
                new Contracts.ViewModels.TemplateTypeItem { Key = options.SelectedHandlebarsLanguage },
                allowedTemplates);

            if (!wizardArgs.IsInvokedByWizard)
            {
                await VS.StatusBar.ClearAsync();
            }

            var modelingOptionsResult = modelDialog.ShowAndAwaitUserResponse(true);

            if (!modelingOptionsResult.ClosedByOK)
            {
                return false;
            }

            if (isInvokedByWizard)
            {
                return true;
            }

            return GetModelOptionsPostDialog(options, projectName, wizardArgs, modelingOptionsResult.Payload);
        }

        public bool GetModelOptionsPostDialog(
            ReverseEngineerOptions options,
            string projectName,
            WizardEventArgs wizardArgs = null,
            ModelingOptionsModel modelingOptionsResult = null)
        {
            var isHandleBarsLanguage = modelingOptionsResult.SelectedHandlebarsLanguage == 0
                || modelingOptionsResult.SelectedHandlebarsLanguage == 1;
            options.UseHandleBars = modelingOptionsResult.UseHandlebars && isHandleBarsLanguage;
            options.SelectedHandlebarsLanguage = modelingOptionsResult.SelectedHandlebarsLanguage;

            options.UseT4 = modelingOptionsResult.UseHandlebars && !isHandleBarsLanguage;

            if (modelingOptionsResult.UseHandlebars
                && modelingOptionsResult.SelectedHandlebarsLanguage == 4)
            {
                options.UseT4 = false;
                options.UseT4Split = true;
            }

            options.InstallNuGetPackage = modelingOptionsResult.InstallNuGetPackage;
            options.UseFluentApiOnly = !modelingOptionsResult.UseDataAnnotations;
            options.ContextClassName = modelingOptionsResult.ModelName;
            options.OutputPath = modelingOptionsResult.OutputPath;
            options.OutputContextPath = modelingOptionsResult.OutputContextPath;
            options.ContextNamespace = modelingOptionsResult.ContextNamespace;
            options.UseSchemaFolders = modelingOptionsResult.UseSchemaFolders;
            options.ModelNamespace = modelingOptionsResult.ModelNamespace;
            options.ProjectRootNamespace = modelingOptionsResult.Namespace;
            options.UseDatabaseNames = modelingOptionsResult.UseDatabaseNames;
            options.UseInflector = modelingOptionsResult.UsePluralizer;
            options.UseLegacyPluralizer = modelingOptionsResult.UseEf6Pluralizer;
            options.UseSpatial = modelingOptionsResult.MapSpatialTypes;
            options.UseHierarchyId = modelingOptionsResult.MapHierarchyId;
            options.UseNodaTime = modelingOptionsResult.MapNodaTimeTypes;
            options.UseDbContextSplitting = modelingOptionsResult.UseDbContextSplitting;
            options.IncludeConnectionString = modelingOptionsResult.IncludeConnectionString;
            options.SelectedToBeGenerated = modelingOptionsResult.SelectedToBeGenerated;
            options.UseBoolPropertiesWithoutDefaultSql = modelingOptionsResult.UseBoolPropertiesWithoutDefaultSql;
            options.UseNullableReferences = modelingOptionsResult.UseNullableReferences;
            options.UseNoObjectFilter = modelingOptionsResult.UseNoObjectFilter;
            options.UseNoNavigations = modelingOptionsResult.UseNoNavigations;
            options.UseNoDefaultConstructor = modelingOptionsResult.UseNoDefaultConstructor;
            options.UseManyToManyEntity = modelingOptionsResult.UseManyToManyEntity;
            options.UseDateOnlyTimeOnly = modelingOptionsResult.UseDateOnlyTimeOnly;
            options.UseSchemaNamespaces = modelingOptionsResult.UseSchemaNamespaces;
            options.T4TemplatePath = modelingOptionsResult.T4TemplatePath;

            return true;
        }

        public async System.Threading.Tasks.Task SaveOptionsAsync(Project project, string optionsPath, ReverseEngineerOptions options, ReverseEngineerUserOptions userOptions, Tuple<List<Schema>, string> renamingOptions)
        {
            if (optionsPath.EndsWith(Constants.ConfigFileName, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (File.Exists(optionsPath) && File.GetAttributes(optionsPath).HasFlag(FileAttributes.ReadOnly))
            {
                VSHelper.ShowError($"Unable to save options, the file is readonly: {optionsPath}");
                return;
            }

            if (!File.Exists(optionsPath + ".ignore"))
            {
                if (userOptions != null)
                {
                    userOptions.UiHint = options.UiHint;
                    File.WriteAllText(optionsPath + ".user", userOptions.Write(Path.GetDirectoryName(project.FullPath)), Encoding.UTF8);
                }

                options.UiHint = null;

                foreach (var table in options.Tables)
                {
                    if (legacyDiscoveryObjects.Contains(table.Name))
                    {
                        table.UseLegacyResultSetDiscovery = true;
                    }

                    if (mappedTypes.ContainsKey(table.Name))
                    {
                        table.MappedType = mappedTypes[table.Name];
                    }
                }

                File.WriteAllText(optionsPath, options.Write(), Encoding.UTF8);

                await project.AddExistingFilesAsync(new List<string> { optionsPath }.ToArray());
            }

            if (renamingOptions?.Item1 != null && !File.Exists(renamingOptions.Item2 + ".ignore") && renamingOptions.Item1.Count > 0)
            {
                if (File.Exists(renamingOptions.Item2) && File.GetAttributes(renamingOptions.Item2).HasFlag(FileAttributes.ReadOnly))
                {
                    VSHelper.ShowError($"Unable to save renaming options, the file is readonly: {renamingOptions.Item2}");
                    return;
                }

                File.WriteAllText(renamingOptions.Item2, CustomNameOptionsExtensions.Write(renamingOptions.Item1), Encoding.UTF8);
                await project.AddExistingFilesAsync(new List<string> { renamingOptions.Item2 }.ToArray());
            }
        }
    }
}