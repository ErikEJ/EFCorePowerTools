// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using EFCorePowerTools.BLL;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.ViewModels;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.Wizard
{
    public class WizardDialogBox : NavigationWindow, IWizardView
    {
        private WizardCommandFilter commandFilter;

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

            // Register priority command filter to handle Delete key properly in text fields
            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                commandFilter = new WizardCommandFilter(ServiceProvider.GlobalProvider);
                commandFilter.RegisterCommandFilter();
            });

            // Launch the wizard
            var wizardLauncher = new WizardLauncher(wizardViewModel, this);
            WizardReturn += WizardLauncher_WizardReturn;
            Closed += WizardDialogBox_Closed;
            Navigate(wizardLauncher);
        }

        private void WizardDialogBox_Closed(object sender, EventArgs e)
        {
            // Clean up command filter
            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                commandFilter?.UnregisterCommandFilter();
                commandFilter?.Dispose();
                commandFilter = null;
            });

            RevEngWizardHandler.WizardIsRunning = false;
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
                try
                {
                    DialogResult = e.Result == WizardResult.Finished;
                }
                catch
                {
                    this.Close();
                }
            }
        }
    }
}