// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Navigation;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.ViewModels;

namespace EFCorePowerTools.Wizard
{
    public class WizardLauncher(WizardDataViewModel wizardViewModel, IWizardView wizardView)
        : PageFunction<WizardResult>
    {
        protected override void Start()
        {
            base.Start();

            // So we remember the WizardCompleted event registration
            KeepAlive = true;

            // Launch the wizard
            var wizardPage1 = new WizardPage1(wizardViewModel, wizardView);
            wizardPage1.Return += WizardPage_Return;
            NavigationService?.Navigate(wizardPage1);
        }

        protected void WizardPage_Return(object sender, ReturnEventArgs<WizardResult> e)
        {
            // Notify client that wizard has completed
            // NOTE: We need this custom event because the Return event cannot be
            // registered by window code - if WizardDialogBox registers an event handler with
            // the WizardLauncher's Return event, the event is not raised.
            wizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(e.Result, wizardViewModel));
            OnReturn(null);
        }
    }
}
