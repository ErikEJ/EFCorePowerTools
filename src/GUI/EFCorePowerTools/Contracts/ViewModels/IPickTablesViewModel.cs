namespace EFCorePowerTools.Contracts.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using EventArgs;
    using ReverseEngineer20.ReverseEngineer;
    using Shared.Models;

    public interface IPickTablesViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        bool? TableSelectionThreeState { get; set; }
        string SearchText { get; set; }
        IObjectTreeViewModel ObjectTree { get; }

        /// <summary>
        /// Adds the <paramref name="objects"/> to the <see cref="Tables"/>, wrapping it in a new <see cref="ITableInformationViewModel"/> instance.
        /// </summary>
        /// <param name="objects">The tables to add.</param>
        void AddObjects(IEnumerable<TableModel> objects, IEnumerable<Schema> customReplacers);

        /// <summary>
        /// Selects the <paramref name="objects"/> from the <see cref="Tables"/> property, setting the <see cref="ITableInformationViewModel.IsSelected"/> to true, if both collections contain the table.
        /// </summary>
        /// <param name="objects">The tables to select.</param>
        void SelectObjects(IEnumerable<SerializationTableModel> objects);

        SerializationTableModel[] GetSelectedObjects();

        Schema[] GetRenamedObjects();
    }
}