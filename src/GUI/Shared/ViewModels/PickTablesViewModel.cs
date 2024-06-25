using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RevEng.Common;

namespace EFCorePowerTools.ViewModels
{
    public class PickTablesViewModel : ViewModelBase, IPickTablesViewModel
    {
        private bool? tableSelectionThreeState;
        private string searchText;
        private SearchMode searchMode = SearchMode.Text;

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

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        /// <summary>
        /// At least a single table, function or stored procedure must be selected.
        /// </summary>
        private bool Ok_CanExecute()
            => ObjectTree.GetSelectedObjects().Any(c => c.ObjectType.HasColumns())
              || ObjectTree.GetSelectedObjects().Any(c => c.ObjectType == ObjectType.Procedure)
              || ObjectTree.GetSelectedObjects().Any(c => c.ObjectType == ObjectType.ScalarFunction);

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
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
    }
}
