using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.Models;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IPickProjectViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand LoadedCommand { get; }
        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        ObservableCollection<ProjectModel> Projects { get; }

        ProjectModel SelectedProject { get; set; }
    }
}
