namespace EFCorePowerTools.Contracts.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using EventArgs;
    using Shared.Models;

    public interface IPickTablesViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand LoadedCommand { get; }
        ICommand SaveSelectionCommand { get; }
        ICommand LoadSelectionCommand { get; }
        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        ObservableCollection<ITableInformationViewModel> Tables { get; }
        ICollectionView FilteredTables { get; }

        bool IncludeTables { get; set; }
        bool? TableSelectionThreeState { get; set; }
        string SearchText { get; set; }

        /// <summary>
        /// Adds the <paramref name="table"/> to the <see cref="Tables"/>, wrapping it in a new <see cref="ITableInformationViewModel"/> instance.
        /// </summary>
        /// <param name="table">The table to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is <b>null</b>.</exception>
        void AddTable(TableInformationModel table);

        TableInformationModel[] GetResult();
    }
}