using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using JetBrains.Annotations;
using RevEng.Common;

namespace EFCorePowerTools.ViewModels
{
    public class PickConnectionViewModel : ViewModelBase, IPickConnectionViewModel
    {
        private string connectionString;
        private string name;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public string ConnectionString
        {
            get => connectionString;
            set
            {
                if (Equals(value, connectionString))
                {
                    return;
                }

                connectionString = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (Equals(value, name))
                {
                    return;
                }

                name = value;
                OnPropertyChanged();
            }
        }

        public DatabaseType DbType { get; set; }

        public DatabaseConnectionModel DatabaseConnection
        {
            get
            {
                if (Ok_CanExecute())
                {
                    return new DatabaseConnectionModel { DatabaseType = DbType, ConnectionString = ConnectionString, ConnectionName = Name };
                }

                return null;
            }
        }

        public PickConnectionViewModel()
        {
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => !string.IsNullOrWhiteSpace(ConnectionString) && !string.IsNullOrWhiteSpace(Name);

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
