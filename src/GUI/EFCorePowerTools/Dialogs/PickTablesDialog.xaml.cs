namespace EFCorePowerTools.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts.ViewModels;
    using Contracts.Views;
    using Shared.DAL;
    using Shared.Models;

    public partial class PickTablesDialog : IPickTablesDialog
    {
        private readonly Func<TableInformationModel[]> _getDialogResult;
        private readonly Action _includeTables;
        private readonly Action<TableInformationModel> _addTable;
        private readonly Action<TableInformationModel> _selectTable;

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
            _includeTables = () => viewModel.IncludeTables = true;
            _addTable = viewModel.AddTable;
            _selectTable = table =>
            {
                var t = viewModel.Tables.SingleOrDefault(m => m.Model.SafeFullName == table.SafeFullName);
                if (t != null)
                    t.IsSelected = true;
            };

            InitializeComponent();
        }

        (bool ClosedByOK, TableInformationModel[] Payload) IDialog<TableInformationModel[]>.ShowAndAwaitUserResponse(bool modal)
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

        IPickTablesDialog IPickTablesDialog.IncludeTables()
        {
            _includeTables();
            return this;
        }

        IPickTablesDialog IPickTablesDialog.AddTables(IEnumerable<TableInformationModel> tables)
        {
            if (tables == null) return this;
            foreach (var table in tables)
                _addTable(table);
            return this;
        }

        IPickTablesDialog IPickTablesDialog.PreselectTables(IEnumerable<TableInformationModel> tables)
        {
            if (tables == null) return this;
            foreach (var table in tables)
                _selectTable(table);
            return this;
        }
    }
}
