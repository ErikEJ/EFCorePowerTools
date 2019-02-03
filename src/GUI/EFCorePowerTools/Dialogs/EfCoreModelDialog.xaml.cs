namespace EFCorePowerTools.Dialogs
{
    using System;
    using System.Windows;
    using Contracts.ViewModels;
    using Contracts.Views;
    using Shared.DAL;
    using Shared.Models;

    public partial class EfCoreModelDialog : IModelingOptionsDialog
    {
        private readonly Func<ModelingOptionsModel> _getDialogResult;
        private readonly Action<ModelingOptionsModel> _applyPresets;

        public EfCoreModelDialog(ITelemetryAccess telemetryAccess,
                                 IModelingOptionsViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(EfCoreModelDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _getDialogResult = () => viewModel.Model;
            _applyPresets = viewModel.ApplyPresets;

            Loaded += Window_Loaded;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FirstTextBox.Focus();
        }

        (bool ClosedByOK, ModelingOptionsModel Payload) IDialog<ModelingOptionsModel>.ShowAndAwaitUserResponse(bool modal)
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

        IModelingOptionsDialog IModelingOptionsDialog.ApplyPresets(ModelingOptionsModel presets)
        {
            _applyPresets(presets);
            return this;
        }
    }
}
