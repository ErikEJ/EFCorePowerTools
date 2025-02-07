using System.Collections.Generic;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IModelingOptionsDialog : IDialog<ModelingOptionsModel>
    {
        IModelingOptionsDialog ApplyPresets(ModelingOptionsModel presets);

        void PublishTemplateTypes(TemplateTypeItem templateType, IList<TemplateTypeItem> allowedTemplateTypes);
    }
}