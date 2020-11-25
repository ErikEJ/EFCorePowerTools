namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using EFCorePowerTools.Shared.Models;
    using GalaSoft.MvvmLight;
    using RevEng.Shared;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    public class ObjectTreeViewModel : ViewModelBase, IObjectTreeViewModel
    {
        public event EventHandler ObjectSelectionChanged;

        private IEnumerable<ITableInformationViewModel> _objects => Types.SelectMany(c => c.Objects);
        private readonly Func<ITableInformationViewModel> _tableInformationViewModelFactory;
        private readonly Func<IColumnInformationViewModel> _columnInformationViewModelFactory;

        public ObjectTreeViewModel(Func<ITableInformationViewModel> tableInformationViewModelFactory,
                                   Func<IColumnInformationViewModel> columnInformationViewModelFactory)
        {
            _tableInformationViewModelFactory = tableInformationViewModelFactory ?? throw new ArgumentNullException(nameof(tableInformationViewModelFactory));
            _columnInformationViewModelFactory = columnInformationViewModelFactory ?? throw new ArgumentNullException(nameof(columnInformationViewModelFactory));

            Types.Add(new ObjectTreeRootItemViewModel { Text = "Tables" });
            Types.Add(new ObjectTreeRootItemViewModel { Text = "Views" });
            Types.Add(new ObjectTreeRootItemViewModel { Text = "Stored procedures" });
            Types.Add(new ObjectTreeRootItemViewModel { Text = "Functions" });
        }

        public ObservableCollection<IObjectTreeRootItemViewModel> Types { get; } = new ObservableCollection<IObjectTreeRootItemViewModel>();

        public bool? GetSelectionState()
        {
            return _objects.All(m => m.IsSelected)
                ? true
                : _objects.All(m => !m.IsSelected)
                    ? (bool?)false
                    : null;
        }

        public void Search(string searchText)
        {
            foreach (var type in Types)
            {
                foreach (var obj in type.Objects)
                {
                    obj.IsVisible = string.IsNullOrWhiteSpace(searchText) || obj.Name.ToUpper().Contains(searchText.ToUpper());
                }
            }
        }

        public void SetSelectionState(bool value)
        {
            foreach (var t in _objects)
                t.IsSelected = value;
        }

        public IEnumerable<ITableInformationViewModel> GetSelected()
        {
            return _objects.Where(c => c.IsSelected);
        }

        public void AddObjects(IEnumerable<TableModel> tables)
        {
            foreach (var table in tables)
            {
                var tvm = _tableInformationViewModelFactory();
                tvm.HasPrimaryKey = table.HasPrimaryKey;
                tvm.Name = table.Name;
                tvm.ObjectType = table.ObjectType;
                if (table.ObjectType.HasColumns())
                {
                    foreach (var column in table.Columns)
                    {
                        var cvm = _columnInformationViewModelFactory();
                        cvm.Name = column.Name;
                        cvm.IsPrimaryKey = column.IsPrimaryKey;
                        tvm.Columns.Add(cvm);
                    }
                }
                PredefineSelection(tvm);

                if (tvm.ObjectType == ObjectType.Table)
                    Types[0].Objects.Add(tvm);
                else if (tvm.ObjectType == ObjectType.View)
                    Types[1].Objects.Add(tvm);
                else if (tvm.ObjectType == ObjectType.Procedure)
                    Types[2].Objects.Add(tvm);
                else
                    Types[3].Objects.Add(tvm);

                tvm.PropertyChanged += TableViewModel_PropertyChanged;
            }
        }

        private static void PredefineSelection(ITableInformationViewModel t)
        {
            var unSelect = t.Name.StartsWith("[__")
                        || t.Name.StartsWith("[dbo].[__")
                        || t.Name.EndsWith(".[sysdiagrams]");
            if (unSelect) t.IsSelected = false;
        }

        private void TableViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ITableInformationViewModel.IsSelected))
            {
                var handler = ObjectSelectionChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SelectObject(IEnumerable<SerializationTableModel> tables)
        {
            foreach (var obj in _objects)
            {
                var t = tables.SingleOrDefault(m => m.Name == obj.Name);
                obj.IsSelected = t != null;
                if (obj.ObjectType.HasColumns() && obj.IsSelected)
                {
                    foreach (var column in obj.Columns)
                    {
                        column.IsSelected = !t?.ExcludedColumns?.Any(m => m == column.Name) ?? true;
                    }
                }
            }
            foreach (var type in Types)
            {
                type.IsSelected = type.Objects.All(m => m.IsSelected)
                    ? true
                    : _objects.All(m => !m.IsSelected)
                        ? (bool?)false
                        : null;
            }
        }
    }
}