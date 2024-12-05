﻿// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        private bool isInitialized;

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

                viewModel.SelectedDatabaseConnection = viewModel.DatabaseConnections.FirstOrDefault();
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

            Loaded += WizardPage1_Loaded;

            InitializeComponent();
        }

        private void WizardPage1_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInitialized)
            {
                var viewModel = wizardViewModel;

                Statusbar.Status.ShowStatusProgress(ReverseEngineerLocale.GettingReadyToConnect);

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

                isInitialized = true;
                NextButton.IsEnabled = true;
            }

            WindowTitle = ReverseEngineerLocale.ChooseDatabaseConnection;
            Statusbar.Status.ShowStatus();
        }

        public void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Statusbar.Status.ShowStatusProgress(ReverseEngineerLocale.LoadingDatabaseObjects);

            // Go to next wizard page
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
