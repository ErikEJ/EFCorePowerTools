namespace EFCorePowerTools.Contracts.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using EventArgs;
    using Shared.Models;

    public interface IModelingOptionsViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand LoadedCommand { get; }
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