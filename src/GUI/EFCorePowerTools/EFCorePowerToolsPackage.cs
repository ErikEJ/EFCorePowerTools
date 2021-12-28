using Community.VisualStudio.Toolkit;
using EFCorePowerTools.BLL;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.DAL;
using EFCorePowerTools.Dialogs;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Handlers;
using EFCorePowerTools.Handlers.Compare;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using EFCorePowerTools.Shared.BLL;
using EFCorePowerTools.Shared.DAL;
using EFCorePowerTools.Shared.Models;
using EFCorePowerTools.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EFCorePowerTools
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "2.5", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(GuidList.guidDbContextPackagePkgString)]
    [ProvideOptionPage(typeof(OptionsPageGeneral), "EF Core Power Tools", "General", 100, 101, true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // ReSharper disable once InconsistentNaming
    public sealed class EFCorePowerToolsPackage : AsyncPackage
    {
        private readonly ReverseEngineerHandler _reverseEngineerHandler;
        private readonly ModelAnalyzerHandler _modelAnalyzerHandler;
        private readonly AboutHandler _aboutHandler;
        private readonly DgmlNugetHandler _dgmlNugetHandler;
        private readonly MigrationsHandler _migrationsHandler;
        private readonly CompareHandler _compareHandler;
        private IServiceProvider _extensionServices;

        public EFCorePowerToolsPackage()
        {
            _reverseEngineerHandler = new ReverseEngineerHandler(this);
            _modelAnalyzerHandler = new ModelAnalyzerHandler(this);
            _aboutHandler = new AboutHandler(this);
            _dgmlNugetHandler = new DgmlNugetHandler(this);
            _migrationsHandler = new MigrationsHandler(this);
            _compareHandler = new CompareHandler(this);
        }

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            try
            {
                await base.InitializeAsync(cancellationToken, progress);

                var oleMenuCommandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (oleMenuCommandService != null)
                {
                    var menuCommandId3 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                        (int)PkgCmdIDList.cmdidDgmlBuild);
                    var menuItem3 = new OleMenuCommand(OnProjectContextMenuInvokeHandler, null,
                        OnProjectMenuBeforeQueryStatus, menuCommandId3);
                    oleMenuCommandService.AddCommand(menuItem3);

                    var menuCommandId5 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                        (int)PkgCmdIDList.cmdidReverseEngineerCodeFirst);
                    var menuItem5 = new OleMenuCommand(OnProjectContextMenuInvokeHandler, null,
                        OnProjectMenuBeforeQueryStatus, menuCommandId5);
                    oleMenuCommandService.AddCommand(menuItem5);

                    var menuCommandId7 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                        (int)PkgCmdIDList.cmdidAbout);
                    var menuItem7 = new OleMenuCommand(OnProjectContextMenuInvokeHandler, null,
                        OnProjectMenuBeforeQueryStatus, menuCommandId7);
                    oleMenuCommandService.AddCommand(menuItem7);

                    var menuCommandId8 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                        (int)PkgCmdIDList.cmdidDgmlNuget);
                    var menuItem8 = new OleMenuCommand(OnProjectContextMenuInvokeHandler, null,
                        OnProjectMenuBeforeQueryStatus, menuCommandId8);
                    oleMenuCommandService.AddCommand(menuItem8);

                    var menuCommandId9 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                       (int)PkgCmdIDList.cmdidSqlBuild);
                    var menuItem9 = new OleMenuCommand(OnProjectContextMenuInvokeHandler, null,
                        OnProjectMenuBeforeQueryStatus, menuCommandId9);
                    oleMenuCommandService.AddCommand(menuItem9);

                    var menuCommandId10 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                        (int)PkgCmdIDList.cmdidDebugViewBuild);
                    var menuItem10 = new OleMenuCommand(OnProjectContextMenuInvokeHandler, null,
                        OnProjectMenuBeforeQueryStatus, menuCommandId10);
                    oleMenuCommandService.AddCommand(menuItem10);

                    var menuCommandId11 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                        (int)PkgCmdIDList.cmdidMigrationStatus);
                    var menuItem11 = new OleMenuCommand(OnProjectContextMenuInvokeHandler, null,
                        OnProjectMenuBeforeQueryStatus, menuCommandId11);
                    oleMenuCommandService.AddCommand(menuItem11);

                    var menuCommandId12 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                        (int)PkgCmdIDList.cmdidDbCompare);
                    var menuItem12 = new OleMenuCommand(OnProjectContextMenuInvokeHandler, null,
                        OnProjectMenuBeforeQueryStatus, menuCommandId12);
                    oleMenuCommandService.AddCommand(menuItem12);

                    var menuCommandId1101 = new CommandID(GuidList.guidReverseEngineerMenu,
                        (int)PkgCmdIDList.cmdidReverseEngineerEdit);
                    var menuItem251 = new OleMenuCommand(OnReverseEngineerConfigFileMenuInvokeHandler, null,
                        OnReverseEngineerConfigFileMenuBeforeQueryStatus, menuCommandId1101);
                    oleMenuCommandService.AddCommand(menuItem251);

                    var menuCommandId1102 = new CommandID(GuidList.guidReverseEngineerMenu,
                        (int)PkgCmdIDList.cmdidReverseEngineerRefresh);
                    var menuItem252 = new OleMenuCommand(OnReverseEngineerConfigFileMenuInvokeHandler, null,
                        OnReverseEngineerConfigFileMenuBeforeQueryStatus, menuCommandId1102);
                    oleMenuCommandService.AddCommand(menuItem252);

                }
                typeof(Microsoft.Xaml.Behaviors.Behavior).ToString();
                typeof(Xceed.Wpf.Toolkit.SplitButton).ToString();

                _extensionServices = CreateServiceProvider();

                Telemetry.Enabled = Properties.Settings.Default.ParticipateInTelemetry;
                if (Telemetry.Enabled)
                {
                    Telemetry.Initialize(
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
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

        private async Task<Version> VisualStudioVersionAsync()
        { 
            return await VS.Shell.GetVsVersionAsync();
        }

        private async void OnReverseEngineerConfigFileMenuBeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as MenuCommand;
            if (menuCommand == null || (await VS.Solutions.GetActiveItemsAsync()).Count() != 1)
            {
                return; 
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

            menuCommand.Visible = IsConfigFile(item.Text) && project.IsCSharpProject();
        }

        private static bool IsConfigFile(string itemName)
        {
            return itemName != null &&
                itemName.StartsWith("efpt.", StringComparison.OrdinalIgnoreCase) &&
                itemName.EndsWith(".config.json", StringComparison.OrdinalIgnoreCase);
        }

        private async void OnProjectMenuBeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as MenuCommand;

            if (menuCommand == null)
            {
                return;
            }

            menuCommand.Visible = false;

            var project = await VS.Solutions.GetActiveProjectAsync();

            if (project == null)
            {
                return;
            }

            menuCommand.Visible = project.IsCSharpProject();
        }

        private async void OnReverseEngineerConfigFileMenuInvokeHandler(object sender, EventArgs e)
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
                    await _reverseEngineerHandler.ReverseEngineerCodeFirstAsync(project, filename, false);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerRefresh)
                {
                    await _reverseEngineerHandler.ReverseEngineerCodeFirstAsync(project, filename, true);
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

        private async void OnProjectContextMenuInvokeHandler(object sender, EventArgs e)
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
                    menuCommand.CommandID.ID == PkgCmdIDList.cmdidMigrationStatus ||
                    menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbCompare)
                {
                    path = await LocateProjectAssemblyPathAsync(project);
                    if (path == null) return;
                }

                if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerCodeFirst)
                {
                    await _reverseEngineerHandler.ReverseEngineerCodeFirstAsync(project);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDgmlNuget)
                {
                    await _dgmlNugetHandler.InstallDgmlNugetAsync(project);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDgmlBuild)
                {
                    await _modelAnalyzerHandler.GenerateAsync(path, project, GenerationType.Dgml);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidSqlBuild)
                {
                    await _modelAnalyzerHandler.GenerateAsync(path, project, GenerationType.Ddl);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDebugViewBuild)
                {
                    await _modelAnalyzerHandler.GenerateAsync(path, project, GenerationType.DebugView);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidMigrationStatus)
                {
                    await _migrationsHandler.ManageMigrationsAsync(path, project);
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidAbout)
                {
                    _aboutHandler.ShowDialog();
                }
                else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDbCompare)
                {
                    await _compareHandler.HandleComparisonAsync(path, project);
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
                    .AddTransient<IPickServerDatabaseDialog, PickServerDatabaseDialog>()
                    .AddTransient<IPickTablesDialog, PickTablesDialog>()
                    .AddTransient<IModelingOptionsDialog, EfCoreModelDialog>()
                    .AddTransient<IMigrationOptionsDialog, EfCoreMigrationsDialog>()
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
                    .AddTransient<IPickConnectionViewModel, PickConnectionViewModel>()
                    .AddTransient<IPickServerDatabaseViewModel, PickServerDatabaseViewModel>()
                    .AddTransient<IPickTablesViewModel, PickTablesViewModel>()
                    .AddSingleton<Func<ISchemaInformationViewModel>>(() => new SchemaInformationViewModel())
                    .AddSingleton<Func<ITableInformationViewModel>>(provider => () => new TableInformationViewModel(provider.GetService<IMessenger>()))
                    .AddSingleton<Func<IColumnInformationViewModel>>(provider => () => new ColumnInformationViewModel(provider.GetService<IMessenger>()))
                    .AddTransient<IModelingOptionsViewModel, ModelingOptionsViewModel>()
                    .AddTransient<IMigrationOptionsViewModel, MigrationOptionsViewModel>()
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
            services.AddTransient<IVisualStudioAccess, VisualStudioAccess>(provider => new VisualStudioAccess(this))
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

        internal void LogError(List<string> statusMessages, Exception exception)
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                // Switch to main thread
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (exception != null)
                {
                    Telemetry.TrackException(exception);
                }

                await VS.StatusBar.ShowMessageAsync(SharedLocale.AnErrorOccurred);

                var messageBuilder = new StringBuilder();

                foreach (var error in statusMessages)
                {
                    messageBuilder.AppendLine(error);
                }

                if (exception != null)
                {
                    await exception.Demystify().LogAsync(messageBuilder.ToString());
                }
                else
                {
                    await exception.LogAsync(messageBuilder.ToString());
                }
            });
        }

        internal T GetService<T>()
            where T : class
        {
            return (T)GetService(typeof(T));
        }

        internal TView GetView<TView>()
            where TView : IView
        {
            return _extensionServices.GetService<TView>();
        }
    }
}
