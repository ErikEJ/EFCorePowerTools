﻿// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.Wizard
{
    public partial class Wiz4_StatusDialog : WizardResultPageFunction
    {
        private readonly IWizardView wizardView;
        private readonly WizardDataViewModel wizardViewModel;

        public Wiz4_StatusDialog(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            this.wizardView = wizardView;
            this.wizardViewModel = viewModel;

            Loaded += WizardPage4_Loaded;

            InitializeComponent();
            InitializeMessengerWithStatusbar(Statusbar, "Loading Status");
        }

        private void WizardPage4_Loaded(object sender, RoutedEventArgs e)
        {
            WindowTitle = "Status";
            Statusbar.Status.ShowStatus("ready");
        }

        protected override void OnPageVisible(object sender, StatusbarEventArgs e)
        {
            var viewModel = wizardViewModel;
            IsPageLoaded = viewModel.IsPage4Initialized;

            if (!IsPageLoaded)
            {
                viewModel.IsPage4Initialized = true;

                Messenger.Send(new ShowStatusbarMessage("Loading status"));

                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    // await wizardViewModel.Bll.xxx(wea.Options, wea.DbInfo, wea.NamingOptionsAndPath, wea);
                });
            }
        }


        public void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            var wizardPage2 = new Wiz2_PickTablesDialog((WizardDataViewModel)DataContext, wizardView);
            wizardPage2.Return += WizardPage_Return;
            NavigationService?.Navigate(wizardPage2);
        }
    }
}
