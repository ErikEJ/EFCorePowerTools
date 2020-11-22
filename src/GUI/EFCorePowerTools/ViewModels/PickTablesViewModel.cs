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
    using RevEng.Shared;
    using Shared.DAL;
    using Shared.Models;

    public class PickTablesViewModel : ViewModelBase, IPickTablesViewModel
    {
        private readonly Func<ITableInformationViewModel> _tableInformationViewModelFactory;
        private readonly Func<IColumnInformationViewModel> _columnInformationViewModelFactory;

        private bool? _tableSelectionThreeState;
        private string _searchText;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand TableSelectionCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableCollection<ITableInformationViewModel> Tables { get; }

        public ICollectionView FilteredTables { get; }

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
                HandleSearchTextChangeAsync(value);
            }
        }

        public PickTablesViewModel(Func<ITableInformationViewModel> tableInformationViewModelFactory,
                                   Func<IColumnInformationViewModel> columnInformationViewModelFactory)
        {
            _tableInformationViewModelFactory = tableInformationViewModelFactory ?? throw new ArgumentNullException(nameof(tableInformationViewModelFactory));
            _columnInformationViewModelFactory = columnInformationViewModelFactory ?? throw new ArgumentNullException(nameof(columnInformationViewModelFactory));

            LoadedCommand = new RelayCommand(Loaded_Executed);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);

            Tables = new ObservableCollection<ITableInformationViewModel>();
            Tables.CollectionChanged += Tables_CollectionChanged;

            var filteredTablesSource = CollectionViewSource.GetDefaultView(Tables);
            filteredTablesSource.Filter = t => FilterTable((ITableInformationViewModel)t);
            FilteredTables = filteredTablesSource;

            SearchText = string.Empty;
        }

        private void Loaded_Executed()
        {
            foreach (var t in Tables)
                PredefineSelection(t);
        }

        private void Ok_Executed()
        {
            foreach (var t in Tables)
                t.PropertyChanged -= TableViewModel_PropertyChanged;

            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        /// <summary>
        /// Currently at least a single table must be selected.
        /// </summary>
        private bool Ok_CanExecute() => Tables.Any(m => m.IsSelected && !m.IsProcedure);

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
                t.IsSelected = t.HasPrimaryKey && selectionMode.Value;

            SearchText = string.Empty;
        }

        private void UpdateTableSelectionThreeState()
        {
            TableSelectionThreeState = Tables.Where(m => m.HasPrimaryKey)
                                             .All(m => m.IsSelected)
                                           ? true
                                           : Tables.All(m => !m.IsSelected)
                                               ? (bool?)false
                                               : null;
        }

        private async Task HandleSearchTextChangeAsync(string text)
        {
            await Task.Delay(500); // Add a delay (like a debounce) so that not every character change triggers a search
            if (text != SearchText)
                return;

            FilteredTables.Refresh();
        }

        private bool FilterTable(ITableInformationViewModel tableInformationViewModel)
        {
            return string.IsNullOrWhiteSpace(SearchText)
                || tableInformationViewModel.Name.ToUpper().Contains(SearchText.ToUpper());
        }

        private static void PredefineSelection(ITableInformationViewModel t)
        {
            var unSelect = t.Name.StartsWith("[__")
                        || t.Name.StartsWith("[dbo].[__")
                        || t.Name.EndsWith(".[sysdiagrams]");
            if (unSelect) t.IsSelected = false;
        }

        void IPickTablesViewModel.AddTables(IEnumerable<TableModel> tables)
        {
            if (tables == null) return;

            foreach (var table in tables)
            {
                var tvm = _tableInformationViewModelFactory();
                tvm.HasPrimaryKey = table.HasPrimaryKey;
                tvm.Name = table.Name;
                tvm.ObjectType = table.ObjectType;
                if (table.ObjectType == ObjectType.Table)
                {
                    foreach (var column in table.Columns)
                    {
                        var cvm = _columnInformationViewModelFactory();
                        cvm.Name = column;
                        tvm.Columns.Add(cvm);
                    }
                }
                PredefineSelection(tvm);
                Tables.Add(tvm);
                tvm.PropertyChanged += TableViewModel_PropertyChanged;
            }

            FilteredTables.Refresh();
        }

        void IPickTablesViewModel.SelectTables(IEnumerable<SerializationTableModel> tables)
        {
            if (tables == null) return;

            foreach (var table in Tables)
            {
                var t = tables.SingleOrDefault(m => m.Name == table.Name);
                table.IsSelected = t != null;
                if (table.ObjectType == ObjectType.Table && table.IsSelected)
                {
                    foreach (var column in table.Columns)
                    {
                        column.IsSelected = !t?.ExcludedColumns?.Any(m => m == column.Name) ?? true;
                    }
                }
            }
        }

        SerializationTableModel[] IPickTablesViewModel.GetResult()
        {
            return Tables.Where(m => m.IsSelected)
                         .Select(m => new SerializationTableModel(m.Name, m.ObjectType, m.Columns.Where(c => !c.IsSelected).Select(c => c.Name)))
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