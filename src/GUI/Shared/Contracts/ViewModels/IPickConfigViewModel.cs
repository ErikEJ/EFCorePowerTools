using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IPickConfigViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand LoadedCommand { get; }

        ICommand OkCommand { get; }

        ICommand CancelCommand { get; }

        ObservableCollection<ConfigModel> Configurations { get; }

        ConfigModel SelectedConfiguration { get; set; }
    }
}