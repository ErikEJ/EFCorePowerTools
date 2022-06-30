using EFCorePowerTools.Common.Models;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IModelingOptionsDialog : IDialog<ModelingOptionsModel>
    {
        IModelingOptionsDialog ApplyPresets(ModelingOptionsModel presets);
    }
}