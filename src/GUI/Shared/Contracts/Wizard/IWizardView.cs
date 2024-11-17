using EFCorePowerTools.BLL;
using EFCorePowerTools.Wizard;

namespace EFCorePowerTools.Contracts.Wizard
{
    public interface IWizardView
    {
        /// <summary>
        /// Assign BLL implementation to each view so that views can use
        /// Bll to perform business logic; versus the presenter/handler
        /// having to reach into views to run business logc.
        /// </summary>
        IReverseEngineerBll Bll { get; set; }

        void WizardReturnInvoke(object sender, WizardReturnEventArgs e);

        bool? ShowDialog();
    }
}
