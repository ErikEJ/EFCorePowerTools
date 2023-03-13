using System;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;

namespace EFCorePowerTools.Dialogs
{
    public partial class AdvancedModelingOptionsDialog : IAdvancedModelingOptionsDialog
    {
        private readonly Func<ModelingOptionsModel> getDialogResult;
        private readonly Action<ModelingOptionsModel> applyPresets;

        public AdvancedModelingOptionsDialog(
            ITelemetryAccess telemetryAccess,
            IAdvancedModelingOptionsViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(AdvancedModelingOptionsDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            getDialogResult = () => viewModel.Model;
            applyPresets = viewModel.ApplyPresets;

            InitializeComponent();
        }

        public (bool ClosedByOK, ModelingOptionsModel Payload) ShowAndAwaitUserResponse(bool modal)
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

        IAdvancedModelingOptionsDialog IAdvancedModelingOptionsDialog.ApplyPresets(ModelingOptionsModel presets)
        {
            applyPresets(presets);
            return this;
        }
    }
}
