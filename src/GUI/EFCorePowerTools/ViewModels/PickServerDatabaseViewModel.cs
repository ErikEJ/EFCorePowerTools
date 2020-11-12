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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using ReverseEngineer20.ReverseEngineer;

    public class PickServerDatabaseViewModel : ViewModelBase, IPickServerDatabaseViewModel
    {
        private readonly IVisualStudioAccess _visualStudioAccess;
        private readonly Func<IPickSchemasDialog> _pickSchemasDialogFactory;

        private DatabaseConnectionModel _selectedDatabaseConnection;
        private DatabaseDefinitionModel _selectedDatabaseDefinition;
        private bool _includeViews = true;
        private bool _filterSchemas = false;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand AddDatabaseConnectionCommand { get; }
        public ICommand AddDatabaseDefinitionCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand FilterSchemasCommand { get; }

        public ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; }
        public ObservableCollection<DatabaseDefinitionModel> DatabaseDefinitions { get; }

        public List<SchemaInfo> Schemas { get; private set; }

        public bool IncludeViews
        {
            get => _includeViews;
            set
            {
                if (value == _includeViews) return;
                _includeViews = value;
                RaisePropertyChanged();
            }
        }

        public bool FilterSchemas
        {
            get => _filterSchemas;
            set
            {
                if (value == _filterSchemas) return;
                _filterSchemas = value;
                RaisePropertyChanged();
            }
        }

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

        public PickServerDatabaseViewModel(IVisualStudioAccess visualStudioAccess, Func<IPickSchemasDialog> pickSchemasDialogFactory)
        {
            _visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));
            _pickSchemasDialogFactory = pickSchemasDialogFactory ?? throw new ArgumentNullException(nameof(pickSchemasDialogFactory));

            LoadedCommand = new RelayCommand(Loaded_Executed);
            AddDatabaseConnectionCommand = new RelayCommand(AddDatabaseConnection_Executed);
            AddDatabaseDefinitionCommand = new RelayCommand(AddDatabaseDefinition_Executed);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);
            FilterSchemasCommand = new RelayCommand(FilterSchemas_Executed, FilterSchemas_CanExecute);

            DatabaseConnections = new ObservableCollection<DatabaseConnectionModel>();
            DatabaseDefinitions = new ObservableCollection<DatabaseDefinitionModel>();
            Schemas = new List<SchemaInfo>();
            DatabaseDefinitions.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(DatabaseDefinitions));
        }

        private void Loaded_Executed()
        {
            // Database first
            if (DatabaseConnections.Any())
            {
                SelectedDatabaseConnection = DatabaseConnections.First();
                return;
            }

            // Database definition (SQL project) first
            if (DatabaseDefinitions.Any())
            {
                SelectedDatabaseDefinition = PreSelectDatabaseDefinition();
            }
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

        private void AddDatabaseDefinition_Executed()
        {
            DatabaseDefinitionModel newDatabaseDefinition;
            try
            {
                newDatabaseDefinition = _visualStudioAccess.PromptForNewDatabaseDefinition();
            }
            catch (Exception e)
            {
                _visualStudioAccess.ShowMessage("Unable to add to list: " + e.Message);
                return;
            }

            if (newDatabaseDefinition == null)
                return;

            DatabaseDefinitions.Add(newDatabaseDefinition);
            SelectedDatabaseDefinition = newDatabaseDefinition;
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

        private void FilterSchemas_Executed()
        {
            IPickSchemasDialog dialog = _pickSchemasDialogFactory();
            dialog.AddSchemas(Schemas);
            var dialogResult = dialog.ShowAndAwaitUserResponse(true);
            if(dialogResult.ClosedByOK)
                Schemas = new List<SchemaInfo>(dialogResult.Payload);
        }

        private bool FilterSchemas_CanExecute() => FilterSchemas;

        private DatabaseDefinitionModel PreSelectDatabaseDefinition()
        {
            var subset = DatabaseDefinitions.Where(m => !string.IsNullOrWhiteSpace(m.FilePath) && m.FilePath.EndsWith(".sqlproj"))
                                            .ToArray();
            return subset.Any()
                       ? subset.OrderBy(m => Path.GetFileNameWithoutExtension(m.FilePath)).First()
                       : null;
        }
    }
}