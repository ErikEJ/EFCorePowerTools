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
        private bool isPageInitialized;
        private string initStatusMessage;
        private StatusbarControl statusbarCtrl;

        protected IMessenger messenger;

        private WizardDataViewModel viewModel;

        private IWizardView WizardView { get; set; }

        public WizardResultPageFunction(
            WizardDataViewModel wizardViewModel,
            IWizardView wizardView)
        {
            this.viewModel = wizardViewModel;
            DataContext = wizardViewModel;
            this.WizardView = wizardView;
        }

        public void InitializeMessengerWithStatusbar(StatusbarControl statusbarCtrl, string initStatusMessage)
        {
            this.statusbarCtrl = statusbarCtrl;
            this.initStatusMessage = initStatusMessage;

            messenger = viewModel.WizardEventArgs.ServiceProvider.GetRequiredService<IMessenger>();
            messenger.Register<ShowStatusbarMessage>(this, (message) =>
            {
                switch (message.Type)
                {
                    case StatusbarMessageTypes.Status:
                        statusbarCtrl.Status.ShowStatus(); // defaults to Ready
                        break;
                    case StatusbarMessageTypes.Progress:
                        statusbarCtrl.Status.ShowStatusProgress(message.Message);
                        break;
                    case StatusbarMessageTypes.Success:
                        statusbarCtrl.Status.ShowStatusSuccess(message.Message);
                        break;
                    case StatusbarMessageTypes.Error:
                        statusbarCtrl.Status.ShowStatusError(message.Message);
                        break;
                    case StatusbarMessageTypes.Warning:
                        statusbarCtrl.Status.ShowStatusWarning(message.Message);
                        break;
                }
            });

            statusbarCtrl.Loaded += StatusbarCtrl_Loaded;
            statusbarCtrl.StatusEvent += StatusbarCtrl_StatusEvent;
        }

        private void StatusbarCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            statusbarCtrl.Status.ShowStatusProgress(initStatusMessage, 500);
            statusbarCtrl.StatusEvent += StatusbarCtrl_StatusEvent;
        }

        private void StatusbarCtrl_StatusEvent(object sender, StatusbarEventArgs e)
        {
            if (e.EventType == StatusbarEventType.Reset)
            {
                isPageInitialized = true;

                // We're done with this reset event - only fires once
                statusbarCtrl.StatusEvent -= StatusbarCtrl_StatusEvent;

                OnPageVisible(sender, e);

                statusbarCtrl.Status.ShowStatusProgress(initStatusMessage, 100);
            }
        }

        protected virtual void OnPageVisible(object sender, StatusbarEventArgs e)
        {
            // Intended to be overridden
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
