using System.Windows;
using System.Windows.Navigation;
using EFCorePowerTools.Contracts.Wizard;

namespace EFCorePowerTools.Wizard
{
    public class WizardResultPageFunction : PageFunction<WizardResult>
    {
        /// <summary>
        /// Provide derived classes access to an instance of the WizardView.
        /// </summary>
        protected readonly IWizardView wizardView;

        public WizardResultPageFunction(WizardDataViewModel wizardViewModel, IWizardView wizardView)
        {
            DataContext = wizardViewModel;
            this.wizardView = wizardView;
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to previous wizard page
            NavigationService?.GoBack();
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            wizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(WizardResult.Canceled, DataContext));
        }

        public void WizardPage_Return(object sender, ReturnEventArgs<WizardResult> e)
        {
            wizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(WizardResult.Finished, DataContext));
        }

        public void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            wizardView.WizardReturnInvoke(this, new WizardReturnEventArgs(WizardResult.Finished, DataContext));
        }

    }
}
