namespace EFCorePowerTools.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Contracts.ViewModels;
    using Contracts.Views;
    using Shared.DAL;
    using Shared.Models;

    public partial class PickTablesDialog : IPickTablesDialog
    {
        private readonly Func<SerializationTableModel[]> _getDialogResult;
        private readonly Action<IEnumerable<TableModel>> _addTables;
        private readonly Action<IEnumerable<SerializationTableModel>> _selectTables;

        public PickTablesDialog(ITelemetryAccess telemetryAccess,
                                IPickTablesViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(PickTablesDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _getDialogResult = viewModel.GetSelectedObjects;
            _addTables = viewModel.AddObjects;
            _selectTables = viewModel.SelectObjects;

            InitializeComponent();
        }

        (bool ClosedByOK, SerializationTableModel[] Payload) IDialog<SerializationTableModel[]>.ShowAndAwaitUserResponse(bool modal)
        {
            bool closedByOkay;

            if (modal)
            {
                closedByOkay = ShowModal() == true;
            }
            else
            {
                closedByOkay = ShowDialog() == true;
            }

            return (closedByOkay, _getDialogResult());
        }

        IPickTablesDialog IPickTablesDialog.AddTables(IEnumerable<TableModel> tables)
        {
            _addTables(tables);
            return this;
        }

        IPickTablesDialog IPickTablesDialog.PreselectTables(IEnumerable<SerializationTableModel> tables)
        {
            _selectTables(tables);
            return this;
        }

        private void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            
            if (checkBox.IsChecked == false)
            {
                statusBar.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
