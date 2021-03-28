namespace EFCorePowerTools.Dialogs
{
    using Contracts.ViewModels;
    using Contracts.Views;
    using EFCorePowerTools.Shared.Models;
    using Shared.DAL;
    using System;
    using System.Collections.Generic;

    public partial class CompareOptionsDialog : ICompareOptionsDialog
    {
        private readonly Func<(DatabaseConnectionModel Connection, IEnumerable<string> contextTypes)> _getDialogResult;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> _addConnections;
        private readonly Action<IEnumerable<string>> _contextTypes;

        public CompareOptionsDialog(ITelemetryAccess telemetryAccess,
                                    ICompareOptionsViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(CompareOptionsDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _getDialogResult = viewModel.GetSelection;
            _addConnections = viewModel.AddDatabaseConnections;
            _contextTypes = viewModel.AddContextTypes;
            InitializeComponent();
        }

        public (bool ClosedByOK, (DatabaseConnectionModel Connection, IEnumerable<string> ContextTypes) Payload) ShowAndAwaitUserResponse(bool modal)
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

        public void AddConnections(IEnumerable<DatabaseConnectionModel> connections)
        {
            _addConnections(connections);
        }

        public void AddContextTypes(IEnumerable<string> contextTypes)
        {
            _contextTypes(contextTypes);
        }
    }
}
