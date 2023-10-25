using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IModelingOptionsViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        ModelingOptionsModel Model { get; }

        IReadOnlyList<string> GenerationModeList { get; }
        string Title { get; }
        bool MayIncludeConnectionString { get; }
        ObservableCollection<TemplateTypeItem> TemplateTypeList { get; }
        int SelectedTemplateType { get; set; }
        void ApplyPresets(ModelingOptionsModel presets);
    }
}
