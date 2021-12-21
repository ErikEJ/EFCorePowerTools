namespace EFCorePowerTools.ViewModels
{
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using Contracts.Views;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Shared.DAL;
    using Shared.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class PickConfigViewModel : ViewModelBase, IPickConfigViewModel
    {
        private ConfigModel _selectedConfiguration;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableCollection<ConfigModel> Configurations { get; }

        public ConfigModel SelectedConfiguration
        {
            get => _selectedConfiguration;
            set
            {
                if (Equals(value, _selectedConfiguration)) return;
                _selectedConfiguration = value;
                RaisePropertyChanged();
            }
        }

        public PickConfigViewModel(IVisualStudioAccess visualStudioAccess, Func<IPickSchemasDialog> pickSchemasDialogFactory)
        {
            LoadedCommand = new RelayCommand(Loaded_Executed);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);

            Configurations = new ObservableCollection<ConfigModel>();

            Configurations.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(Configurations));
        }

        private void Loaded_Executed()
        {
            // Database first
            if (Configurations.Any())
            {
                SelectedConfiguration = Configurations.First();
            }
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => SelectedConfiguration != null;

        private void Cancel_Executed()
        {
            SelectedConfiguration = null;
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }
    }
}