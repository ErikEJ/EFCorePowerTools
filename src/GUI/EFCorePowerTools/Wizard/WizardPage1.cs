// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Windows;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.Wizard;

namespace EFCorePowerTools.Wizard
{
    public partial class WizardPage1 : WizardResultPageFunction
    {
        private WizardDataViewModel wizardViewModel;
        public WizardPage1(WizardDataViewModel wizardViewModel, IWizardView wizardView)
            : base(wizardViewModel, wizardView)
        {
            InitializeComponent();

            this.wizardViewModel = wizardViewModel;

            var options = wizardViewModel.Bll.PickConfigDialogInitializeAsync(wizardViewModel).Result;
            foreach (var option in options)
            {
                wizardViewModel.Configurations.Add(option);
            }
            OnConfigurationChange(options.FirstOrDefault());
        }

        public void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            var wizardPage2 = new WizardPage2((WizardDataViewModel)DataContext, wizardView);
            wizardPage2.Return += WizardPage_Return;
            NavigationService?.Navigate(wizardPage2);
        }

        /// <summary>
        /// Code to invoke when the Configuration [dropdown] changes
        /// </summary>
        /// <param name="config">ConfigModel to set as selected</param>
        protected void OnConfigurationChange(ConfigModel config)
        {
            wizardViewModel.SelectedConfiguration = config;
            wizardViewModel.OptionsPath = config.ConfigPath;
        }
    }
}
