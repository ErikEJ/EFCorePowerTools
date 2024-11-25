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
        private readonly WizardDataViewModel wizardViewModel = new();

        public WizardDialogBox(IReverseEngineerBll bll, EventArgs e)
        {
            InitializeComponent();

            if (e is WizardEventArgs args)
            {
                wizardViewModel.UiHint = args.UiHint;
                wizardViewModel.Project = args.Project;
                wizardViewModel.Filename = args.Filename;
                wizardViewModel.OnlyGenerate = args.OnlyGenerate;
            }

            // Assign the Business logic layer used for processing
            wizardViewModel.Bll = bll;

            // Launch the wizard
            var wizardLauncher = new WizardLauncher(wizardViewModel, this);
            WizardReturn += WizardLauncher_WizardReturn;
            Navigate(wizardLauncher);
        }

        public event WizardReturnEventHandler WizardReturn;

        public WizardDataViewModel WizardDataViewModel { get; private set; }

        public void WizardReturnInvoke(object sender, WizardReturnEventArgs e)
        {
            WizardReturn?.Invoke(this, new WizardReturnEventArgs(e.Result, wizardViewModel));
        }

        private void WizardLauncher_WizardReturn(object sender, WizardReturnEventArgs e)
        {
            // Handle wizard return
            WizardDataViewModel = e.Data as WizardDataViewModel;
            if (DialogResult == null)
            {
                DialogResult = e.Result == WizardResult.Finished;
            }
        }
    }
}
