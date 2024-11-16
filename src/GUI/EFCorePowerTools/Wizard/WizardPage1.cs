// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using EFCorePowerTools.Contracts.Wizard;

namespace EFCorePowerTools.Wizard
{
    public partial class WizardPage1 : WizardResultPageFunction
    {
        public WizardPage1(WizardData wizardViewModel, IWizardView wizardView)
            : base(wizardViewModel, wizardView)
        {
            InitializeComponent();
        }

        public void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            var wizardPage2 = new WizardPage2((WizardData)DataContext, wizardView);
            wizardPage2.Return += WizardPage_Return;
            NavigationService?.Navigate(wizardPage2);
        }
    }
}
