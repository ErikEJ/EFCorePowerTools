namespace EFCorePowerTools.Contracts.Views
{
    using Common.Models;

    public interface IModelingOptionsDialog : IDialog<ModelingOptionsModel>
    {
        IModelingOptionsDialog ApplyPresets(ModelingOptionsModel presets);
    }
}