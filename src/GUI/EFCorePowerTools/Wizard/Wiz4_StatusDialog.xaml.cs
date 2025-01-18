// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;

namespace EFCorePowerTools.Wizard
{
    public partial class Wiz4_StatusDialog : WizardResultPageFunction
    {
        private readonly IWizardView wizardView;
        private readonly WizardDataViewModel wizardViewModel;

        public Wiz4_StatusDialog(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            this.wizardView = wizardView;
            this.wizardViewModel = viewModel;

            Loaded += WizardPage4_Loaded;

            InitializeComponent();
            InitializeMessengerWithStatusbar(Statusbar, "Generating files...");
        }

        private void WizardPage4_Loaded(object sender, RoutedEventArgs e)
        {
            WindowTitle = "Status";
        }

        protected override void OnPageVisible(object sender, StatusbarEventArgs e)
        {
            var viewModel = wizardViewModel;
            IsPageLoaded = viewModel.IsPage4Initialized;

            if (!IsPageLoaded)
            {
                Statusbar.Status.ShowStatus("Process completed");
            }
        }

        private void TextChangedEventHandler(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Statusbar.Status.ShowStatus(); // Will reset status bar
        }
    }
}
