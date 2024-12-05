using System.Windows;
using System.Windows.Navigation;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.ViewModels;

namespace EFCorePowerTools.Wizard
{
    public class WizardResultPageFunction : PageFunction<WizardResult>
    {
        private IWizardView WizardView { get; set; }

        public WizardResultPageFunction(WizardDataViewModel wizardViewModel, IWizardView wizardView)
        {
            DataContext = wizardViewModel;
            this.WizardView = wizardView;
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
