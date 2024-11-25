// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Wizard;
using RevEng.Common;

namespace EFCorePowerTools.Wizard
{
    public partial class WizardPage2 : WizardResultPageFunction
    {
        private readonly Func<(DatabaseConnectionModel Connection, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint)> getDialogResult;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> addConnections;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> addDefinitions;
        private readonly Action<IEnumerable<SchemaInfo>> addSchemas;
        private readonly Action<CodeGenerationMode, IList<CodeGenerationItem>> codeGeneration;
        private readonly Action<string> uiHint;

        public WizardPage2(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            getDialogResult = () => (viewModel.SelectedDatabaseConnection, (CodeGenerationMode)viewModel.CodeGenerationMode, viewModel.FilterSchemas, viewModel.Schemas.ToArray(), viewModel.UiHint);
            addConnections = models =>
            {
                foreach (var model in models)
                {
                    viewModel.DatabaseConnections.Add(model);
                }
            };
            addDefinitions = models =>
            {
                foreach (var model in models)
                {
                    viewModel.DatabaseConnections.Add(model);
                }
            };
            addSchemas = models =>
            {
                viewModel.FilterSchemas = models.Any();
                foreach (var model in models)
                {
                    viewModel.Schemas.Add(model);
                }
            };
            codeGeneration = (codeGeneration, allowedVersions) =>
            {
                if (allowedVersions.Count == 1
                    && allowedVersions[0].Value == "DAB")
                {
                    grdRow1.Height = new GridLength(0);
                    grdRow2.Height = new GridLength(0);
                    grdRow3.Height = new GridLength(0);
                    viewModel.CodeGenerationMode = (int)codeGeneration;
                    return;
                }

                foreach (var item in allowedVersions)
                {
                    viewModel.CodeGenerationModeList.Add(item);
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

            InitializeComponent();
        }

        public void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            var wizardPage3 = new WizardPage3((WizardDataViewModel)DataContext, wizardView);
            wizardPage3.Return += WizardPage_Return;
            NavigationService?.Navigate(wizardPage3);
        }
    }
}
