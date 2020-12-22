namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using EFCorePowerTools.Shared.Models;
    using GalaSoft.MvvmLight;
    using RevEng.Shared;
    using ReverseEngineer20.ReverseEngineer;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    public class ObjectTreeViewModel : ViewModelBase, IObjectTreeViewModel
    {
        public event EventHandler ObjectSelectionChanged;

        private IEnumerable<ITableInformationViewModel> _objects => Types.SelectMany(c => c.Schemas).SelectMany(c => c.Objects);
        private IEnumerable<Schema> _allSchemas = new List<Schema>();
        private readonly Func<ISchemaInformationViewModel> _schemaInformationViewModelFactory;
        private readonly Func<ITableInformationViewModel> _tableInformationViewModelFactory;
        private readonly Func<IColumnInformationViewModel> _columnInformationViewModelFactory;

        public ObjectTreeViewModel(Func<ISchemaInformationViewModel> schemaInformationViewModelFactory,
                                   Func<ITableInformationViewModel> tableInformationViewModelFactory,
                                   Func<IColumnInformationViewModel> columnInformationViewModelFactory)
        {
            _schemaInformationViewModelFactory = schemaInformationViewModelFactory ?? throw new ArgumentNullException(nameof(schemaInformationViewModelFactory));
            _tableInformationViewModelFactory = tableInformationViewModelFactory ?? throw new ArgumentNullException(nameof(tableInformationViewModelFactory));
            _columnInformationViewModelFactory = columnInformationViewModelFactory ?? throw new ArgumentNullException(nameof(columnInformationViewModelFactory));
        }

        public ObservableCollection<IObjectTreeRootItemViewModel> Types { get; } = new ObservableCollection<IObjectTreeRootItemViewModel>();

        public bool? GetSelectionState()
        {
            return _objects.All(m => m.IsSelected.Value)
                ? true
                : _objects.All(m => !m.IsSelected.Value)
                    ? (bool?)false
                    : null;
        }

        public bool IsInEditMode { get => _objects.Any(o => o.IsEditing) || _objects.SelectMany(c => c.Columns).Any(o => o.IsEditing); }

        public void Search(string searchText)
        {
            foreach (var obj in _objects)
            {
                obj.IsVisible = string.IsNullOrWhiteSpace(searchText) || obj.DisplayName.ToUpper().Contains(searchText.ToUpper());
            }
        }

        public void SetSelectionState(bool value)
        {
            foreach (var t in _objects)
                t.SetSelectedCommand.Execute(value);
        }

        public IEnumerable<SerializationTableModel> GetSelectedObjects()
        {
            return _objects
                .Where(c => c.IsSelected.Value)
                .Select(m => new SerializationTableModel(m.ModelDisplayName, m.ObjectType, m.Columns.Where(c => !c.IsSelected.Value).Select(c => c.Name).ToList()));
        }

        public IEnumerable<Schema> GetRenamedObjects()
        {
            var result = new List<Schema>();
            foreach (var schema in Types.SelectMany(t => t.Schemas).GroupBy(c => c.Name).Select(c => new { Name = c.Key, ReplacingSchema = c.First().ReplacingSchema, Objects = c.SelectMany(f => f.Objects.Where(o => o.IsSelected.Value)) }))
            {
                var replacingSchema = schema.ReplacingSchema ?? new Schema();
                replacingSchema.SchemaName = schema.Name;
                replacingSchema.Tables?.Clear();
                foreach (var obj in schema.Objects)
                {
                    var objectIsRenamed = !obj.Name.Equals(obj.NewName);
                    var renamedColumns = obj.Columns.Where(c => !c.Name.Equals(c.NewName) && c.IsSelected.Value);
                    if (objectIsRenamed || renamedColumns.Any())
                    {
                        if (replacingSchema.Tables == null)
                            replacingSchema.Tables = new List<TableRenamer>();

                        replacingSchema.Tables.Add(new TableRenamer
                        {
                            Name = obj.Name,
                            NewName = obj.NewName,
                            Columns = renamedColumns.Select(c => new ColumnNamer { Name = c.Name, NewName = c.NewName }).ToList()
                        });
                    }
                }

                if (replacingSchema.ColumnPatternReplaceWith != default ||
                    replacingSchema.ColumnRegexPattern != default ||
                    replacingSchema.TablePatternReplaceWith != default ||
                    replacingSchema.TableRegexPattern != default ||
                    (replacingSchema.Tables?.Any() ?? false) ||
                    replacingSchema.UseSchemaName)
                {
                    result.Add(replacingSchema);
                }
            }

            if (!_allSchemas.Any())
            {
                return result;
            }

            foreach (var schema in _allSchemas)
            {
                var existingSchema = result.FirstOrDefault(s => s.SchemaName == schema.SchemaName);
                if (existingSchema == null)
                {
                    result.Add(schema);
                }
            }

            return result;
        }

        public void AddObjects(IEnumerable<TableModel> objects, IEnumerable<Schema> customReplacers)
        {
            if (objects == null) throw new ArgumentNullException(nameof(objects));

            var objectTypes = new List<(ObjectType ObjectType, string Text)> {
                (ObjectType.Table, "Tables"),
                (ObjectType.View, "Views"),
                (ObjectType.Procedure, "Stored procedures"),
                (ObjectType.ScalarFunction, "Functions")
            };

            _allSchemas = customReplacers ?? new List<Schema>();

            foreach (var objectType in objectTypes)
            {
                var objectsBySchema = objects.Where(c => c.ObjectType == objectType.ObjectType).GroupBy(c => c.Schema);

                if (!objectsBySchema.Any())
                    continue;

                var type = new ObjectTreeRootItemViewModel() { Text = objectType.Text, ObjectType = objectType.ObjectType };
                Types.Add(type);

                foreach (var schema in objectsBySchema)
                {
                    var schemaReplacer = customReplacers?.FirstOrDefault(c => c.SchemaName == schema.Key);
                    var objectReplacers = schemaReplacer?.Tables;

                    var svm = _schemaInformationViewModelFactory();
                    svm.Name = schema.Key;
                    svm.ReplacingSchema = schemaReplacer ?? new Schema();
                    type.Schemas.Add(svm);

                    foreach (var obj in schema)
                    {
                        var objectReplacer = objectReplacers?.FirstOrDefault(c => c.Name.Equals(obj.Name, StringComparison.OrdinalIgnoreCase));
                        var tvm = _tableInformationViewModelFactory();
                        tvm.Name = obj.Name;
                        tvm.ModelDisplayName = obj.DisplayName;
                        tvm.NewName = objectReplacer?.NewName ?? obj.Name;
                        tvm.Schema = obj.Schema;
                        tvm.ObjectType = obj.ObjectType;

                        var columnReplacers = objectReplacer?.Columns;
                        if (obj.ObjectType.HasColumns())
                        {
                            foreach (var column in obj.Columns)
                            {
                                var cvm = _columnInformationViewModelFactory();
                                cvm.Name = column.Name;
                                cvm.NewName = columnReplacers?.FirstOrDefault(c => c.Name.Equals(column.Name, StringComparison.OrdinalIgnoreCase))?.NewName ?? column.Name;
                                cvm.IsPrimaryKey = column.IsPrimaryKey;
                                tvm.Columns.Add(cvm);
                            }
                        }
                        PredefineSelection(tvm);

                        svm.Objects.Add(tvm);
                        tvm.PropertyChanged += TableViewModel_PropertyChanged;
                    }
                }
            }
        }

        private static void PredefineSelection(ITableInformationViewModel t)
        {
            var unSelect = t.Name.StartsWith("[__")
                        || t.Name.StartsWith("[dbo].[__")
                        || t.Name.EndsWith(".[sysdiagrams]");
            if (unSelect) t.SetSelectedCommand.Execute(false);
        }

        private void TableViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ITableInformationViewModel.IsSelected))
            {
                var handler = ObjectSelectionChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SelectObjects(IEnumerable<SerializationTableModel> objects)
        {
            if (objects == null) throw new ArgumentNullException(nameof(objects));

            foreach (var obj in _objects)
            {
                var t = objects.SingleOrDefault(m => m.Name == obj.ModelDisplayName);
                obj.SetSelectedCommand.Execute(t != null);
                if (obj.ObjectType.HasColumns() && obj.IsSelected.Value)
                {
                    foreach (var column in obj.Columns)
                    {
                        column.SetSelected(!t?.ExcludedColumns?.Any(m => m == column.Name) ?? true);
                    }
                }
            }
        }
    }
}