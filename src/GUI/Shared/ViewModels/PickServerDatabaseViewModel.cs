namespace EFCorePowerTools.ViewModels
{
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using Contracts.Views;
    using EFCorePowerTools.DAL;
    using EFCorePowerTools.Locales;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using RevEng.Shared;
    using Shared.DAL;
    using Shared.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;

    public class PickServerDatabaseViewModel : ViewModelBase, IPickServerDatabaseViewModel
    {
        private readonly IVisualStudioAccess _visualStudioAccess;
        private readonly ICredentialStore _credentialStore;
        private readonly Func<IPickSchemasDialog> _pickSchemasDialogFactory;
        private readonly Func<IPickConnectionDialog> _pickConnectionDialogFactory;

        private DatabaseConnectionModel _selectedDatabaseConnection;
        private DatabaseDefinitionModel _selectedDatabaseDefinition;
        private bool _filterSchemas = false;
        private string _uiHint;
        private int _codeGenerationMode;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand AddDatabaseConnectionCommand { get; }
        public ICommand AddAdhocDatabaseConnectionCommand { get; }
        public ICommand RemoveDatabaseConnectionCommand { get; }
        public ICommand AddDatabaseDefinitionCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand FilterSchemasCommand { get; }

        public ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; }
        public ObservableCollection<DatabaseDefinitionModel> DatabaseDefinitions { get; }

        public List<SchemaInfo> Schemas { get; private set; }
        public IReadOnlyList<string> CodeGenerationModeList { get; }

        public int CodeGenerationMode
        {
            get => _codeGenerationMode;
            set
            {
                if (value == _codeGenerationMode) return;
                _codeGenerationMode = value;
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

        public string UiHint
        {
            get
            {
                if (SelectedDatabaseConnection != null)
                {
                    return SelectedDatabaseConnection.ConnectionName;
                }
                if (SelectedDatabaseDefinition != null)
                {
                    return SelectedDatabaseDefinition.FilePath;
                }

                return _uiHint;  
            } 
            set
            {
                var databaseConnectionCandidate = DatabaseConnections
                    .FirstOrDefault(c => c.ConnectionName.ToLowerInvariant() == value.ToLowerInvariant());

                if (databaseConnectionCandidate != null)
                {
                    SelectedDatabaseConnection = databaseConnectionCandidate;
                }

                _uiHint = value;
            }
        }

        public PickServerDatabaseViewModel(IVisualStudioAccess visualStudioAccess, 
            ICredentialStore credentialStore,
            Func<IPickSchemasDialog> pickSchemasDialogFactory,
            Func<IPickConnectionDialog> pickConnectionDialogFactory)
        {
            _visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));
            _pickSchemasDialogFactory = pickSchemasDialogFactory ?? throw new ArgumentNullException(nameof(pickSchemasDialogFactory));
            _pickConnectionDialogFactory = pickConnectionDialogFactory ?? throw new ArgumentNullException(nameof(pickConnectionDialogFactory));
            _credentialStore = credentialStore ?? throw new ArgumentNullException(nameof(credentialStore));

            LoadedCommand = new RelayCommand(Loaded_Executed);
            AddDatabaseConnectionCommand = new RelayCommand(AddDatabaseConnection_Executed);
            AddAdhocDatabaseConnectionCommand = new RelayCommand(AddAdhocDatabaseConnection_Executed);
            AddDatabaseDefinitionCommand = new RelayCommand(AddDatabaseDefinition_Executed);
            RemoveDatabaseConnectionCommand = new RelayCommand(RemoveDatabaseConnection_Executed, RemoveDatabaseConnection_CanExecute);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);
            FilterSchemasCommand = new RelayCommand(FilterSchemas_Executed, FilterSchemas_CanExecute);

            CodeGenerationModeList = new[]
            {
                "EF Core 5",
                "EF Core 3",
                "EF Core 6",
            };

            DatabaseConnections = new ObservableCollection<DatabaseConnectionModel>();
            DatabaseDefinitions = new ObservableCollection<DatabaseDefinitionModel>();
            Schemas = new List<SchemaInfo>();
            DatabaseDefinitions.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(DatabaseDefinitions));
        }

        private void Loaded_Executed()
        {
            // Database connection first
            if (DatabaseConnections.Any() && SelectedDatabaseConnection == null)
            {
                if (DatabaseDefinitions.Any() 
                    && !string.IsNullOrEmpty(UiHint)
                    && UiHint.EndsWith(".sqlproj")
                )
                {
                    var candidate = DatabaseDefinitions
                        .FirstOrDefault(m => !string.IsNullOrWhiteSpace(m.FilePath)
                            && m.FilePath.EndsWith(".sqlproj")
                            && m.FilePath.Equals(UiHint));

                    if (candidate != null)
                    {
                        SelectedDatabaseDefinition = candidate;
                        return;
                    }
                }

                SelectedDatabaseConnection = DatabaseConnections.First();
                return;
            }

            // Database definition (SQL project) first
            if (DatabaseDefinitions.Any() && SelectedDatabaseConnection == null)
            {
                SelectedDatabaseDefinition = PreSelectDatabaseDefinition(UiHint);
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
                _visualStudioAccess.ShowMessage($"{ReverseEngineerLocale.UnableToAddConnection}: {e.Message}");
                return;
            }

            if (newDatabaseConnection == null)
                return;

            DatabaseConnections.Add(newDatabaseConnection);
            SelectedDatabaseConnection = newDatabaseConnection;
        }

        private void AddAdhocDatabaseConnection_Executed()
        {
            DatabaseConnectionModel newDatabaseConnection = null;

            IPickConnectionDialog dialog = _pickConnectionDialogFactory();
            var dialogResult = dialog.ShowAndAwaitUserResponse(true);
            if (dialogResult.ClosedByOK)
            {
                newDatabaseConnection = dialogResult.Payload;
            }

            if (newDatabaseConnection == null)
                return;

            try
            {
                _credentialStore.SaveCredential(newDatabaseConnection);
            }
            catch (Exception e)
            {
                _visualStudioAccess.ShowMessage($"{ReverseEngineerLocale.UnableToAddConnection}: {e.Message}");
                return;
            }

            DatabaseConnections.Add(newDatabaseConnection);
            SelectedDatabaseConnection = newDatabaseConnection;
        }

        private void RemoveDatabaseConnection_Executed()
        {
            if (SelectedDatabaseConnection == null)
            {
                return;
            }

            try
            {
                if (SelectedDatabaseConnection.DataConnection is null)
                {
                    _credentialStore.DeleteCredential(SelectedDatabaseConnection.ConnectionName);
                }
                else
                {
                    _visualStudioAccess.RemoveDatabaseConnection(SelectedDatabaseConnection.DataConnection);
                }
                
                DatabaseConnections.Remove(SelectedDatabaseConnection);
            }
            catch (Exception e)
            {
                _visualStudioAccess.ShowMessage($"{ReverseEngineerLocale.UnableToRemoveConnection}: {e.Message}");
                return;
            }

            SelectedDatabaseConnection = null;
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
                _visualStudioAccess.ShowMessage($"{ReverseEngineerLocale.UnableToAddToList}: {e.Message}");
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

        private bool RemoveDatabaseConnection_CanExecute() => SelectedDatabaseConnection != null;

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

        private DatabaseDefinitionModel PreSelectDatabaseDefinition(string uiHint)
        {
            var subset = DatabaseDefinitions
                .Where(m => !string.IsNullOrWhiteSpace(m.FilePath) && m.FilePath.EndsWith(".sqlproj"))
                .ToArray();
            if (!string.IsNullOrEmpty(uiHint) && uiHint.EndsWith(".sqlproj"))
            {
                var candidate = subset
                    .FirstOrDefault(m => m.FilePath.Equals(uiHint));

                if (candidate != null)
                {
                    return candidate;
                }
            }

            return subset.Any()
                       ? subset.OrderBy(m => Path.GetFileNameWithoutExtension(m.FilePath)).First()
                       : null;
        }
    }
}