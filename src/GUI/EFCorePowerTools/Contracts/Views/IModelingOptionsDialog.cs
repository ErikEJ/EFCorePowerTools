namespace EFCorePowerTools.Contracts.Views
{
    using Shared.Models;

    public interface IModelingOptionsDialog : IDialog<ModelingOptionsModel>
    {
        IModelingOptionsDialog ApplyPresets(ModelingOptionsModel presets);
    }
}