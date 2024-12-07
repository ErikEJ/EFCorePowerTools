using System.Windows;
using System.Windows.Navigation;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Westwind.Wpf.Statusbar;

namespace EFCorePowerTools.Wizard
{
    public class WizardResultPageFunction : PageFunction<WizardResult>
    {
        protected IMessenger messenger;

        private WizardDataViewModel viewModel;

        private IWizardView WizardView { get; set; }

        public WizardResultPageFunction(WizardDataViewModel wizardViewModel, IWizardView wizardView)
        {
            this.viewModel = wizardViewModel;
            DataContext = wizardViewModel;
            this.WizardView = wizardView;
        }

        public void InitializeMessengerWithStatusbar(StatusbarControl statusbarCtrl)
        {
            messenger = viewModel.WizardEventArgs.ServiceProvider.GetRequiredService<IMessenger>();
            messenger.Register<ShowStatusbarMessage>(this, (message) =>
            {
                switch (message.Type)
                {
                    case StatusbarMessageTypes.Status:
                        statusbarCtrl.Status.ShowStatus(); // defaults to Ready
                        break;
                    case StatusbarMessageTypes.Progress:
                        statusbarCtrl.Status.ShowStatusProgress(message.Content);
                        break;
                    case StatusbarMessageTypes.Success:
                        statusbarCtrl.Status.ShowStatusSuccess(message.Content);
                        break;
                    case StatusbarMessageTypes.Error:
                        statusbarCtrl.Status.ShowStatusError(message.Content);
                        break;
                    case StatusbarMessageTypes.Warning:
                        statusbarCtrl.Status.ShowStatusWarning(message.Content);
                        break;
                }
            });
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to previous wizard page
            NavigationService?.GoBack();
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(WizardResult.Canceled));
        }

        public void WizardPage_Return(object sender, ReturnEventArgs<WizardResult> e)
        {
            WizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(WizardResult.Finished));
        }

        public void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            WizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(WizardResult.Finished));
        }
    }
}
