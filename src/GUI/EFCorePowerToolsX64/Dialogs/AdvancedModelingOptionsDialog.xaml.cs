using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Shared.DAL;
using EFCorePowerTools.Shared.Models;
using System;

namespace EFCorePowerTools.Dialogs
{
    public partial class AdvancedModelingOptionsDialog : IAdvancedModelingOptionsDialog
    {
        private readonly Func<ModelingOptionsModel> _getDialogResult;
        private readonly Action<ModelingOptionsModel> _applyPresets;

        public AdvancedModelingOptionsDialog(ITelemetryAccess telemetryAccess,
            IAdvancedModelingOptionsViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(AdvancedModelingOptionsDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _getDialogResult = () => viewModel.Model;
            _applyPresets = viewModel.ApplyPresets;

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

            return (closedByOkay, _getDialogResult());
        }

        IAdvancedModelingOptionsDialog IAdvancedModelingOptionsDialog.ApplyPresets(ModelingOptionsModel presets)
        {
            _applyPresets(presets);
            return this;
        }
    }
}