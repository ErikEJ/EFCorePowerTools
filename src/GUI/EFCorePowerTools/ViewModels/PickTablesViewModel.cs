namespace EFCorePowerTools.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using System.Windows.Input;
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Shared.DAL;
    using Shared.Models;

    public class PickTablesViewModel : ViewModelBase, IPickTablesViewModel
    {
        private readonly IOperatingSystemAccess _operatingSystemAccess;
        private readonly IFileSystemAccess _fileSystemAccess;
        private readonly Func<ITableInformationViewModel> _tableInformationViewModelFactory;

        private bool _includeTables;
        private bool? _tableSelectionThreeState;
        private string _searchText;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand SaveSelectionCommand { get; }
        public ICommand LoadSelectionCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableCollection<ITableInformationViewModel> Tables { get; }

        public ICollectionView FilteredTables
        {
            get
            {
                var source = CollectionViewSource.GetDefaultView(Tables);
                source.Filter = t => FilterTable((ITableInformationViewModel) t);
                return source;
            }
        }

        public bool IncludeTables
        {
            get => _includeTables;
            set
            {
                if (Equals(value, _includeTables)) return;
                _includeTables = value;
                RaisePropertyChanged();
            }
        }

        public bool? TableSelectionThreeState
        {
            get => _tableSelectionThreeState;
            set
            {
                if (Equals(value, _tableSelectionThreeState)) return;
                _tableSelectionThreeState = value;
                RaisePropertyChanged();
                HandleTableSelectionThreeStateChange(value);
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (Equals(value, _searchText)) return;
                _searchText = value;
                RaisePropertyChanged();
                HandleSearchTextChange(value);
            }
        }

        public PickTablesViewModel(IOperatingSystemAccess operatingSystemAccess,
                                   IFileSystemAccess fileSystemAccess,
                                   Func<ITableInformationViewModel> tableInformationViewModelFactory)
        {
            _operatingSystemAccess = operatingSystemAccess ?? throw new ArgumentNullException(nameof(operatingSystemAccess));
            _fileSystemAccess = fileSystemAccess ?? throw new ArgumentNullException(nameof(fileSystemAccess));
            _tableInformationViewModelFactory = tableInformationViewModelFactory ?? throw new ArgumentNullException(nameof(tableInformationViewModelFactory));

            LoadedCommand = new RelayCommand(Loaded_Executed);
            SaveSelectionCommand = new RelayCommand(SaveSelection_Executed, SaveSelection_CanExecute);
            LoadSelectionCommand = new RelayCommand(LoadSelection_Executed, LoadSelection_CanExecute);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);
            
            Tables = new ObservableCollection<ITableInformationViewModel>();
            Tables.CollectionChanged += Tables_CollectionChanged;

            SearchText = string.Empty;
        }

        private void Loaded_Executed()
        {
            foreach (var t in Tables)
                PredefineSelection(t);
        }

        private void SaveSelection_Executed()
        {
            var resultFileName = _operatingSystemAccess.RequestSaveFileName("Save list of tables as",
                                                                            "Text file (*.txt)|*.txt|All Files(*.*)|*.*",
                                                                            true);
            if (resultFileName == null)
                return;

            _fileSystemAccess.WriteAllLines(resultFileName, Tables.Where(m => m.IsSelected && m.Model.HasPrimaryKey).Select(m => m.Model.UnsafeFullName));
        }

        private bool SaveSelection_CanExecute() => Tables.Any(m => m.IsSelected && m.Model.HasPrimaryKey);

        private void LoadSelection_Executed()
        {
            var resultFileName = _operatingSystemAccess.RequestLoadFileName("Select list of tables to load",
                                                                            "Text file (*.txt)|*.txt|All Files(*.*)|*.*",
                                                                            false,
                                                                            true);
            if (resultFileName == null)
                return;

            var lines = _fileSystemAccess.ReadAllLines(resultFileName);
            var parsedTables = new List<TableInformationModel>();
            foreach (var line in lines)
                if (TableInformationModel.TryParse(line, out var t))
                    parsedTables.Add(t);

            foreach (var t in Tables)
            {
                var parsedTable = parsedTables.SingleOrDefault(m => m.SafeFullName == t.Model.SafeFullName);
                t.IsSelected = parsedTable != null;
            }

            SearchText = string.Empty;
        }

        private bool LoadSelection_CanExecute() => Tables.Any();

        private void Ok_Executed()
        {
            foreach (var t in Tables)
                t.PropertyChanged -= TableViewModel_PropertyChanged;

            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => Tables.Any(m => m.IsSelected && m.Model.HasPrimaryKey);

        private void Cancel_Executed()
        {
            foreach (var t in Tables)
                t.PropertyChanged -= TableViewModel_PropertyChanged;

            Tables.Clear();
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        private void HandleTableSelectionThreeStateChange(bool? selectionMode)
        {
            if (selectionMode == null)
                return;

            foreach (var t in Tables)
                t.IsSelected = selectionMode.Value;

            SearchText = string.Empty;
        }

        private void UpdateTableSelectionThreeState()
        {
            TableSelectionThreeState = Tables.All(m => m.IsSelected)
                                           ? true
                                           : Tables.All(m => !m.IsSelected)
                                               ? (bool?)false
                                               : null;
        }

        private async void HandleSearchTextChange(string text)
        {
            await Task.Delay(500); // Add a delay (like a debounce) so that not every character change triggers a search
            if (text != SearchText)
                return;

            FilteredTables.Refresh();
        }

        private bool FilterTable(ITableInformationViewModel tableInformationViewModel)
        {
            return string.IsNullOrWhiteSpace(SearchText)
                || tableInformationViewModel.Model.UnsafeFullName.ToUpper().Contains(SearchText.ToUpper());
        }

        private static void PredefineSelection(ITableInformationViewModel t)
        {
            t.IsSelected = !t.Model.UnsafeFullName.StartsWith("__")
                        && !t.Model.UnsafeFullName.StartsWith("dbo.__")
                        && !t.Model.UnsafeFullName.EndsWith(".sysdiagrams");
        }

        void IPickTablesViewModel.AddTable(TableInformationModel table)
        {
            // Don't create a new ViewModel instance if the table parameter is null
#pragma warning disable IDE0016
            if (table == null) throw new ArgumentNullException(nameof(table));
#pragma warning restore IDE0016

            var tvm = _tableInformationViewModelFactory();
            tvm.Model = table;
            PredefineSelection(tvm);
            Tables.Add(tvm);
            tvm.PropertyChanged += TableViewModel_PropertyChanged;
            FilteredTables.Refresh();
        }

        TableInformationModel[] IPickTablesViewModel.GetResult()
        {
            return (from t in Tables.Where(m => m.Model.HasPrimaryKey)
                    where IncludeTables && t.IsSelected || !IncludeTables && !t.IsSelected
                    select t.Model)
               .ToArray();
        }

        private void Tables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTableSelectionThreeState();
        }

        private void TableViewModel_PropertyChanged(object sender,
                                                    PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ITableInformationViewModel.IsSelected))
                UpdateTableSelectionThreeState();
        }
    }
}