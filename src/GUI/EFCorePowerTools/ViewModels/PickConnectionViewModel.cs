namespace EFCorePowerTools.ViewModels
{
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using JetBrains.Annotations;
    using RevEng.Shared;
    using Shared.Models;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class PickConnectionViewModel : ViewModelBase, IPickConnectionViewModel, INotifyPropertyChanged
    {
        private string _connectionString;
        private string _name;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                if (Equals(value, _connectionString)) return;
                _connectionString = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (Equals(value, _name)) return;
                _name = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}