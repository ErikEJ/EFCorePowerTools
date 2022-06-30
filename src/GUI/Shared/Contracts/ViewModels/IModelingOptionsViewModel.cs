using System;
using System.Collections.Generic;
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
        IReadOnlyList<string> HandlebarsLanguageList { get; }
        string Title { get; }
        bool MayIncludeConnectionString { get; }

        void ApplyPresets(ModelingOptionsModel presets);
    }
}
