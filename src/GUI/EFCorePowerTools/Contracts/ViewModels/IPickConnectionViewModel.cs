using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Shared.Models;
using System;
using System.Windows.Input;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IPickConnectionViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        DatabaseConnectionModel DatabaseConnection { get; }
    }
}