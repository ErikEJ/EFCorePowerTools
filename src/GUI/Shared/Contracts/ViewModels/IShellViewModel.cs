using System;
using System.Windows.Input;
using EFCorePowerTools.Contracts.EventArgs;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IShellViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        string Message { get; set; }

        void SetIntervalValue(int intervalValue);
    }
}
