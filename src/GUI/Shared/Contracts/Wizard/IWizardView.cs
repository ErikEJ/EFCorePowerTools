using EFCorePowerTools.Wizard;

namespace EFCorePowerTools.Contracts.Wizard
{
    public interface IWizardView
    {
        void WizardReturnInvoke(object sender, WizardReturnEventArgs e);

        bool? ShowDialog();
    }
}
