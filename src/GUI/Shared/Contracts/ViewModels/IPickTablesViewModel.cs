using System;
using System.Collections.Generic;
using System.Windows.Input;
using EFCorePowerTools.Contracts.EventArgs;
using RevEng.Common;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IPickTablesViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        bool? TableSelectionThreeState { get; set; }
        string SearchText { get; set; }
        SearchMode SearchMode { get; set; }
        IObjectTreeViewModel ObjectTree { get; }

        void AddObjects(IEnumerable<TableModel> objects, IEnumerable<Schema> customReplacers);

        void SelectObjects(IEnumerable<SerializationTableModel> objects);

        SerializationTableModel[] GetSelectedObjects();

        Schema[] GetRenamedObjects();
    }
}
