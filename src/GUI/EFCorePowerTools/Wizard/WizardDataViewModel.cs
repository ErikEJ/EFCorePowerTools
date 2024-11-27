// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.BLL;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.DAL;
using EFCorePowerTools.Locales;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.Wizard
{
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1124:DoNotUseRegions", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:Do not place regions within elements", Justification = "<Pending>")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "<Pending>")]
    /// <summary>
    /// WizardData will serve as view model for the wizard pages.
    /// </summary>
    public class WizardDataViewModel : ViewModelBase, IWizardViewModel
    {
        public WizardDataViewModel(
            IVisualStudioAccess visualStudioAccess,
            ICredentialStore credentialStore,
            Func<IPickSchemasDialog> pickSchemasDialogFactory,
            Func<IPickConnectionDialog> pickConnectionDialogFactory,
            IObjectTreeViewModel treeviewModel)
        {
            this.visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));
            this.pickSchemasDialogFactory = pickSchemasDialogFactory ?? throw new ArgumentNullException(nameof(pickSchemasDialogFactory));
            this.pickConnectionDialogFactory = pickConnectionDialogFactory ?? throw new ArgumentNullException(nameof(pickConnectionDialogFactory));
            this.credentialStore = credentialStore ?? throw new ArgumentNullException(nameof(credentialStore));

            #region WizardPage1 - Configuration / database connection
            Page1LoadedCommand = new RelayCommand(Page1Loaded_Executed);
            AddDatabaseConnectionCommand = new RelayCommand(AddDatabaseConnection_Executed);
            AddAdhocDatabaseConnectionCommand = new RelayCommand(AddAdhocDatabaseConnection_Executed);
            AddDatabaseDefinitionCommand = new RelayCommand(AddDatabaseDefinition_Executed);
            RemoveDatabaseConnectionCommand = new RelayCommand(RemoveDatabaseConnection_Executed, RemoveDatabaseConnection_CanExecute);
            FilterSchemasCommand = new RelayCommand(FilterSchemas_Executed, FilterSchemas_CanExecute);
            CodeGenerationModeList = [];
            DatabaseConnections = [];
            Schemas = new List<SchemaInfo>();
            DatabaseConnections.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(DatabaseConnections));
            #endregion

            #region WizardPage2 - Database Objects
            ObjectTree = treeviewModel;
            ObjectTree.ObjectSelectionChanged += (s, e) => UpdateTableSelectionThreeState();
            SearchText = string.Empty;
            #endregion
        }

        private readonly IVisualStudioAccess visualStudioAccess;
        private readonly ICredentialStore credentialStore;
        private readonly Func<IPickSchemasDialog> pickSchemasDialogFactory;
        private readonly Func<IPickConnectionDialog> pickConnectionDialogFactory;

        private DatabaseConnectionModel selectedDatabaseConnection;
        private bool filterSchemas = false;
        private string uiHint;
        private int codeGenerationMode;
        private ConfigModel selectedConfiguration;

        public string DataItem1 { get; set; }
        public string DataItem2 { get; set; }
        public string DataItem3 { get; set; }

        public IReverseEngineerBll Bll { get; set; }
        public Project Project { get; internal set; }
        public string Filename { get; internal set; }
        public bool OnlyGenerate { get; internal set; }
        public string OptionsPath { get; internal set; }
        public bool FromSqlProject { get; internal set; }

        #region //-- WizardPage1 - Configuration
        public ObservableCollection<ConfigModel> Configurations { get; set; } = [];

        public ConfigModel SelectedConfiguration
        {
            get => selectedConfiguration;
            set
            {
                if (Equals(value, selectedConfiguration))
                {
                    return;
                }

                selectedConfiguration = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #region //-- WizardPage1 - Database connection
        public ICommand Page1LoadedCommand { get; set; }
        public ICommand AddDatabaseConnectionCommand { get; set; }
        public ICommand AddAdhocDatabaseConnectionCommand { get; set; }
        public ICommand AddDatabaseDefinitionCommand { get; set; }
        public ICommand RemoveDatabaseConnectionCommand { get; set; }
        public RelayCommand FilterSchemasCommand { get; set; }

        public ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; set; } = [];
        public ObservableCollection<CodeGenerationItem> CodeGenerationModeList { get; set; } = [];

        public List<SchemaInfo> Schemas { get; set; } = [];

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
                FilterSchemasCommand.RaiseCanExecuteChanged();
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

        private void Page1Loaded_Executed()
        {
            // Database connection first
            if (DatabaseConnections.Any(c => c.FilePath == null) && SelectedDatabaseConnection == null)
            {
                if (!string.IsNullOrEmpty(UiHint))
                {
                    var candidate = DatabaseConnections
                        .FirstOrDefault(m => m.ConnectionName == UiHint);

                    if (candidate != null)
                    {
                        SelectedDatabaseConnection = candidate;
                        return;
                    }
                }

                SelectedDatabaseConnection = DatabaseConnections[0];
                return;
            }

            // Database definitions (SQL project) second
            if (DatabaseConnections.Any(c => c.FilePath != null && SelectedDatabaseConnection == null))
            {
                SelectedDatabaseConnection = PreSelectDatabaseDefinition(UiHint);
                if (SelectedDatabaseConnection is null && DatabaseConnections.Any())
                {
                    SelectedDatabaseConnection = DatabaseConnections[0];
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

        private bool Ok_CanExecute() => SelectedDatabaseConnection != null;

        private bool RemoveDatabaseConnection_CanExecute() => SelectedDatabaseConnection != null && SelectedDatabaseConnection.FilePath == null;

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

        private bool FilterSchemas_CanExecute()
        {
            return FilterSchemas;
        }

        private DatabaseConnectionModel PreSelectDatabaseDefinition(string uiHint)
        {
            var subset = DatabaseConnections
                .Where(m => !string.IsNullOrWhiteSpace(m.FilePath) && m.FilePath.EndsWith(".sqlproj"))
                .ToList();

            if (!string.IsNullOrEmpty(uiHint) && uiHint.EndsWith(".sqlproj"))
            {
                var candidate = subset.Find(m => m.FilePath.Equals(uiHint));

                if (candidate != null)
                {
                    return candidate;
                }
            }

            return subset.Any()
                       ? subset.OrderBy(m => Path.GetFileNameWithoutExtension(m.FilePath)).First()
                       : null;
        }
        #endregion

        #region //-- WizardPage2 - Database objects
        public RelayCommand Page2LoadedCommand { get; set; }
        private bool? tableSelectionThreeState;
        private string searchText;
        private SearchMode searchMode = SearchMode.Text;

        public IObjectTreeViewModel ObjectTree { get; set; }

        public bool? TableSelectionThreeState
        {
            get => tableSelectionThreeState;
            set
            {
                if (Equals(value, tableSelectionThreeState))
                {
                    return;
                }

                tableSelectionThreeState = value;
                RaisePropertyChanged();
                HandleTableSelectionThreeStateChange(value);
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                if (Equals(value, searchText))
                {
                    return;
                }

                searchText = value;
                RaisePropertyChanged();
                HandleSearchTextChange(searchText, searchMode);
            }
        }

        public SearchMode SearchMode
        {
            get => searchMode;
            set
            {
                if (Equals(value, searchMode))
                {
                    return;
                }

                searchMode = value;
                RaisePropertyChanged();
                HandleSearchTextChange(searchText, searchMode);
            }
        }

        public void AddObjects(IEnumerable<TableModel> objects, IEnumerable<Schema> customReplacers)
        {
            if (objects == null)
            {
                return;
            }

            ObjectTree.AddObjects(objects, customReplacers);
        }

        public void SelectObjects(IEnumerable<SerializationTableModel> objects)
        {
            if (objects == null)
            {
                return;
            }

            ObjectTree.SelectObjects(objects);
        }

        public SerializationTableModel[] GetSelectedObjects()
        {
            return ObjectTree
                .GetSelectedObjects()
                .ToArray();
        }

        public Schema[] GetRenamedObjects()
        {
            return ObjectTree
                .GetRenamedObjects()
                .ToArray();
        }

        // private bool Ok_CanExecute()
        //    => ObjectTree.GetSelectedObjects().Any(c => c.ObjectType.HasColumns())
        // || ObjectTree.GetSelectedObjects().Any(c => c.ObjectType == ObjectType.Procedure)
        // || ObjectTree.GetSelectedObjects().Any(c => c.ObjectType == ObjectType.ScalarFunction)
        private void Cancel_Executed()
        {
            // CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        private void HandleTableSelectionThreeStateChange(bool? selectionMode)
        {
            if (selectionMode == null)
            {
                return;
            }

            ObjectTree.SetSelectionState(selectionMode.Value);
            SearchText = string.Empty;
        }

        private void HandleSearchTextChange(string text, SearchMode searchMode)
        {
            _ = Task.Delay(TimeSpan.FromMilliseconds(300))
                .ContinueWith(
                _ =>
                {
                    if (text != SearchText)
                    {
                        return;
                    }

                    ObjectTree.Search(SearchText, searchMode);
                },
                TaskScheduler.Default);
        }

        private void UpdateTableSelectionThreeState()
        {
            TableSelectionThreeState = ObjectTree.GetSelectionState();
        }

        #endregion

    }
}
