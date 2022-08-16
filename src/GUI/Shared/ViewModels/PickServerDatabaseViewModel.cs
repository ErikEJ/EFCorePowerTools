using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.DAL;
using EFCorePowerTools.Locales;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RevEng.Common;

namespace EFCorePowerTools.ViewModels
{
    public class PickServerDatabaseViewModel : ViewModelBase, IPickServerDatabaseViewModel
    {
        private readonly IVisualStudioAccess visualStudioAccess;
        private readonly ICredentialStore credentialStore;
        private readonly Func<IPickSchemasDialog> pickSchemasDialogFactory;
        private readonly Func<IPickConnectionDialog> pickConnectionDialogFactory;

        private DatabaseConnectionModel selectedDatabaseConnection;
        private DatabaseDefinitionModel selectedDatabaseDefinition;
        private bool filterSchemas = false;
        private string uiHint;
        private int codeGenerationMode;

        public PickServerDatabaseViewModel(
            IVisualStudioAccess visualStudioAccess,
            ICredentialStore credentialStore,
            Func<IPickSchemasDialog> pickSchemasDialogFactory,
            Func<IPickConnectionDialog> pickConnectionDialogFactory)
        {
            this.visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));
            this.pickSchemasDialogFactory = pickSchemasDialogFactory ?? throw new ArgumentNullException(nameof(pickSchemasDialogFactory));
            this.pickConnectionDialogFactory = pickConnectionDialogFactory ?? throw new ArgumentNullException(nameof(pickConnectionDialogFactory));
            this.credentialStore = credentialStore ?? throw new ArgumentNullException(nameof(credentialStore));

            LoadedCommand = new RelayCommand(Loaded_Executed);
            AddDatabaseConnectionCommand = new RelayCommand(AddDatabaseConnection_Executed);
            AddAdhocDatabaseConnectionCommand = new RelayCommand(AddAdhocDatabaseConnection_Executed);
            AddDatabaseDefinitionCommand = new RelayCommand(AddDatabaseDefinition_Executed);
            RemoveDatabaseConnectionCommand = new RelayCommand(RemoveDatabaseConnection_Executed, RemoveDatabaseConnection_CanExecute);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);
            FilterSchemasCommand = new RelayCommand(FilterSchemas_Executed, FilterSchemas_CanExecute);

            CodeGenerationModeList = new ObservableCollection<CodeGenerationItem>();

            DatabaseConnections = new ObservableCollection<DatabaseConnectionModel>();
            DatabaseDefinitions = new ObservableCollection<DatabaseDefinitionModel>();
            Schemas = new List<SchemaInfo>();
            DatabaseDefinitions.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(DatabaseDefinitions));
        }

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
        public ObservableCollection<CodeGenerationItem> CodeGenerationModeList { get; }

        public List<SchemaInfo> Schemas { get; private set; }

        public int CodeGenerationMode
        {
            get => codeGenerationMode;
            set
            {
                if (value == codeGenerationMode)
                {
                    return;
                }

                codeGenerationMode = value;
                RaisePropertyChanged();
            }
        }

        public bool FilterSchemas
        {
            get => filterSchemas;
            set
            {
                if (value == filterSchemas)
                {
                    return;
                }

                filterSchemas = value;
                RaisePropertyChanged();
            }
        }

        public DatabaseConnectionModel SelectedDatabaseConnection
        {
            get => selectedDatabaseConnection;
            set
            {
                if (Equals(value, selectedDatabaseConnection))
                {
                    return;
                }

                selectedDatabaseConnection = value;
                RaisePropertyChanged();

                if (selectedDatabaseConnection != null)
                {
                    SelectedDatabaseDefinition = null;
                }
            }
        }

        public DatabaseDefinitionModel SelectedDatabaseDefinition
        {
            get => selectedDatabaseDefinition;
            set
            {
                if (Equals(value, selectedDatabaseDefinition))
                {
                    return;
                }

                selectedDatabaseDefinition = value;
                RaisePropertyChanged();

                if (selectedDatabaseDefinition != null)
                {
                    SelectedDatabaseConnection = null;
                }
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

                return uiHint;
            }

            set
            {
                var databaseConnectionCandidate = DatabaseConnections
                    .FirstOrDefault(c => c.ConnectionName.ToLowerInvariant() == value.ToLowerInvariant());

                if (databaseConnectionCandidate != null)
                {
                    SelectedDatabaseConnection = databaseConnectionCandidate;
                }

                uiHint = value;
            }
        }

        private void Loaded_Executed()
        {
            // Database connection first
            if (DatabaseConnections.Any() && SelectedDatabaseConnection == null)
            {
                if (DatabaseDefinitions.Any()
                    && !string.IsNullOrEmpty(UiHint)
                    && UiHint.EndsWith(".sqlproj"))
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
                newDatabaseConnection = visualStudioAccess.PromptForNewDatabaseConnection();
            }
            catch (Exception e)
            {
                visualStudioAccess.ShowMessage($"{ReverseEngineerLocale.UnableToAddConnection}: {e.Message}");
                return;
            }

            if (newDatabaseConnection == null)
            {
                return;
            }

            DatabaseConnections.Add(newDatabaseConnection);
            SelectedDatabaseConnection = newDatabaseConnection;
        }

        private void AddAdhocDatabaseConnection_Executed()
        {
            DatabaseConnectionModel newDatabaseConnection = null;

            IPickConnectionDialog dialog = pickConnectionDialogFactory();
            var dialogResult = dialog.ShowAndAwaitUserResponse(true);
            if (dialogResult.ClosedByOK)
            {
                newDatabaseConnection = dialogResult.Payload;
            }

            if (newDatabaseConnection == null)
            {
                return;
            }

            try
            {
                credentialStore.SaveCredential(newDatabaseConnection);
            }
            catch (Exception e)
            {
                visualStudioAccess.ShowMessage($"{ReverseEngineerLocale.UnableToAddConnection}: {e.Message}");
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
                    credentialStore.DeleteCredential(SelectedDatabaseConnection.ConnectionName);
                }
                else
                {
                    visualStudioAccess.RemoveDatabaseConnection(SelectedDatabaseConnection.DataConnection);
                }

                DatabaseConnections.Remove(SelectedDatabaseConnection);
            }
            catch (Exception e)
            {
                visualStudioAccess.ShowMessage($"{ReverseEngineerLocale.UnableToRemoveConnection}: {e.Message}");
                return;
            }

            SelectedDatabaseConnection = null;
        }

        private void AddDatabaseDefinition_Executed()
        {
            DatabaseDefinitionModel newDatabaseDefinition;
            try
            {
                newDatabaseDefinition = visualStudioAccess.PromptForNewDatabaseDefinition();
            }
            catch (Exception e)
            {
                visualStudioAccess.ShowMessage($"{ReverseEngineerLocale.UnableToAddToList}: {e.Message}");
                return;
            }

            if (newDatabaseDefinition == null)
            {
                return;
            }

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
            IPickSchemasDialog dialog = pickSchemasDialogFactory();
            dialog.AddSchemas(Schemas);
            var dialogResult = dialog.ShowAndAwaitUserResponse(true);
            if (dialogResult.ClosedByOK)
            {
                Schemas = new List<SchemaInfo>(dialogResult.Payload);
            }
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
