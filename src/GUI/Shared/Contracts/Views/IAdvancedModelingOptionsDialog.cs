using EFCorePowerTools.Shared.Models;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IAdvancedModelingOptionsDialog : IDialog<ModelingOptionsModel>
    {
        IAdvancedModelingOptionsDialog ApplyPresets(ModelingOptionsModel presets);
    }
}
