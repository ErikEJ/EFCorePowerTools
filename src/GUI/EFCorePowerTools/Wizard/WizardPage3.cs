// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using EFCorePowerTools.Contracts.Wizard;

namespace EFCorePowerTools.Wizard
{
    public partial class WizardPage3 : WizardResultPageFunction
    {
        public WizardPage3(WizardDataViewModel wizardViewModel, IWizardView wizardView)
            : base(wizardViewModel, wizardView)
        {
            InitializeComponent();
        }
    }
}
