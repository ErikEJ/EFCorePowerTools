using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.Wizard
{
    public partial class Wiz1_PickServerDatabaseDialog : WizardResultPageFunction, IPickServerDatabaseDialog
    {
        private readonly IWizardView wizardView;
        private readonly WizardDataViewModel wizardViewModel;
        private readonly Func<(DatabaseConnectionModel Connection, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint, WizardEventArgs WizardArgs)> getDialogResult;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> addConnections;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> addDefinitions;
        private readonly Action<IEnumerable<SchemaInfo>> addSchemas;
        private readonly Action<CodeGenerationMode, IList<CodeGenerationItem>> codeGeneration;
        private readonly Action<string> uiHint;

        public Wiz1_PickServerDatabaseDialog(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            getDialogResult = () =>
            {
                wizardViewModel.WizardEventArgs.PickServerDatabaseComplete = true;

                return (
                    viewModel.SelectedDatabaseConnection,
                    (CodeGenerationMode)viewModel.CodeGenerationMode,
                    viewModel.FilterSchemas,
                    viewModel.Schemas.ToArray(),
                    viewModel.UiHint,
                    viewModel.WizardEventArgs);
            };
            addConnections = models =>
            {
                foreach (var model in models)
                {
                    if (!viewModel.DatabaseConnections.Any(db => db.DisplayName == model.DisplayName))
                    {
                        viewModel.DatabaseConnections.Add(model);
                    }
                }

                wizardViewModel.RemoveDatabaseConnectionCommand.RaiseCanExecuteChanged();
            };
            addDefinitions = models =>
            {
                foreach (var model in models)
                {
                    if (!viewModel.DatabaseConnections.Any(db => db.DisplayName == model.DisplayName))
                    {
                        viewModel.DatabaseConnections.Add(model);
                    }
                }
            };
            addSchemas = models =>
            {
                viewModel.FilterSchemas = models.Any();
                foreach (var model in models)
                {
                    if (!viewModel.Schemas.Exists(m => m.Name == model.Name))
                    {
                        viewModel.Schemas.Add(model);
                    }
                }
            };
            codeGeneration = (codeGeneration, allowedVersions) =>
            {
                if (allowedVersions.Count == 1 && allowedVersions[0].Value == "DAB")
                {
                    grdRow1.Height = new GridLength(0);
                    grdRow2.Height = new GridLength(0);
                    grdRow3.Height = new GridLength(0);
                    viewModel.CodeGenerationMode = (int)codeGeneration;
                    return;
                }

                foreach (var item in allowedVersions)
                {
                    if (!viewModel.CodeGenerationModeList.Any(a => a.Value == item.Value))
                    {
                        viewModel.CodeGenerationModeList.Add(item);
                    }
                }

                if (!allowedVersions.Any())
                {
                    grdRow1.Height = new GridLength(0);
                    grdRow2.Height = new GridLength(0);
                }

                viewModel.CodeGenerationMode = (int)codeGeneration;
            };
            uiHint = uiHint =>
            {
                viewModel.UiHint = uiHint;
            };

            this.wizardView = wizardView;
            this.wizardViewModel = viewModel;

            InitializeComponent();

            Loaded += (s, e) => OnPageVisible(s, null);
        }

        protected override void OnPageVisible(object sender, StatusbarEventArgs e)
        {
            var viewModel = wizardViewModel;
            IsPageLoaded = viewModel.IsPage1Initialized;

            if (!IsPageLoaded)
            {
                Statusbar.Status.ShowStatus(ReverseEngineerLocale.GettingReadyToConnect, 100);

                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    viewModel.WizardEventArgs.PickServerDatabaseDialog = this;
                    await wizardViewModel.Bll.ReverseEngineerCodeFirstAsync(null, viewModel.WizardEventArgs);
                });

                foreach (var option in viewModel.WizardEventArgs.Configurations)
                {
                    if (!wizardViewModel.Configurations.Any(o => o.DisplayName == option.DisplayName))
                    {
                        wizardViewModel.Configurations.Add(option);
                    }
                }

                OnConfigurationChange(wizardViewModel.WizardEventArgs.Configurations.FirstOrDefault());
            }
        }

#pragma warning disable SA1202 // Elements should be ordered by access
        public void NextButton_Click(object sender, RoutedEventArgs e)
#pragma warning restore SA1202 // Elements should be ordered by access
        {
            wizardViewModel.IsPage1Initialized = true;
            var wizardPage2 = new Wiz2_PickTablesDialog((WizardDataViewModel)DataContext, wizardView);
            wizardPage2.Return += WizardPage_Return;
            NavigationService?.Navigate(wizardPage2);
        }

        /// <summary>
        /// Code to invoke when the Configuration [dropdown] changes.
        /// </summary>
        /// <param name="config">ConfigModel to set as selected.</param>
        public void OnConfigurationChange(ConfigModel config)
        {
            if (config == null)
            {
                return;
            }

            Statusbar.Status.ShowStatus();
            wizardViewModel.SelectedConfiguration = config;
            wizardViewModel.OptionsPath = config.ConfigPath;
            NextButton.IsEnabled = true;
        }

        public void PublishConnections(IEnumerable<DatabaseConnectionModel> connections)
        {
            addConnections(connections);
        }

        public void PublishDefinitions(IEnumerable<DatabaseConnectionModel> definitions)
        {
            addDefinitions(definitions);
        }

        public void PublishSchemas(IEnumerable<SchemaInfo> schemas)
        {
            addSchemas(schemas);
        }

        public void PublishCodeGenerationMode(CodeGenerationMode codeGenerationMode, IList<CodeGenerationItem> allowedVersions)
        {
            codeGeneration(codeGenerationMode, allowedVersions);
        }

        public void PublishUiHint(string uiHint)
        {
            this.uiHint(uiHint);
        }

        public (bool ClosedByOK, (
                DatabaseConnectionModel Connection,
                CodeGenerationMode CodeGenerationMode,
                bool FilterSchemas,
                SchemaInfo[] Schemas,
                string UiHint,
                WizardEventArgs WizardArgs) Payload)
            ShowAndAwaitUserResponse(bool modal)
        {
            return (true, getDialogResult());
        }

        public (DatabaseConnectionModel Connection, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint, WizardEventArgs WizardArgs) GetResults()
        {
            return getDialogResult();
        }
    }
}
