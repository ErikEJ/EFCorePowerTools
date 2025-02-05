using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.BLL;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.DAL;
using EFCorePowerTools.Locales;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.ViewModels
{
    /// <summary>
    /// WizardData will serve as view model for the wizard pages.
    /// </summary>
    public class WizardDataViewModel : ViewModelBase, IWizardViewModel
    {
        public WizardDataViewModel(
            IServiceProvider provider,
            ICredentialStore credentialStore,
            IObjectTreeViewModel treeviewModel,
            IVisualStudioAccess visualStudioAccess,
            Func<IPickSchemasDialog> pickSchemasDialogFactory,
            Func<IPickConnectionDialog> pickConnectionDialogFactory,
            Func<IAdvancedModelingOptionsDialog> advancedModelingOptionsDialogFactory)
        {
            this.visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));
            this.pickSchemasDialogFactory = pickSchemasDialogFactory ?? throw new ArgumentNullException(nameof(pickSchemasDialogFactory));
            this.pickConnectionDialogFactory = pickConnectionDialogFactory ?? throw new ArgumentNullException(nameof(pickConnectionDialogFactory));
            this.credentialStore = credentialStore ?? throw new ArgumentNullException(nameof(credentialStore));
            this.serviceProvider = provider;

            // WizardPage1 - Configuration / database connection
            Page1LoadedCommand = new RelayCommand(Page1Loaded_Executed);
            AddDatabaseConnectionCommand = new RelayCommand(AddDatabaseConnection_Executed);
            AddAdhocDatabaseConnectionCommand = new RelayCommand(AddAdhocDatabaseConnection_Executed);
            AddDatabaseDefinitionCommand = new RelayCommand(AddDatabaseDefinition_Executed);
            RemoveDatabaseConnectionCommand = new RelayCommand(RemoveDatabaseConnection_Executed, RemoveDatabaseConnection_CanExecute);
            FilterSchemasCommand = new RelayCommand(FilterSchemas_Executed, FilterSchemas_CanExecute);

            CodeGenerationModeList = new ObservableCollection<CodeGenerationItem>();

            DatabaseConnections = new ObservableCollection<DatabaseConnectionModel>();
            Schemas = new List<SchemaInfo>();
            DatabaseConnections.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(DatabaseConnections));

            // WizardPage2 - Database Objects
            ObjectTree = treeviewModel;
            ObjectTree.ObjectSelectionChanged += (s, e) => UpdateTableSelectionThreeState();
            SearchText = string.Empty;

            // WizardPage3 - Modeling Options
            this.advancedModelingOptionsDialogFactory = advancedModelingOptionsDialogFactory;

            MayIncludeConnectionString = true;
            AdvancedCommand = new RelayCommand(Advanced_Executed);
            Model = new ModelingOptionsModel();
            Model.PropertyChanged += Model_PropertyChanged;

            TemplateTypeList = [];

            GenerationModeList =
            [
                ReverseEngineerLocale.EntityTypesAndContext,
                ReverseEngineerLocale.DbContextOnly,
                ReverseEngineerLocale.EntityTypesOnly,
            ];
        }

#pragma warning disable SA1201 // Elements should appear in the correct order
        private readonly IServiceProvider serviceProvider;
        private readonly IVisualStudioAccess visualStudioAccess;
        private readonly ICredentialStore credentialStore;
        private readonly Func<IPickSchemasDialog> pickSchemasDialogFactory;
        private readonly Func<IPickConnectionDialog> pickConnectionDialogFactory;
        private readonly Func<IAdvancedModelingOptionsDialog> advancedModelingOptionsDialogFactory;

        private DatabaseConnectionModel selectedDatabaseConnection;
        private bool filterSchemas = false;
        private string uiHint;
        private int codeGenerationMode;
        private ConfigModel selectedConfiguration;

        private string title = "Hello world";
        private bool mayIncludeConnectionString;
        private int selectedTemplateType;

        public IServiceProvider ServiceProvider
        {
            get { return serviceProvider; }
        }

        public IReverseEngineerBll Bll { get; set; }

        public string DialogResult { get; set; } = "DialogResult in WizardDataViewModel";

        public Project Project { get; internal set; }
        public string Filename { get; internal set; }
        public bool OnlyGenerate { get; internal set; }
        public string OptionsPath { get; internal set; }
        public bool FromSqlProject { get; internal set; }

        // WizardPage1 - Configuration and DatabaseObjects
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

        public bool IsPage1Initialized { get; set; }
        public ICommand Page1LoadedCommand { get; set; }
        public ICommand AddDatabaseConnectionCommand { get; set; }
        public ICommand AddAdhocDatabaseConnectionCommand { get; set; }
        public ICommand AddDatabaseDefinitionCommand { get; set; }
        public RelayCommand RemoveDatabaseConnectionCommand { get; set; }
        public RelayCommand FilterSchemasCommand { get; set; }

        public ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; }
        public ObservableCollection<CodeGenerationItem> CodeGenerationModeList { get; }

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
                this.ObjectTree.Types.Clear();
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

        private bool RemoveDatabaseConnection_CanExecute()
        {
            return SelectedDatabaseConnection != null && SelectedDatabaseConnection.FilePath == null;
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

        // WizardPage2 - Database objects
        public bool IsPage2Initialized { get; set; }
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

        public WizardEventArgs WizardEventArgs { get; set; }

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

        // WizardPage3 - Modeling Options / Advanced
        public bool IsPage3Initialized { get; set; }

        public RelayCommand Page3LoadedCommand { get; set; }

        public ICommand AdvancedCommand { get; }

        public ModelingOptionsModel Model { get; }
        public IReadOnlyList<string> GenerationModeList { get; }
        public IReadOnlyList<string> HandlebarsLanguageList { get; }

        public ObservableCollection<TemplateTypeItem> TemplateTypeList { get; }

        public int SelectedTemplateType
        {
            get => selectedTemplateType;
            set
            {
                if (value == selectedTemplateType)
                {
                    return;
                }

                selectedTemplateType = value;
                Model.SelectedHandlebarsLanguage = selectedTemplateType;
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get => title;
            set
            {
                if (value == title)
                {
                    return;
                }

                title = value;
                RaisePropertyChanged();
            }
        }

        public bool MayIncludeConnectionString
        {
            get => mayIncludeConnectionString;
            private set
            {
                if (value == mayIncludeConnectionString)
                {
                    return;
                }

                mayIncludeConnectionString = value;
                RaisePropertyChanged();
            }
        }

        public void ApplyPresets(ModelingOptionsModel presets)
        {
            Model.InstallNuGetPackage = presets.InstallNuGetPackage;
            Model.SelectedToBeGenerated = presets.SelectedToBeGenerated;
            Model.SelectedHandlebarsLanguage = presets.SelectedHandlebarsLanguage;
            Model.IncludeConnectionString = presets.IncludeConnectionString;
            Model.UseHandlebars = presets.UseHandlebars;
            Model.UsePluralizer = presets.UsePluralizer;
            Model.UseDatabaseNames = presets.UseDatabaseNames;
            Model.Namespace = presets.Namespace;
            Model.OutputPath = presets.OutputPath;
            Model.OutputContextPath = presets.OutputContextPath;
            Model.UseSchemaFolders = presets.UseSchemaFolders;
            Model.ModelNamespace = presets.ModelNamespace;
            Model.ContextNamespace = presets.ContextNamespace;
            Model.ModelName = presets.ModelName;
            Model.UseDataAnnotations = presets.UseDataAnnotations;
            Model.UseDbContextSplitting = presets.UseDbContextSplitting;
            Model.ProjectName = presets.ProjectName;
            Model.DacpacPath = presets.DacpacPath;
            Model.MapSpatialTypes = presets.MapSpatialTypes;
            Model.MapHierarchyId = presets.MapHierarchyId;
            Model.MapNodaTimeTypes = presets.MapNodaTimeTypes;
            Model.UseEf6Pluralizer = presets.UseEf6Pluralizer;
            Model.UseBoolPropertiesWithoutDefaultSql = presets.UseBoolPropertiesWithoutDefaultSql;
            Model.UseNoDefaultConstructor = presets.UseNoDefaultConstructor;
            Model.UseNoNavigations = presets.UseNoNavigations;
            Model.UseNullableReferences = presets.UseNullableReferences;
            Model.UseNoObjectFilter = presets.UseNoObjectFilter;
            Model.UseManyToManyEntity = presets.UseManyToManyEntity;
            Model.UseDateOnlyTimeOnly = presets.UseDateOnlyTimeOnly;
            Model.UseSchemaNamespaces = presets.UseSchemaNamespaces;
            Model.T4TemplatePath = presets.T4TemplatePath;

            Title = string.Format(ReverseEngineerLocale.GenerateEFCoreModelInProject, Model.ProjectName);
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ModelingOptionsModel.DacpacPath):
                    if (!string.IsNullOrWhiteSpace(Model.DacpacPath))
                    {
                        MayIncludeConnectionString = false;
                        Model.IncludeConnectionString = false;
                    }
                    else
                    {
                        MayIncludeConnectionString = true;
                    }

                    break;

                case nameof(ModelingOptionsModel.SelectedToBeGenerated):
                    if (Model.InstallNuGetPackage && Model.SelectedToBeGenerated == 2)
                    {
                        Model.InstallNuGetPackage = false;
                    }

                    break;
            }
        }

        private void Advanced_Executed()
        {
            IAdvancedModelingOptionsDialog dialog = advancedModelingOptionsDialogFactory();
            dialog.ApplyPresets(Model);
            var advancedModelingOptionsResult = dialog.ShowAndAwaitUserResponse(true);
            if (!advancedModelingOptionsResult.ClosedByOK)
            {
                return;
            }

            Model.UseDbContextSplitting = advancedModelingOptionsResult.Payload.UseDbContextSplitting;
            Model.MapSpatialTypes = advancedModelingOptionsResult.Payload.MapSpatialTypes;
            Model.MapHierarchyId = advancedModelingOptionsResult.Payload.MapHierarchyId;
            Model.MapNodaTimeTypes = advancedModelingOptionsResult.Payload.MapNodaTimeTypes;
            Model.UseEf6Pluralizer = advancedModelingOptionsResult.Payload.UseEf6Pluralizer;
            Model.UseBoolPropertiesWithoutDefaultSql = advancedModelingOptionsResult.Payload.UseBoolPropertiesWithoutDefaultSql;
            Model.UseNoDefaultConstructor = advancedModelingOptionsResult.Payload.UseNoDefaultConstructor;
            Model.UseNullableReferences = advancedModelingOptionsResult.Payload.UseNullableReferences;
            Model.UseNoObjectFilter = advancedModelingOptionsResult.Payload.UseNoObjectFilter;
            Model.UseNoNavigations = advancedModelingOptionsResult.Payload.UseNoNavigations;
            Model.UseSchemaFolders = advancedModelingOptionsResult.Payload.UseSchemaFolders;
            Model.UseManyToManyEntity = advancedModelingOptionsResult.Payload.UseManyToManyEntity;
            Model.UseDateOnlyTimeOnly = advancedModelingOptionsResult.Payload.UseDateOnlyTimeOnly;
            Model.ContextNamespace = advancedModelingOptionsResult.Payload.ContextNamespace;
            Model.OutputContextPath = advancedModelingOptionsResult.Payload.OutputContextPath;
            Model.ModelNamespace = advancedModelingOptionsResult.Payload.ModelNamespace;
            Model.UseSchemaNamespaces = advancedModelingOptionsResult.Payload.UseSchemaNamespaces;
            Model.T4TemplatePath = advancedModelingOptionsResult.Payload.T4TemplatePath;
        }

        public bool IsPage4Initialized { get; set; }

        public RelayCommand Page4LoadedCommand { get; set; }

        private string generateStatus = string.Empty;
#pragma warning restore SA1201 // Elements should appear in the correct order
        public string GenerateStatus
        {
            get
            {
                return generateStatus;
            }
            set
            {
                generateStatus = value;
                this.RaisePropertyChanged("GenerateStatus");
            }
        }

        public string ErrorMessage { get; internal set; }
    }
}
