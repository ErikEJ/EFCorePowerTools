namespace EFCorePowerTools.Contracts.ViewModels
{
    using EFCorePowerTools.Shared.Models;
    using RevEng.Shared;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IObjectTreeViewModel : IViewModel
    {
        event EventHandler ObjectSelectionChanged;

        ObservableCollection<IObjectTreeRootItemViewModel> Types { get; }

        void Search(string searchText);
        void SetSelectionState(bool value);
        bool? GetSelectionState();
        IEnumerable<SerializationTableModel> GetSelectedObjects();
        void AddObjects(IEnumerable<TableModel> tables);
        void SelectObjects(IEnumerable<SerializationTableModel> tables);
    }
}