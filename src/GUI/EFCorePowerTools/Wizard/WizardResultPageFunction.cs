using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Westwind.Wpf.Statusbar;

namespace EFCorePowerTools.Wizard
{
    public class WizardResultPageFunction : PageFunction<WizardResult>
    {
        private readonly IWizardView wizardView;
        private string initStatusMessage;
        private StatusbarControl statusbarCtrl;

#pragma warning disable SA1202 // Elements should be ordered by access

        protected IMessenger Messenger { get; set; }
        public bool IsPageLoaded { get; set; }

#pragma warning disable SA1201 // Elements should appear in the correct order
        public WizardResultPageFunction(WizardDataViewModel wizardViewModel, IWizardView wizardView)
#pragma warning restore SA1201 // Elements should appear in the correct order
        {
            DataContext = wizardViewModel;
            this.wizardView = wizardView;
        }

        public void InitializeMessengerWithStatusbar(StatusbarControl statusbarCtrl, string initStatusMessage)
        {
            this.statusbarCtrl = statusbarCtrl;
            this.initStatusMessage = initStatusMessage;
            statusbarCtrl.Loaded += StatusbarCtrl_Loaded;
        }

        private void StatusbarCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                IsPageLoaded = true;
                statusbarCtrl.StatusEvent += StatusbarCtrl_StatusEvent;
                statusbarCtrl.Status.ShowStatusProgress(initStatusMessage, 100);
                OnPageLoaded(sender, e);
            }
            else
            {
                statusbarCtrl.Status.ShowStatus(); // defaults to Ready
            }
        }

        protected void OpenBrowserWithLink(string link)
        {
            // Subscribe to status event, once the 2 seconds (2000 ms) expires
            // the StatusbarEventType.Reset will be raised by ShowStatus
            statusbarCtrl.StatusEvent += Statusbar_StatusEvent;
            statusbarCtrl.Status.ShowStatus(ReverseEngineerLocale.StatusbarLoadingBrowser, 2000, null, true, true, true);
            Process.Start(new ProcessStartInfo(link));
        }

        private void Statusbar_StatusEvent(object sender, StatusbarEventArgs e)
        {
            if (e.EventType == StatusbarEventType.Reset)
            {
                // Unsubscribe to event and ShowStatus with no message to
                // reset to Ready
                statusbarCtrl.StatusEvent -= Statusbar_StatusEvent;
                statusbarCtrl.Status.ShowStatus(); // Reset
            }
        }

        private void StatusbarCtrl_StatusEvent(object sender, StatusbarEventArgs e)
        {
            if (e.EventType == StatusbarEventType.Reset)
            {
                // We're done with this reset event - only fires once
                statusbarCtrl.StatusEvent -= StatusbarCtrl_StatusEvent;
                OnPageVisible(sender, e);

                statusbarCtrl.Status.ShowStatus();
            }
        }

        protected virtual void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            // Intended to be overridden
        }

        protected virtual void OnPageVisible(object sender, StatusbarEventArgs e)
        {
            // Intended to be overridden
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
#pragma warning restore SA1202 // Elements should be ordered by access
        {
            // Go to previous wizard page
            NavigationService?.GoBack();
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            wizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(WizardResult.Canceled));
        }

        public void WizardPage_Return(object sender, ReturnEventArgs<WizardResult> e)
        {
            wizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(WizardResult.Finished));
        }

        public void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            wizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(WizardResult.Finished));
        }
    }
}
