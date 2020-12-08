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
    using ReverseEngineer20.ReverseEngineer;
    using Shared.DAL;
    using Shared.Models;

    public class PickTablesViewModel : ViewModelBase, IPickTablesViewModel
    {

        private bool? _tableSelectionThreeState;
        private string _searchText;

        public PickTablesViewModel(IObjectTreeViewModel objectTreeViewModel)
        {
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);

            ObjectTree = objectTreeViewModel;
            ObjectTree.ObjectSelectionChanged += (s, e) => UpdateTableSelectionThreeState();

            SearchText = string.Empty;
        }

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public IObjectTreeViewModel ObjectTree { get; }

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

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        /// <summary>
        /// Currently at least a single table must be selected.
        /// </summary>
        private bool Ok_CanExecute() => ObjectTree.GetSelectedObjects().Any(c => c.ObjectType.HasColumns());

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        private void HandleTableSelectionThreeStateChange(bool? selectionMode)
        {
            if (selectionMode == null)
                return;

            ObjectTree.SetSelectionState(selectionMode.Value);
            SearchText = string.Empty;
        }

        private async Task HandleSearchTextChangeAsync(string text)
        {
            await Task.Delay(500); // Add a delay (like a debounce) so that not every character change triggers a search
            if (text != SearchText)
                return;

            ObjectTree.Search(SearchText);
        }

        private void UpdateTableSelectionThreeState()
        {
            TableSelectionThreeState = ObjectTree.GetSelectionState(); 
        }

        public void AddObjects(IEnumerable<TableModel> objects, IEnumerable<Schema> customReplacers)
        {
            if (objects == null) return;

            ObjectTree.AddObjects(objects, customReplacers);
        }

        public void SelectObjects(IEnumerable<SerializationTableModel> objects)
        {
            if (objects == null) return;
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
    }
}