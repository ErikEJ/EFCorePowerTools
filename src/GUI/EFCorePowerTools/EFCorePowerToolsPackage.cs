using EFCorePowerTools.BLL;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.DAL;
using EFCorePowerTools.Dialogs;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Handlers;
using EFCorePowerTools.Messages;
using EFCorePowerTools.Shared.BLL;
using EFCorePowerTools.Shared.DAL;
using EFCorePowerTools.Shared.Models;
using EFCorePowerTools.ViewModels;
using EnvDTE;
using EnvDTE80;
using ErikEJ.SqlCeToolbox.Helpers;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace EFCorePowerTools
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [SqliteProviderRegistration]
    [InstalledProductRegistration("#110", "#112", "2.5", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(GuidList.guidDbContextPackagePkgString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_string, PackageAutoLoadFlags.BackgroundLoad)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // ReSharper disable once InconsistentNaming
    public sealed class EFCorePowerToolsPackage : AsyncPackage
    {
        private readonly ReverseEngineerHandler _reverseEngineerHandler;
        private readonly ModelAnalyzerHandler _modelAnalyzerHandler;
        private readonly AboutHandler _aboutHandler;
        private readonly DgmlNugetHandler _dgmlNugetHandler;
        private readonly ServerDgmlHandler _serverDgmlHandler;
        private readonly MigrationsHandler _migrationsHandler;
        private readonly IServiceProvider _extensionServices;
        private DTE2 _dte2;

        public EFCorePowerToolsPackage()
        {
            _reverseEngineerHandler = new ReverseEngineerHandler(this);
            _modelAnalyzerHandler = new ModelAnalyzerHandler(this);
            _aboutHandler = new AboutHandler(this);
            _dgmlNugetHandler = new DgmlNugetHandler(this);
            _serverDgmlHandler = new ServerDgmlHandler(this);
            _migrationsHandler = new MigrationsHandler(this);
            _extensionServices = CreateServiceProvider();
        }

        internal DTE2 Dte2 => _dte2;

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            _dte2 = await GetServiceAsync(typeof(DTE)) as DTE2;

            if (_dte2 == null)
            {
                return;
            }

            var oleMenuCommandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (oleMenuCommandService != null)
            {
                var menuCommandId3 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                    (int)PkgCmdIDList.cmdidDgmlBuild);
                var menuItem3 = new OleMenuCommand(async (s, e) => await OnProjectContextMenuInvokeHandlerAsync(s, e), null,
                    async (s, e) => await OnProjectMenuBeforeQueryStatusAsync(s, e), menuCommandId3);
                oleMenuCommandService.AddCommand(menuItem3);

                var menuCommandId4 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                    (int)PkgCmdIDList.cmdidReverseEngineerDgml);
                var menuItem4 = new OleMenuCommand(async (s, e) => await OnProjectContextMenuInvokeHandlerAsync(s, e), null,
                    async (s, e) => await OnProjectMenuBeforeQueryStatusAsync(s, e), menuCommandId4);
                oleMenuCommandService.AddCommand(menuItem4);

                var menuCommandId5 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                    (int)PkgCmdIDList.cmdidReverseEngineerCodeFirst);
                var menuItem5 = new OleMenuCommand(async (s, e) => await OnProjectContextMenuInvokeHandlerAsync(s, e), null,
                    async (s, e) => await OnProjectMenuBeforeQueryStatusAsync(s, e), menuCommandId5);
                oleMenuCommandService.AddCommand(menuItem5);

                var menuCommandId7 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                    (int)PkgCmdIDList.cmdidAbout);
                var menuItem7 = new OleMenuCommand(async (s, e) => await OnProjectContextMenuInvokeHandlerAsync(s, e), null,
                    async (s, e) => await OnProjectMenuBeforeQueryStatusAsync(s, e), menuCommandId7);
                oleMenuCommandService.AddCommand(menuItem7);

                var menuCommandId8 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                    (int)PkgCmdIDList.cmdidDgmlNuget);
                var menuItem8 = new OleMenuCommand(async (s, e) => await OnProjectContextMenuInvokeHandlerAsync(s, e), null,
                    async (s, e) => await OnProjectMenuBeforeQueryStatusAsync(s, e), menuCommandId8);
                oleMenuCommandService.AddCommand(menuItem8);

                var menuCommandId9 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                   (int)PkgCmdIDList.cmdidSqlBuild);
                var menuItem9 = new OleMenuCommand(async (s, e) => await OnProjectContextMenuInvokeHandlerAsync(s, e), null,
                    async (s, e) => await OnProjectMenuBeforeQueryStatusAsync(s, e), menuCommandId9);
                oleMenuCommandService.AddCommand(menuItem9);

                var menuCommandId10 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                    (int)PkgCmdIDList.cmdidDebugViewBuild);
                var menuItem10 = new OleMenuCommand(async (s, e) => await OnProjectContextMenuInvokeHandlerAsync(s, e), null,
                    async (s, e) => await OnProjectMenuBeforeQueryStatusAsync(s, e), menuCommandId10);
                oleMenuCommandService.AddCommand(menuItem10);

                var menuCommandId11 = new CommandID(GuidList.guidDbContextPackageCmdSet,
                    (int)PkgCmdIDList.cmdidMigrationStatus);
                var menuItem11 = new OleMenuCommand(async (s, e) => await OnProjectContextMenuInvokeHandlerAsync(s, e), null,
                    async (s, e) => await OnProjectMenuBeforeQueryStatusAsync(s, e), menuCommandId11);
                oleMenuCommandService.AddCommand(menuItem11);
            }
            typeof(Microsoft.Xaml.Behaviors.Behavior).ToString();
            typeof(Microsoft.VisualStudio.ProjectSystem.ProjectCapabilities).ToString();

            //Boot Telemetry
            Telemetry.Enabled = false;
            if (Telemetry.Enabled)
            {
                Telemetry.Initialize(Dte2,
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    VisualStudioVersion.ToString(),
                    "00dac4de-337c-4fed-a835-70db30078b2a");
            }
            Telemetry.TrackEvent("Platform: Visual Studio " + VisualStudioVersion.ToString(1));
        }

        private Version VisualStudioVersion => new Version(int.Parse(_dte2.Version.Split('.')[0], System.Globalization.CultureInfo.InvariantCulture), 0);

        private System.Threading.Tasks.Task OnProjectMenuBeforeQueryStatusAsync(object sender, EventArgs e)
        {
            var menuCommand = sender as MenuCommand;

            if (menuCommand == null)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }

            if (_dte2.SelectedItems.Count != 1)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }

            var project = _dte2.SelectedItems.Item(1).Project;

            if (project == null)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }

            menuCommand.Visible =
                project.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" ||
                project.Kind == "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"; // csproj
            
            return System.Threading.Tasks.Task.CompletedTask;
        }

        private async System.Threading.Tasks.Task OnProjectContextMenuInvokeHandlerAsync(object sender, EventArgs e)
        {
            var menuCommand = sender as MenuCommand;
            if (menuCommand == null || _dte2.SelectedItems.Count != 1)
            {
                return;
            }

            var project = _dte2.SelectedItems.Item(1).Project;
            if (project == null)
            {
                return;
            }
            string path = null;

            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidDgmlBuild ||
                menuCommand.CommandID.ID == PkgCmdIDList.cmdidDebugViewBuild ||
                menuCommand.CommandID.ID == PkgCmdIDList.cmdidSqlBuild ||
                menuCommand.CommandID.ID == PkgCmdIDList.cmdidMigrationStatus)
            {
                path = LocateProjectAssemblyPath(project);
                if (path == null) return;
            }

            if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerCodeFirst)
            {
                await _reverseEngineerHandler.ReverseEngineerCodeFirstAsync(project);
            }
            else if (menuCommand.CommandID.ID == PkgCmdIDList.cmdidReverseEngineerDgml)
            {
                _serverDgmlHandler.GenerateServerDgmlFiles();
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
        }

        private string LocateProjectAssemblyPath(Project project)
        {
            if (!project.TryBuild())
            {
                _dte2.StatusBar.Text = "Build failed. Unable to discover a DbContext class.";

                return null;
            }

            var path = project.GetOutPutAssemblyPath();
            if (path != null)
            {
                return path;
            }

            _dte2.StatusBar.Text = "Unable to locate project assembly.";

            return null;
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            // Register models
            services.AddSingleton<AboutExtensionModel>();

            // Register views
            services.AddTransient<IAboutDialog, AboutDialog>()
                    .AddTransient<IPickServerDatabaseDialog, PickServerDatabaseDialog>()
                    .AddTransient<IPickConfigDialog, PickConfigDialog>()
                    .AddTransient<IPickTablesDialog, PickTablesDialog>()
                    .AddTransient<IModelingOptionsDialog, EfCoreModelDialog>()
                    .AddTransient<IMigrationOptionsDialog, EfCoreMigrationsDialog>()
                    .AddTransient<IPickSchemasDialog, PickSchemasDialog>()
                    .AddTransient<IAdvancedModelingOptionsDialog, AdvancedModelingOptionsDialog>()
                    .AddSingleton<Func<IPickSchemasDialog>>(sp => sp.GetService<IPickSchemasDialog>)
                    .AddSingleton<Func<IAdvancedModelingOptionsDialog>>(sp => sp.GetService<IAdvancedModelingOptionsDialog>);

            // Register view models
            services.AddTransient<IAboutViewModel, AboutViewModel>()
                    .AddTransient<IPickConfigViewModel, PickConfigViewModel>()
                    .AddTransient<IPickServerDatabaseViewModel, PickServerDatabaseViewModel>()
                    .AddTransient<IPickTablesViewModel, PickTablesViewModel>()
                    .AddSingleton<Func<ISchemaInformationViewModel>>(() => new SchemaInformationViewModel())
                    .AddSingleton<Func<ITableInformationViewModel>>(provider => () => new TableInformationViewModel(provider.GetService<IMessenger>()))
                    .AddSingleton<Func<IColumnInformationViewModel>>(provider => () => new ColumnInformationViewModel(provider.GetService<IMessenger>()))
                    .AddTransient<IModelingOptionsViewModel, ModelingOptionsViewModel>()
                    .AddTransient<IMigrationOptionsViewModel, MigrationOptionsViewModel>()
                    .AddTransient<IPickSchemasViewModel, PickSchemasViewModel>()
                    .AddTransient<IAdvancedModelingOptionsViewModel, AdvancedModelingOptionsViewModel>()
                    .AddTransient<IObjectTreeViewModel, ObjectTreeViewModel>();

            // Register BLL
            var messenger = new Messenger();
            messenger.Register<ShowMessageBoxMessage>(this, HandleShowMessageBoxMessage);

            services.AddSingleton<IExtensionVersionService, ExtensionVersionService>()
                    .AddSingleton<IInstalledComponentsService, InstalledComponentsService>()
                    .AddSingleton<IMessenger>(messenger);

            // Register DAL
            services.AddTransient<IVisualStudioAccess, VisualStudioAccess>(provider => new VisualStudioAccess(this, _dte2))
                    .AddSingleton<ITelemetryAccess, TelemetryAccess>()
                    .AddSingleton<IOperatingSystemAccess, OperatingSystemAccess>()
                    .AddSingleton<IDotNetAccess, DotNetAccess>();

            return services.BuildServiceProvider();
        }

        private void HandleShowMessageBoxMessage(ShowMessageBoxMessage msg)
        {
            EnvDteHelper.ShowMessage(msg.Content);
        }

        internal void LogError(List<string> statusMessages, Exception exception)
        {
            _dte2.StatusBar.Text = "An error occurred. See the Output window for details.";

            try
            {
                var buildOutputWindow = _dte2.ToolWindows.OutputWindow.OutputWindowPanes.Item("Build");
                buildOutputWindow.OutputString(Environment.NewLine);

                foreach (var error in statusMessages)
                {
                    buildOutputWindow.OutputString(error + Environment.NewLine);
                }
                if (exception != null)
                {
                    buildOutputWindow.OutputString(exception + Environment.NewLine);
                }

                buildOutputWindow.Activate();
            }
            catch
            {
                EnvDteHelper.ShowError(exception.ToString());
            }
        }

        internal T GetService<T>()
            where T : class
        {
            return (T)GetService(typeof(T));
        }

        internal TResult GetService<TService, TResult>()
            where TService : class
            where TResult : class
        {
            return (TResult)GetService(typeof(TService));
        }

        internal TView GetView<TView>()
            where TView : IView
        {
            return _extensionServices.GetService<TView>();
        }
    }
}
