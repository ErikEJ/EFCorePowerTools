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
using Microsoft.VisualStudio.Shell;
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
            Schemas = new List<SchemaInfo>();
            DatabaseConnections.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(DatabaseConnections));
        }

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand AddDatabaseConnectionCommand { get; }
        public ICommand AddAdhocDatabaseConnectionCommand { get; }
        public ICommand AddDatabaseDefinitionCommand { get; }
        public ICommand RemoveDatabaseConnectionCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand FilterSchemasCommand { get; }

        public ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; }
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
            }
        }

        public string UiHint
        {
            get
            {
                if (SelectedDatabaseConnection != null)
                {
                    return SelectedDatabaseConnection.ConnectionName ?? selectedDatabaseConnection.FilePath;
                }

                return uiHint;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var databaseConnectionCandidate = DatabaseConnections
                        .FirstOrDefault(c => c.ConnectionName != null && c.ConnectionName.Equals(value, StringComparison.InvariantCultureIgnoreCase));

                    if (databaseConnectionCandidate != null)
                    {
                        SelectedDatabaseConnection = databaseConnectionCandidate;
                    }

                    databaseConnectionCandidate = DatabaseConnections
                        .FirstOrDefault(c => c.FilePath != null && c.FilePath.Equals(value, StringComparison.InvariantCultureIgnoreCase));

                    if (databaseConnectionCandidate != null)
                    {
                        SelectedDatabaseConnection = databaseConnectionCandidate;
                    }
                }

                uiHint = value;
            }
        }

        private void Loaded_Executed()
        {
            // Database connection first
            if (DatabaseConnections.Any(c => c.FilePath == null)
                && !string.IsNullOrEmpty(UiHint)
                && SelectedDatabaseConnection == null)
            {
                var candidate = DatabaseConnections
                    .FirstOrDefault(m => m.ConnectionName == UiHint);

                if (candidate != null)
                {
                    SelectedDatabaseConnection = candidate;
                    return;
                }
            }

            // Database definitions (SQL project) second
            if (DatabaseConnections.Any(c => !string.IsNullOrWhiteSpace(c.FilePath)
                && c.FilePath.EndsWith(".sqlproj")))
            {
                var candidate = DatabaseConnections
                    .FirstOrDefault(m => !string.IsNullOrWhiteSpace(m.FilePath)
                        && m.FilePath.EndsWith(".sqlproj", StringComparison.OrdinalIgnoreCase)
                        && m.FilePath.Equals(uiHint));

                if (candidate != null)
                {
                    SelectedDatabaseConnection = candidate;
                }
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
                if (SelectedDatabaseConnection.DataConnection is null
                    && SelectedDatabaseConnection.FilePath is null)
                {
                    credentialStore.DeleteCredential(SelectedDatabaseConnection.ConnectionName);
                }
                else
                {
                    ThreadHelper.JoinableTaskFactory.Run(async () =>
                    {
                        await visualStudioAccess.RemoveDatabaseConnectionAsync(SelectedDatabaseConnection.DataConnection);
                    });
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
            DatabaseConnectionModel newDatabaseDefinition;
            try
            {
                newDatabaseDefinition = visualStudioAccess.PromptForNewDacpacDatabaseDefinition();
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

            DatabaseConnections.Add(newDatabaseDefinition);
            SelectedDatabaseConnection = newDatabaseDefinition;
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => SelectedDatabaseConnection != null;

        private bool RemoveDatabaseConnection_CanExecute() => SelectedDatabaseConnection != null && SelectedDatabaseConnection.FilePath == null;

        private void Cancel_Executed()
        {
            SelectedDatabaseConnection = null;
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
    }
}
