namespace EFCorePowerTools.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Shared.DAL;
    using Shared.Models;

    public class PickServerDatabaseViewModel : ViewModelBase, IPickServerDatabaseViewModel
    {
        private readonly IVisualStudioAccess _visualStudioAccess;

        private DatabaseConnectionModel _selectedDatabaseConnection;
        private DatabaseDefinitionModel _selectedDatabaseDefinition;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand AddDatabaseConnectionCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; }
        public ObservableCollection<DatabaseDefinitionModel> DatabaseDefinitions { get; }

        public DatabaseConnectionModel SelectedDatabaseConnection
        {
            get => _selectedDatabaseConnection;
            set
            {
                if (Equals(value, _selectedDatabaseConnection)) return;
                _selectedDatabaseConnection = value;
                RaisePropertyChanged();

                if (_selectedDatabaseConnection != null)
                    SelectedDatabaseDefinition = null;
            }
        }

        public DatabaseDefinitionModel SelectedDatabaseDefinition
        {
            get => _selectedDatabaseDefinition;
            set
            {
                if (Equals(value, _selectedDatabaseDefinition)) return;
                _selectedDatabaseDefinition = value;
                RaisePropertyChanged();

                if (_selectedDatabaseDefinition != null)
                    SelectedDatabaseConnection = null;
            }
        }

        public PickServerDatabaseViewModel(IVisualStudioAccess visualStudioAccess)
        {
            _visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));

            LoadedCommand = new RelayCommand(Loaded_Executed);
            AddDatabaseConnectionCommand = new RelayCommand(AddDatabaseConnection_Executed);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);

            DatabaseConnections = new ObservableCollection<DatabaseConnectionModel>();
            DatabaseDefinitions = new ObservableCollection<DatabaseDefinitionModel>();
            DatabaseDefinitions.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(DatabaseDefinitions));
        }

        private void Loaded_Executed()
        {
            if (DatabaseConnections.Any())
                SelectedDatabaseConnection = DatabaseConnections.First();
        }

        private void AddDatabaseConnection_Executed()
        {
            DatabaseConnectionModel newDatabaseConnection;
            try
            {
                newDatabaseConnection = _visualStudioAccess.PromptForNewDatabaseConnection();
            }
            catch (Exception e)
            {
                _visualStudioAccess.ShowMessage("Unable to add connection, maybe the provider is not supported: " + e.Message);
                return;
            }

            if (newDatabaseConnection == null)
                return;

            DatabaseConnections.Add(newDatabaseConnection);
            SelectedDatabaseConnection = newDatabaseConnection;
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => SelectedDatabaseConnection != null || SelectedDatabaseDefinition != null;

        private void Cancel_Executed()
        {
            SelectedDatabaseConnection = null;
            SelectedDatabaseDefinition = null;
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }
    }
}