using System;
using System.Collections.Generic;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;

namespace EFCorePowerTools.Dialogs
{
    public partial class CompareOptionsDialog : ICompareOptionsDialog
    {
        private readonly Func<(DatabaseConnectionModel Connection, IEnumerable<string> ContextTypes)> getDialogResult;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> addConnections;
        private readonly Action<IEnumerable<string>> contextTypes;

        public CompareOptionsDialog(
            ITelemetryAccess telemetryAccess,
            ICompareOptionsViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(CompareOptionsDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            getDialogResult = viewModel.GetSelection;
            addConnections = viewModel.AddDatabaseConnections;
            contextTypes = viewModel.AddContextTypes;
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

            return (closedByOkay, getDialogResult());
        }

        public void AddConnections(IEnumerable<DatabaseConnectionModel> connections)
        {
            addConnections(connections);
        }

        public void AddContextTypes(IEnumerable<string> contextTypes)
        {
            this.contextTypes(contextTypes);
        }
    }
}
