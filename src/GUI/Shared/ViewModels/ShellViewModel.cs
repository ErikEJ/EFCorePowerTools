using System;
using System.Windows.Input;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EFCorePowerTools.ViewModels
{
    public class ShellViewModel : ViewModelBase, IShellViewModel
    {
        private int progressValue = 0;
        private string message = string.Empty;
        private int intervalValue = 20;

        public ShellViewModel()
        {
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);
        }

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public int ProgressValue
        {
            get
            {
                return progressValue;
            }
            set
            {
                progressValue = value;
                RaisePropertyChanged();
            }
        }


        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                ProgressValue += intervalValue;
                RaisePropertyChanged();
            }
        }

        public void SetIntervalValue(int intervalValue)
        {
            this.intervalValue = intervalValue;
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => true;

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }
    }
}
