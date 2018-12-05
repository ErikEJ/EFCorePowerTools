namespace EFCorePowerTools.Contracts.ViewModels
{
    using System;
    using System.Collections.Generic;
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

        bool? TableSelectionThreeState { get; set; }
        string SearchText { get; set; }

        /// <summary>
        /// Adds the <paramref name="tables"/> to the <see cref="Tables"/>, wrapping it in a new <see cref="ITableInformationViewModel"/> instance.
        /// </summary>
        /// <param name="tables">The tables to add.</param>
        void AddTables(IEnumerable<TableInformationModel> tables);

        /// <summary>
        /// Selects the <paramref name="tables"/> from the <see cref="Tables"/> property, setting the <see cref="ITableInformationViewModel.IsSelected"/> to true, if both collections contain the table.
        /// </summary>
        /// <param name="tables">The tables to select.</param>
        void SelectTables(IEnumerable<TableInformationModel> tables);

        TableInformationModel[] GetResult();
    }
}