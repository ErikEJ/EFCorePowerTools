// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using EFCorePowerTools.Contracts.Wizard;

namespace EFCorePowerTools.Wizard
{
    public class WizardDialogBox : NavigationWindow, IWizardView
    {
        private readonly WizardData wizardViewModel = new();

        public WizardDialogBox()
        {
            InitializeComponent();

            // Launch the wizard
            var wizardLauncher = new WizardLauncher(wizardViewModel, this);
            WizardReturn += WizardLauncher_WizardReturn;
            Navigate(wizardLauncher);
        }

        public event WizardReturnEventHandler WizardReturn;

        public WizardData WizardData { get; private set; }

        public void WizardReturnInvoke(object sender, WizardReturnEventArgs e)
        {
            WizardReturn?.Invoke(this, new WizardReturnEventArgs(e.Result, wizardViewModel));
        }

        private void WizardLauncher_WizardReturn(object sender, WizardReturnEventArgs e)
        {
            // Handle wizard return
            WizardData = e.Data as WizardData;
            if (DialogResult == null)
            {
                DialogResult = e.Result == WizardResult.Finished;
            }
        }
    }
}
