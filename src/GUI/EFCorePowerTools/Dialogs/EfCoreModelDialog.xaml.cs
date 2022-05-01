namespace EFCorePowerTools.Dialogs
{
    using Contracts.ViewModels;
    using Contracts.Views;
    using Common.DAL;
    using Common.Models;
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Documents;

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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(hyperlink.NavigateUri.AbsoluteUri),
            };
            process.Start();
        }
    }
}
