namespace EFCorePowerTools.Contracts.ViewModels
{
    using RevEng.Shared;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IObjectTreeViewModel : IViewModel
    {
        event EventHandler ObjectSelectionChanged;

        ObservableCollection<IObjectTreeRootItemViewModel> Types { get; }

        bool IsInEditMode { get; }
        void Search(string searchText, SearchMode searchMode);
        void SetSelectionState(bool value);
        bool? GetSelectionState();
        IEnumerable<SerializationTableModel> GetSelectedObjects();
        IEnumerable<Schema> GetRenamedObjects();
        void AddObjects(IEnumerable<TableModel> objects, IEnumerable<Schema> customReplacers);
        void SelectObjects(IEnumerable<SerializationTableModel> objects);
    }
}