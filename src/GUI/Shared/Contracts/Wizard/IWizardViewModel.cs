using EFCorePowerTools.Contracts.ViewModels;

namespace EFCorePowerTools.Contracts.Wizard
{
    public interface IWizardViewModel
    {
        public IObjectTreeViewModel ObjectTree { get; set; }
    }
}