using System;
using System.Collections.Generic;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Handlers.Compare;

namespace EFCorePowerTools.Dialogs
{
    public partial class CompareResultDialog : ICompareResultDialog
    {
        private readonly Action<IEnumerable<CompareLogModel>> addComparisonResult;

        public CompareResultDialog(
            ITelemetryAccess telemetryAccess,
            ICompareResultViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(CompareOptionsDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            addComparisonResult = viewModel.AddComparisonResult;
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
            addComparisonResult(result);
        }
    }
}
