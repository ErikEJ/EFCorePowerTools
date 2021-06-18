namespace EFCorePowerTools.Dialogs
{
    using Contracts.ViewModels;
    using Contracts.Views;
    using EFCorePowerTools.Handlers.Compare;
    using Shared.DAL;
    using System;
    using System.Collections.Generic;

    public partial class CompareResultDialog : ICompareResultDialog
    {
        private readonly Action<IEnumerable<CompareLogModel>> _addComparisonResult;

        public CompareResultDialog(ITelemetryAccess telemetryAccess,
                                    ICompareResultViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(CompareOptionsDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _addComparisonResult = viewModel.AddComparisonResult;
            InitializeComponent();
        }

        public (bool ClosedByOK, object Payload) ShowAndAwaitUserResponse(bool modal)
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

            return (closedByOkay, null);
        }

        public void AddComparisonResult(IEnumerable<CompareLogModel> result)
        {
            _addComparisonResult(result);
        }
    }
}