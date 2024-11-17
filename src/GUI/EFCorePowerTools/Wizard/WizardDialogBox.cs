// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using EFCorePowerTools.BLL;
using EFCorePowerTools.Contracts.Wizard;

namespace EFCorePowerTools.Wizard
{
    public class WizardDialogBox : NavigationWindow, IWizardView
    {
        private readonly WizardDataViewModel wizardViewModel = new();

        public WizardDialogBox(IReverseEngineerBll bll)
        {
            InitializeComponent();

            // Assign the Business logic layer used for processing
            Bll = bll;

            // Launch the wizard
            var wizardLauncher = new WizardLauncher(wizardViewModel, this);
            WizardReturn += WizardLauncher_WizardReturn;
            Navigate(wizardLauncher);
        }

        public event WizardReturnEventHandler WizardReturn;

        public WizardDataViewModel WizardDataViewModel { get; private set; }
        public IReverseEngineerBll Bll { get; set; }

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
