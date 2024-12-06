// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace EFCorePowerTools.Wizard
{
    public partial class Wiz4_StatusDialog : WizardResultPageFunction
    {
        private readonly IMessenger messenger;
        private readonly IWizardView wizardView;

        public Wiz4_StatusDialog(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            messenger = viewModel.WizardEventArgs.ServiceProvider.GetRequiredService<IMessenger>();
            messenger.Register<ShowStatusbarMessage>(this, (message) =>
            {
                switch (message.Type)
                {
                    case StatusbarMessageTypes.Status:
                        Statusbar.Status.ShowStatus(); // defaults to Ready
                        break;
                    case StatusbarMessageTypes.Progress:
                        Statusbar.Status.ShowStatusProgress(message.Content);
                        break;
                    case StatusbarMessageTypes.Success:
                        Statusbar.Status.ShowStatusSuccess(message.Content);
                        break;
                    case StatusbarMessageTypes.Error:
                        Statusbar.Status.ShowStatusError(message.Content);
                        break;
                    case StatusbarMessageTypes.Warning:
                        Statusbar.Status.ShowStatusWarning(message.Content);
                        break;
                }
            });

            this.wizardView = wizardView;

            Loaded += WizardPage4_Loaded;

            InitializeComponent();
        }

        private void WizardPage4_Loaded(object sender, RoutedEventArgs e)
        {
            WindowTitle = "Status";
            Statusbar.Status.ShowStatus();
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
