using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Locales;
using GalaSoft.MvvmLight;
using RevEng.Common;

namespace EFCorePowerTools.ViewModels
{
    public class ObjectTreeViewModel : ViewModelBase, IObjectTreeViewModel
    {
        private readonly Func<ISchemaInformationViewModel> schemaInformationViewModelFactory;
        private readonly Func<ITableInformationViewModel> tableInformationViewModelFactory;
        private readonly Func<IColumnInformationViewModel> columnInformationViewModelFactory;
        private IEnumerable<Schema> allSchemas = new List<Schema>();

        public ObjectTreeViewModel(
            Func<ISchemaInformationViewModel> schemaInformationViewModelFactory,
            Func<ITableInformationViewModel> tableInformationViewModelFactory,
            Func<IColumnInformationViewModel> columnInformationViewModelFactory)
        {
            this.schemaInformationViewModelFactory = schemaInformationViewModelFactory ?? throw new ArgumentNullException(nameof(schemaInformationViewModelFactory));
            this.tableInformationViewModelFactory = tableInformationViewModelFactory ?? throw new ArgumentNullException(nameof(tableInformationViewModelFactory));
            this.columnInformationViewModelFactory = columnInformationViewModelFactory ?? throw new ArgumentNullException(nameof(columnInformationViewModelFactory));
        }

        public event EventHandler ObjectSelectionChanged;

        public bool IsInEditMode { get => Objects.Any(o => o.IsEditing) || Objects.SelectMany(c => c.Columns).Any(o => o.IsEditing); }

        public ObservableCollection<IObjectTreeRootItemViewModel> Types { get; } = new ObservableCollection<IObjectTreeRootItemViewModel>();

        private IEnumerable<ITableInformationViewModel> Objects => Types.SelectMany(c => c.Schemas).SelectMany(c => c.Objects);

        public bool? GetSelectionState()
        {
            return Objects.All(m => m.IsSelected.Value)
                ? true
                : Objects.All(m => !m.IsSelected.Value)
                    ? (bool?)false
                    : null;
        }

        public void Search(string searchText, SearchMode searchMode)
        {
            var regex = new Regex(searchText, RegexOptions.None, new TimeSpan(0, 0, 3));

            foreach (var obj in Objects)
            {
                if (searchMode == SearchMode.Text)
                {
                    obj.IsVisible = string.IsNullOrWhiteSpace(searchText) || obj.DisplayName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                else
                {
                    try
                    {
                        obj.IsVisible = string.IsNullOrWhiteSpace(searchText) || regex.IsMatch(obj.DisplayName);
                    }
                    catch (RegexMatchTimeoutException)
                    {
                        obj.IsVisible = true;
                    }
                }
            }
        }

        public void SetSelectionState(bool value)
        {
            foreach (var t in Objects)
            {
                t.SetSelectedCommand.Execute(value);
            }
        }

        public IEnumerable<SerializationTableModel> GetSelectedObjects()
        {
            return Objects
                .Where(c => c.IsSelected.Value)
                .Select(m => new SerializationTableModel(m.ModelDisplayName, m.ObjectType, m.Columns.Where(c => !c.IsSelected.Value).Select(c => c.Name).ToList(), null));
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

                    var originalReplacers = allSchemas.Where(s => s.SchemaName == schema.Name)
                        .SelectMany(a => a.Tables.Where(t => t.Columns != null && t.Name == obj.Name))
                        .ToList();

                    var ignoredColumnReplacers = originalReplacers
                        .SelectMany(o => o.Columns.Where(c => c.Name != null && c.Name.Equals(c.NewName)))
                        .ToList();

                    var ignoredTableReplacers = originalReplacers
                        .Where(c => c.Name != null && c.Name.Equals(c.NewName) && (!c.Columns?.Any() ?? false))
                        .ToList();

                    if (objectIsRenamed
                        || renamedColumns.Any()
                        || ignoredColumnReplacers.Any()
                        || ignoredTableReplacers.Any())
                    {
                        var columnRenamers = renamedColumns
                            .Select(c => new ColumnNamer { Name = c.Name, NewName = c.NewName })
                            .Concat(ignoredColumnReplacers);

                        if (replacingSchema.Tables == null)
                        {
                            replacingSchema.Tables = new List<TableRenamer>();
                        }

                        replacingSchema.Tables.Add(new TableRenamer
                        {
                            Name = obj.Name,
                            NewName = obj.NewName,
                            Columns = columnRenamers.ToList(),
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

            if (!allSchemas.Any())
            {
                return result;
            }

            foreach (var schema in allSchemas)
            {
                var existingSchema = result.Find(s => s.SchemaName == schema.SchemaName);
                if (existingSchema == null)
                {
                    if (schema.Tables?.Count == 0)
                    {
                        schema.Tables = null;
                    }

                    result.Add(schema);
                }
            }

            return result;
        }

        public void AddObjects(IEnumerable<TableModel> objects, IEnumerable<Schema> customReplacers)
        {
            if (objects == null)
            {
                throw new ArgumentNullException(nameof(objects));
            }

            var objectTypes = new List<(ObjectType ObjectType, string Text)>
            {
                (ObjectType.Table, ReverseEngineerLocale.Tables),
                (ObjectType.View, ReverseEngineerLocale.Views),
                (ObjectType.Procedure, ReverseEngineerLocale.StoredProcedures),
                (ObjectType.ScalarFunction, ReverseEngineerLocale.Functions),
            };

            if (customReplacers != null)
            {
                allSchemas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Schema>>(Newtonsoft.Json.JsonConvert.SerializeObject(customReplacers));
            }
            else
            {
                allSchemas = new List<Schema>();
            }

            foreach (var objectType in objectTypes)
            {
                var objectsBySchema = objects.Where(c => c.ObjectType == objectType.ObjectType).GroupBy(c => c.Schema);

                if (!objectsBySchema.Any())
                {
                    continue;
                }

                var type = new ObjectTreeRootItemViewModel() { Text = objectType.Text, ObjectType = objectType.ObjectType };
                Types.Add(type);

                foreach (var schema in objectsBySchema)
                {
                    var schemaReplacer = customReplacers?.FirstOrDefault(c => c.SchemaName == schema.Key);
                    var objectReplacers = schemaReplacer?.Tables;

                    var svm = schemaInformationViewModelFactory();
                    svm.Name = schema.Key;
                    svm.ReplacingSchema = schemaReplacer ?? new Schema();
                    type.Schemas.Add(svm);

                    foreach (var obj in schema)
                    {
                        var objectReplacer = objectReplacers?.Find(c => c.Name != null && c.Name.Equals(obj.Name, StringComparison.OrdinalIgnoreCase));
                        var tvm = tableInformationViewModelFactory();
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
                                var cvm = columnInformationViewModelFactory();
                                cvm.Name = column.Name;
                                cvm.NewName = columnReplacers?.Find(c => c.Name != null && c.Name.Equals(column.Name, StringComparison.OrdinalIgnoreCase))?.NewName ?? column.Name;
                                cvm.IsPrimaryKey = column.IsPrimaryKey;
                                cvm.IsForeignKey = column.IsForeignKey;
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

        public void SelectObjects(IEnumerable<SerializationTableModel> objects)
        {
            if (objects == null)
            {
                throw new ArgumentNullException(nameof(objects));
            }

            foreach (var obj in Objects)
            {
                var t = objects.FirstOrDefault(m => m.Name == obj.ModelDisplayName);
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

        private static void PredefineSelection(ITableInformationViewModel t)
        {
            var unSelect = t.Name.StartsWith("[__")
                        || t.Name.StartsWith("[dbo].[__")
                        || t.Name.EndsWith(".[sysdiagrams]");
            if (unSelect)
            {
                t.SetSelectedCommand.Execute(false);
            }
        }

        private void TableViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ITableInformationViewModel.IsSelected))
            {
                var handler = ObjectSelectionChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
