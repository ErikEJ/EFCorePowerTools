using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.BLL;
using EFCorePowerTools.Common.BLL;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.DAL;
using EFCorePowerTools.Dialogs;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Handlers;
using EFCorePowerTools.Handlers.Compare;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.ItemWizard;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using EFCorePowerTools.Options;
using EFCorePowerTools.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "2.6", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(GuidList.guidDbContextPackagePkgString)]
    [ProvideOptionPage(typeof(OptionsProvider.AdvancedOptions), "EF Core Power Tools", "General", 100, 101, true)]
    [ProvideProfile(typeof(OptionsProvider.AdvancedOptions), "EF Core Power Tools", "General", 100, 101, true)]
    [ProvideAutoLoad(UIContextGuid, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideUIContextRule(
        UIContextGuid,
        name: "Auto load based on rules",
        expression: "CSharpConfig & (SingleProject | MultipleProjects) ",
        termNames: new[] { "CSharpConfig", "SingleProject", "MultipleProjects" },
        termValues: new[] { "ActiveProjectCapability:CSharp & CPS & !MSBuild.Sdk.SqlProj.BuildTSqlScript", VSConstants.UICONTEXT.SolutionHasSingleProject_string, VSConstants.UICONTEXT.SolutionHasMultipleProjects_string })]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class EFCorePowerToolsPackage : AsyncPackage
    {
        public const string UIContextGuid = "BB60393B-FCF6-4807-AA92-B7C1019AA827";

        private readonly ReverseEngineerHandler reverseEngineerHandler;
        private readonly ModelAnalyzerHandler modelAnalyzerHandler;
        private readonly AboutHandler aboutHandler;
        private readonly DgmlNugetHandler dgmlNugetHandler;
        private readonly CompareHandler compareHandler;
        private readonly DatabaseDiagramHandler databaseDiagramHandler;
        private readonly DacpacAnalyzerHandler dacpacAnalyzerHandler;
        private IServiceProvider extensionServices;

        public EFCorePowerToolsPackage()
        {
            reverseEngineerHandler = new ReverseEngineerHandler(this);
            modelAnalyzerHandler = new ModelAnalyzerHandler(this);
            aboutHandler = new AboutHandler(this);
            dgmlNugetHandler = new DgmlNugetHandler();
            compareHandler = new CompareHandler(this);
            databaseDiagramHandler = new DatabaseDiagramHandler(this);
            dacpacAnalyzerHandler = new DacpacAnalyzerHandler(this);
        }

        internal EnvDTE80.DTE2 Dte2()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
        }

        internal void LogError(List<string> statusMessages, Exception exception)
        {
            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                // Switch to main thread
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (exception != null)
                {
                    Telemetry.TrackException(exception);
                }

                var messageBuilder = new StringBuilder();

                foreach (var error in statusMessages)
                {
                    messageBuilder.AppendLine(error);
                }

                if (exception != null)
                {
                    await VS.StatusBar.ShowMessageAsync(SharedLocale.AnErrorOccurred);
                    await exception.Demystify().LogAsync(messageBuilder.ToString());
                }
                else
                {
                    await exception.LogAsync(messageBuilder.ToString());
                }
            });
        }

        internal TView GetView<TView>()
            where TView : IView
        {
            return extensionServices.GetService<TView>();
        }

        internal async Task<Version> VisualStudioVersionAsync()
        {
            return await VS.Shell.GetVsVersionAsync();
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            try
            {
                await base.InitializeAsync(cancellationToken, progress);

                PackageManager.Package = this;

                var oleMenuCommandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (oleMenuCommandService != null)
                {
                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidDgmlBuild)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidReverseEngineerCodeFirst)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidReverseEngineerCodeFirstRefresh)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidT4Drop)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidAbout)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidDgmlNuget)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidSqlBuild)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidDebugViewBuild)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidDbCompare)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnSqlProjectContextMenuInvokeHandler,
                        null,
                        OnSqlProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidSqlprojContext,
                            (int)PkgCmdIDList.cmdidSqlprojCreate)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnSqlProjectContextMenuInvokeHandler,
                        null,
                        OnSqlProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidSqlprojAnalyze)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnServerExplorerDatabaseMenuInvokeHandler,
                        null,
                        OnServerExplorerDatabaseBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidServerExplorerMenu,
                            (int)PkgCmdIDList.cmdidServerExplorerDiagram)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnServerExplorerDatabaseMenuInvokeHandler,
                        null,
                        OnServerExplorerDatabaseBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidServerExplorerMenu,
                            (int)PkgCmdIDList.cmdidServerExplorerReverseEngineer)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnServerExplorerDatabaseMenuInvokeHandler,
                        null,
                        OnServerExplorerDatabaseBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidServerExplorerMenu,
                            (int)PkgCmdIDList.cmdidServerExplorerAnalyze)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidDbDgml)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidOptions)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnProjectContextMenuInvokeHandler,
                        null,
                        OnProjectMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidDbContextPackageCmdSet,
                            (int)PkgCmdIDList.cmdidDbErDiagram)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnReverseEngineerConfigFileMenuInvokeHandler,
                        null,
                        OnReverseEngineerConfigFileMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidReverseEngineerMenu,
                            (int)PkgCmdIDList.cmdidReverseEngineerEdit)));

                    oleMenuCommandService.AddCommand(new OleMenuCommand(
                        OnReverseEngineerConfigFileMenuInvokeHandler,
                        null,
                        OnReverseEngineerConfigFileMenuBeforeQueryStatus,
                        new CommandID(
                            GuidList.GuidReverseEngineerMenu,
                            (int)PkgCmdIDList.cmdidReverseEngineerRefresh)));
                }

                typeof(Microsoft.Xaml.Behaviors.Behavior).ToString();
                typeof(Xceed.Wpf.Toolkit.SplitButton).ToString();

                extensionServices = CreateServiceProvider();

                if (AdvancedOptions.Instance.ParticipateInTelemetry)
                {
                    Telemetry.Initialize(
                        FileVersionInfo.GetVersionInfo(typeof(EFCorePowerToolsPackage).Assembly.Location).FileVersion,
                        (await VisualStudioVersionAsync()).ToString(),
                        "00dac4de-337c-4fed-a835-70db30078b2a");
                }

                Telemetry.TrackEvent("Platform: Visual Studio " + (await VisualStudioVersionAsync()).ToString(1));
            }
            catch (Exception ex)
            {
                LogError(new List<string>(), ex);
            }
        }

        private static bool IsConfigFile(string itemName)
        {
            return itemName != null
                && itemName.StartsWith("efpt.", StringComparison.OrdinalIgnoreCase)
                && itemName.EndsWith(".config.json", StringComparison.OrdinalIgnoreCase);
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OnReverseEngineerConfigFileMenuBeforeQueryStatus(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null || (await VS.Solutions.GetActiveItemsAsync()).Count() != 1)
            {
                return;
            }

            switch ((uint)menuCommand.CommandID.ID)
            {
                case PkgCmdIDList.cmdidReverseEngineerEdit:
                    menuCommand.Text = ButtonLocale.cmdidReverseEngineerEdit;
                    break;
                case PkgCmdIDList.cmdidReverseEngineerRefresh:
                    menuCommand.Text = ButtonLocale.cmdidReverseEngineerRefresh;
                    break;
                default:
                    break;
            }

            menuCommand.Visible = false;

            var project = await VS.Solutions.GetActiveProjectAsync();

            if (project == null)
            {
                return;
            }

            var item = await VS.Solutions.GetActiveItemAsync();

            if (item == null)
            {
                return;
            }

            menuCommand.Visible = IsConfigFile(item.Text) && (await project.CanUseReverseEngineerAsync());
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OnProjectMenuBeforeQueryStatus(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            if (!(sender is OleMenuCommand menuCommand))
            {
                return;
            }

            menuCommand.Visible = false;

            switch ((uint)menuCommand.CommandID.ID)
            {
                case PkgCmdIDList.cmdidAbout:
                    menuCommand.Text = ButtonLocale.cmdidAbout;
                    break;
                case PkgCmdIDList.cmdidDbCompare:
                    menuCommand.Text = ButtonLocale.cmdidDbCompare;
                    break;
                case PkgCmdIDList.cmdidDbErDiagram:
                    menuCommand.Text = ButtonLocale.cmdidDbErDiagram;
                    break;
                case PkgCmdIDList.cmdidDbDgml:
                    menuCommand.Text = ButtonLocale.cmdidDbDgml;
                    break;
                case PkgCmdIDList.cmdidDebugViewBuild:
                    menuCommand.Text = ButtonLocale.cmdidDebugViewBuild;
                    break;
                case PkgCmdIDList.cmdidDgmlBuild:
                    menuCommand.Text = ButtonLocale.cmdidDgmlBuild;
                    break;
                case PkgCmdIDList.cmdidDgmlNuget:
                    menuCommand.Text = ButtonLocale.cmdidDgmlNuget;
                    break;
                case PkgCmdIDList.cmdidOptions:
                    menuCommand.Text = ButtonLocale.cmdidOptions;
                    break;
                case PkgCmdIDList.cmdidReverseEngineerCodeFirst:
                    menuCommand.Text = ButtonLocale.cmdidReverseEngineerCodeFirst;
                    break;
                case PkgCmdIDList.cmdidReverseEngineerCodeFirstRefresh:
                    menuCommand.Text = ButtonLocale.cmdidReverseEngineerCodeFirstRefresh;
                    break;
                case PkgCmdIDList.cmdidSqlBuild:
                    menuCommand.Text = ButtonLocale.cmdidSqlBuild;
                    break;
                case PkgCmdIDList.cmdidT4Drop:
                    menuCommand.Text = ButtonLocale.cmdidT4Drop;
                    break;

                default:
                    break;
            }

            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidAbout
                || menuCommand.CommandID.ID == PkgCmdIDList.cmdidOptions
                || menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbDgml
                || menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbErDiagram)
            {
                menuCommand.Visible = true;
                return;
            }

            var project = await VS.Solutions.GetActiveProjectAsync();

            if (project == null)
            {
                return;
            }

            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerCodeFirst
                || menuCommand.CommandID.ID == PkgCmdIDList.cmdidT4Drop)
            {
                menuCommand.Visible = await project.CanUseReverseEngineerAsync();
                return;
            }

            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerCodeFirstRefresh)
            {
                await JoinableTaskFactory.SwitchToMainThreadAsync();

                menuCommand.Visible = await project.CanUseReverseEngineerAsync()
                    && project.GetConfigFiles().Count == 1;
                return;
            }

            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDgmlBuild ||
                menuCommand.CommandID.ID == PkgCmdIDList.cmdidDgmlNuget ||
                menuCommand.CommandID.ID == PkgCmdIDList.cmdidDebugViewBuild ||
                menuCommand.CommandID.ID == PkgCmdIDList.cmdidSqlBuild ||
                menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbCompare)
            {
                menuCommand.Visible = await project.IsNet60OrHigherAsync()
                    && await project.IsInstalledAsync(new NuGetPackage { PackageId = "Microsoft.EntityFrameworkCore" });
            }
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OnSqlProjectMenuBeforeQueryStatus(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            var menuCommand = sender as OleMenuCommand;

            if (menuCommand == null)
            {
                return;
            }

            switch ((uint)menuCommand.CommandID.ID)
            {
                case PkgCmdIDList.cmdidSqlprojCreate:
                    menuCommand.Text = ButtonLocale.cmdidSqlprojCreate;
                    break;
                case PkgCmdIDList.cmdidSqlprojAnalyze:
                    menuCommand.Text = ButtonLocale.cmdidSqlprojAnalyze;
                    break;
                default:
                    break;
            }

            menuCommand.Visible = false;

            var project = await VS.Solutions.GetActiveProjectAsync();

            if (project == null)
            {
                return;
            }

            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidSqlprojCreate)
            {
                if (!project.FullPath.EndsWith(".sqlproj", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                var candidateProjects = (await VS.Solutions.GetAllProjectsAsync())
                    .Where(p => p.IsCSharpProject())
                    .Where(p => p.Children.All(c => !c.Text.Equals("efpt.config.json", StringComparison.OrdinalIgnoreCase))).ToList();

                if (!candidateProjects.Any())
                {
                    return;
                }

                menuCommand.Visible = true;
            }

            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidSqlprojAnalyze
                && project.IsSqlDatabaseProject())
            {
                menuCommand.Visible = true;
            }
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OnServerExplorerDatabaseBeforeQueryStatus(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            var menuCommand = sender as OleMenuCommand;

            if (menuCommand == null)
            {
                return;
            }

            switch ((uint)menuCommand.CommandID.ID)
            {
                case PkgCmdIDList.cmdidServerExplorerReverseEngineer:
                    menuCommand.Text = ButtonLocale.cmdidServerExplorerReverseEngineer;
                    break;
                case PkgCmdIDList.cmdidServerExplorerDiagram:
                    menuCommand.Text = ButtonLocale.cmdidServerExplorerDiagram;
                    break;
                case PkgCmdIDList.cmdidServerExplorerAnalyze:
                    menuCommand.Text = ButtonLocale.cmdidServerExplorerAnalyze;
                    break;
                default:
                    break;
            }

            menuCommand.Visible = false;

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var uih = Dte2().ToolWindows.GetToolWindow(EnvDTE.Constants.vsWindowKindServerExplorer) as EnvDTE.UIHierarchy;
            var selectedItems = (Array)uih.SelectedItems;

            if (selectedItems != null)
            {
                var connectionName = ((EnvDTE.UIHierarchyItem)selectedItems.GetValue(0)).Name;

                if (string.IsNullOrEmpty(connectionName))
                {
                    return;
                }

                var dataConnectionsService = await VS.GetServiceAsync<IVsDataExplorerConnectionManager, IVsDataExplorerConnectionManager>();
                if (dataConnectionsService.Connections.TryGetValue(connectionName, out IVsDataExplorerConnection explorerConnection))
                {
                    var connection = explorerConnection.Connection;

                    if (connection == null)
                    {
                        return;
                    }

                    if (VsDataHelper.SupportedProviders.Contains(connection.Provider))
                    {
                        if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidServerExplorerReverseEngineer)
                        {
                            if ((await VS.Solutions.GetActiveItemsAsync()).Count() != 1)
                            {
                                return;
                            }

                            var project = await VS.Solutions.GetActiveProjectAsync();
                            if (project == null)
                            {
                                return;
                            }

                            menuCommand.Visible = await project.CanUseReverseEngineerAsync();
                            return;
                        }

                        if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidServerExplorerDiagram)
                        {
                            menuCommand.Visible = true;
                            return;
                        }

                        if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidServerExplorerAnalyze)
                        {
                            menuCommand.Visible = VsDataHelper.SqlServerProviders.Contains(connection.Provider);
                        }
                    }
                }
            }
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OnReverseEngineerConfigFileMenuInvokeHandler(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var menuCommand = sender as MenuCommand;
                if (menuCommand == null || (await VS.Solutions.GetActiveItemsAsync()).Count() != 1)
                {
                    return;
                }

                var item = await VS.Solutions.GetActiveItemAsync();

                if (item == null)
                {
                    return;
                }

                if (!IsConfigFile(item.Text))
                {
                    return;
                }

                Project project = FindProject(item);
                if (project == null)
                {
                    return;
                }

                string filename = item.FullPath;

                if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerEdit)
                {
                    await reverseEngineerHandler.ReverseEngineerCodeFirstAsync(project, filename, false);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerRefresh)
                {
                    await reverseEngineerHandler.ReverseEngineerCodeFirstAsync(project, filename, true);
                }
            }
            catch (Exception ex)
            {
                LogError(new List<string>(), ex);
            }
        }

        private Project FindProject(SolutionItem item)
        {
            var parent = item.Parent;
            while (parent != null && !(parent is Project))
            {
                parent = parent.Parent;
            }

            return parent as Project;
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OnProjectContextMenuInvokeHandler(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var menuCommand = sender as MenuCommand;
                if (menuCommand == null || (await VS.Solutions.GetActiveItemsAsync()).Count() != 1)
                {
                    return;
                }

                var project = await VS.Solutions.GetActiveProjectAsync();
                if (project == null)
                {
                    return;
                }

                string path = null;

                if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDgmlBuild ||
                    menuCommand.CommandID.ID == PkgCmdIDList.cmdidDebugViewBuild ||
                    menuCommand.CommandID.ID == PkgCmdIDList.cmdidSqlBuild ||
                    menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbCompare)
                {
                    path = await LocateProjectAssemblyPathAsync(project);
                    if (path == null)
                    {
                        return;
                    }
                }

                if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerCodeFirst)
                {
                    await reverseEngineerHandler.ReverseEngineerCodeFirstAsync();
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerCodeFirstRefresh)
                {
                    var configs = project.GetConfigFiles();
                    if (configs.Count != 1)
                    {
                        return;
                    }

                    await reverseEngineerHandler.ReverseEngineerCodeFirstAsync(project, configs[0], true);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDgmlNuget)
                {
                    await dgmlNugetHandler.InstallDgmlNugetAsync(project);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidT4Drop)
                {
                    dgmlNugetHandler.UnzipT4Templates(project);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDgmlBuild)
                {
                    await modelAnalyzerHandler.GenerateAsync(path, project, GenerationType.Dgml);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidSqlBuild)
                {
                    await modelAnalyzerHandler.GenerateAsync(path, project, GenerationType.Ddl);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDebugViewBuild)
                {
                    await modelAnalyzerHandler.GenerateAsync(path, project, GenerationType.DebugView);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidAbout)
                {
                    aboutHandler.ShowDialog();
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidOptions)
                {
                    ShowOptionPage(typeof(OptionsProvider.AdvancedOptions));
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbCompare)
                {
                    await compareHandler.HandleComparisonAsync(path, project);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbDgml
                    || menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbErDiagram)
                {
                    string connectionName = null;
                    if (project.IsSqlDatabaseProject())
                    {
                        connectionName = project.FullPath;

                        if (project.IsMsBuildSqlProjProject())
                        {
                            connectionName = await project.GetOutPutAssemblyPathAsync();
                        }
                    }

                    await databaseDiagramHandler.GenerateAsync(
                        connectionName,
                        generateErDiagram: menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbErDiagram);
                }
            }
            catch (Exception ex)
            {
                LogError(new List<string>(), ex);
            }
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OnSqlProjectContextMenuInvokeHandler(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var menuCommand = sender as MenuCommand;
                if (menuCommand == null || (await VS.Solutions.GetActiveItemsAsync()).Count() != 1)
                {
                    return;
                }

                var sqlproject = await VS.Solutions.GetActiveProjectAsync();
                if (sqlproject == null)
                {
                    return;
                }

                if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidSqlprojCreate)
                {
                    var candidateProjects = (await VS.Solutions.GetAllProjectsAsync())
                        .Where(p => p.IsCSharpProject())
                        .Where(p => p.Children.All(c => !c.Text.Equals("efpt.config.json", StringComparison.OrdinalIgnoreCase))).ToList();

                    if (!candidateProjects.Any())
                    {
                        return;
                    }

                    var result = await reverseEngineerHandler.DropSqlprojOptionsAsync(candidateProjects, sqlproject.FullPath);

                    if (result.OptionsPath != null && result.Project != null)
                    {
                        await reverseEngineerHandler.ReverseEngineerCodeFirstAsync(result.Project, result.OptionsPath, false, true);
                    }
                    else if (result.OptionsPath == null && result.Project != null)
                    {
                        await VS.MessageBox.ShowAsync(
                            $"Project '{result.Project.Name}' already contains an EF Core Power Tools config file (efpt.config.json).",
                            icon: Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_WARNING,
                            buttons: Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK);
                    }
                }

                if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidSqlprojAnalyze)
                {
                    await dacpacAnalyzerHandler.GenerateAsync(sqlproject.FullPath, isConnectionString: false);
                }
            }
            catch (Exception ex)
            {
                LogError(new List<string>(), ex);
            }
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OnServerExplorerDatabaseMenuInvokeHandler(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var menuCommand = sender as MenuCommand;
                if (menuCommand == null)
                {
                    return;
                }

                var uih = Dte2().ToolWindows.GetToolWindow(EnvDTE.Constants.vsWindowKindServerExplorer) as EnvDTE.UIHierarchy;
                var selectedItems = (Array)uih.SelectedItems;

                if (selectedItems != null)
                {
                    var connectionName = ((EnvDTE.UIHierarchyItem)selectedItems.GetValue(0)).Name;

                    if (string.IsNullOrEmpty(connectionName))
                    {
                        return;
                    }

                    var dataConnectionsService = await VS.GetServiceAsync<IVsDataExplorerConnectionManager, IVsDataExplorerConnectionManager>();
                    if (dataConnectionsService.Connections.TryGetValue(connectionName, out IVsDataExplorerConnection explorerConnection))
                    {
                        var connection = explorerConnection.Connection;

                        if (connection == null)
                        {
                            return;
                        }

                        if (VsDataHelper.SupportedProviders.Contains(connection.Provider))
                        {
                            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidServerExplorerReverseEngineer)
                            {
                                if ((await VS.Solutions.GetActiveItemsAsync()).Count() != 1)
                                {
                                    return;
                                }

                                var project = await VS.Solutions.GetActiveProjectAsync();
                                if (project == null)
                                {
                                    return;
                                }

                                await reverseEngineerHandler.ReverseEngineerCodeFirstAsync(connectionName);
                            }

                            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidServerExplorerDiagram)
                            {
                                await databaseDiagramHandler.GenerateAsync(connectionName, generateErDiagram: true);
                            }

                            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidServerExplorerAnalyze)
                            {
                                await dacpacAnalyzerHandler.GenerateAsync(DataProtection.DecryptString(connection.EncryptedConnectionString), isConnectionString: true);
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(new List<string>(), ex);
            }
        }

        private async Task<string> LocateProjectAssemblyPathAsync(Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (!await VS.Build.BuildProjectAsync(project))
                {
                    await VS.StatusBar.ShowMessageAsync(SharedLocale.BuildFailed);

                    return null;
                }

                var path = await project.GetOutPutAssemblyPathAsync();
                if (path != null)
                {
                    return path;
                }

                await VS.StatusBar.ShowMessageAsync(SharedLocale.UnableToLocateProjectAssembly);
            }
            catch (Exception ex)
            {
                LogError(new List<string>(), ex);
            }

            return null;
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            // Register models
            services.AddSingleton<AboutExtensionModel>();

            // Register views
            services.AddTransient<IAboutDialog, AboutDialog>()
                    .AddTransient<IPickConfigDialog, PickConfigDialog>()
                    .AddTransient<IPickProjectDialog, PickProjectDialog>()
                    .AddTransient<IPickServerDatabaseDialog, PickServerDatabaseDialog>()
                    .AddTransient<IPickTablesDialog, PickTablesDialog>()
                    .AddTransient<IModelingOptionsDialog, EfCoreModelDialog>()
                    .AddTransient<IPickSchemasDialog, PickSchemasDialog>()
                    .AddTransient<IPickConnectionDialog, ConnectionDialog>()
                    .AddTransient<IAdvancedModelingOptionsDialog, AdvancedModelingOptionsDialog>()
                    .AddSingleton<Func<IPickSchemasDialog>>(sp => sp.GetService<IPickSchemasDialog>)
                    .AddSingleton<Func<IPickConnectionDialog>>(sp => sp.GetService<IPickConnectionDialog>)
                    .AddSingleton<Func<IAdvancedModelingOptionsDialog>>(sp => sp.GetService<IAdvancedModelingOptionsDialog>)
                    .AddTransient<ICompareOptionsDialog, CompareOptionsDialog>()
                    .AddTransient<ICompareResultDialog, CompareResultDialog>();

            // Register view models
            services.AddTransient<IAboutViewModel, AboutViewModel>()
                    .AddTransient<IPickConfigViewModel, PickConfigViewModel>()
                    .AddTransient<IPickProjectViewModel, PickProjectViewModel>()
                    .AddTransient<IPickConnectionViewModel, PickConnectionViewModel>()
                    .AddTransient<IPickServerDatabaseViewModel, PickServerDatabaseViewModel>()
                    .AddTransient<IPickTablesViewModel, PickTablesViewModel>()
                    .AddSingleton<Func<ISchemaInformationViewModel>>(() => new SchemaInformationViewModel())
                    .AddSingleton<Func<ITableInformationViewModel>>(provider => () => new TableInformationViewModel(provider.GetService<IMessenger>()))
                    .AddSingleton<Func<IColumnInformationViewModel>>(provider => () => new ColumnInformationViewModel(provider.GetService<IMessenger>()))
                    .AddTransient<IModelingOptionsViewModel, ModelingOptionsViewModel>()
                    .AddTransient<IPickSchemasViewModel, PickSchemasViewModel>()
                    .AddTransient<IAdvancedModelingOptionsViewModel, AdvancedModelingOptionsViewModel>()
                    .AddTransient<IObjectTreeViewModel, ObjectTreeViewModel>()
                    .AddTransient<ICompareOptionsViewModel, CompareOptionsViewModel>()
                    .AddTransient<ICompareResultViewModel, CompareResultViewModel>();

            // Register BLL
            var messenger = new Messenger();
            messenger.Register<ShowMessageBoxMessage>(this, HandleShowMessageBoxMessage);

            services.AddSingleton<IExtensionVersionService, ExtensionVersionService>()
                    .AddSingleton<IMessenger>(messenger);

            // Register DAL
            services.AddTransient<IVisualStudioAccess, VisualStudioAccess>()
                    .AddSingleton<ITelemetryAccess, TelemetryAccess>()
                    .AddSingleton<IOperatingSystemAccess, OperatingSystemAccess>()
                    .AddSingleton<ICredentialStore, CredentialStore>()
                    .AddSingleton<IDotNetAccess, DotNetAccess>();

            return services.BuildServiceProvider();
        }

        private void HandleShowMessageBoxMessage(ShowMessageBoxMessage msg)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            VSHelper.ShowMessage(msg.Content);
        }
    }
}
