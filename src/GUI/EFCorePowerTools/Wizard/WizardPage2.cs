// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using EFCorePowerTools.Contracts.Wizard;

namespace EFCorePowerTools.Wizard
{
    public partial class WizardPage2 : WizardResultPageFunction
    {
        public WizardPage2(WizardDataViewModel wizardViewModel, IWizardView wizardView)
            : base(wizardViewModel, wizardView)
        {
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
