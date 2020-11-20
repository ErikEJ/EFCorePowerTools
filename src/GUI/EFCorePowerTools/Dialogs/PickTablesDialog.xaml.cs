namespace EFCorePowerTools.Dialogs
{
    using System;
    using System.Collections.Generic;
    using Contracts.ViewModels;
    using Contracts.Views;
    using Shared.DAL;
    using Shared.Models;

    public partial class PickTablesDialog : IPickTablesDialog
    {
        private readonly Func<TableModel[]> _getDialogResult;
        private readonly Action<IEnumerable<TableModel>> _addTables;
        private readonly Action<IEnumerable<TableModel>> _selectTables;

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
            _getDialogResult = viewModel.GetResult;
            _addTables = viewModel.AddTables;
            _selectTables = viewModel.SelectTables;

            InitializeComponent();
        }

        (bool ClosedByOK, TableModel[] Payload) IDialog<TableModel[]>.ShowAndAwaitUserResponse(bool modal)
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

        IPickTablesDialog IPickTablesDialog.PreselectTables(IEnumerable<TableModel> tables)
        {
            _selectTables(tables);
            return this;
        }
    }
}
