// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using EFCorePowerTools.BLL;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.ViewModels;

namespace EFCorePowerTools.Wizard
{
    public class WizardDialogBox : NavigationWindow, IWizardView
    {
        public WizardDialogBox(IReverseEngineerBll bll, EventArgs e, IWizardViewModel viewModel)
        {
            var wizardViewModel = (WizardDataViewModel)viewModel;

            InitializeComponent();

            if (e is WizardEventArgs wizardArgs)
            {
                wizardArgs.IsInvokedByWizard = true;
                wizardViewModel.WizardEventArgs = wizardArgs;
                wizardViewModel.UiHint = wizardArgs.UiHint;
                wizardViewModel.Filename = wizardArgs.Filename;
                wizardViewModel.OnlyGenerate = wizardArgs.OnlyGenerate;
            }

            // Assign the Business logic layer used for processing
            wizardViewModel.Bll = bll;

            // Launch the wizard
            var wizardLauncher = new WizardLauncher(wizardViewModel, this);
            WizardReturn += WizardLauncher_WizardReturn;
            Navigate(wizardLauncher);
        }

        public event WizardReturnEventHandler WizardReturn;

        public void WizardReturnInvoke(object sender, WizardReturnEventArgs e)
        {
            WizardReturn?.Invoke(this, new WizardReturnEventArgs(e.Result));
        }

        private void WizardLauncher_WizardReturn(object sender, WizardReturnEventArgs e)
        {
            if (DialogResult == null)
            {
                DialogResult = e.Result == WizardResult.Finished;
            }
        }
    }
}