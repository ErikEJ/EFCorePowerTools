namespace EFCorePowerTools.Dialogs
{
    using Contracts.ViewModels;
    using Contracts.Views;
    using Shared.DAL;
    using Shared.Models;
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public partial class PickConfigDialog : IPickConfigDialog
    {
        private readonly Func<ConfigModel> _getDialogResult;
        private readonly Action<IEnumerable<ConfigModel>> _addConfigurations;

        public PickConfigDialog(ITelemetryAccess telemetryAccess,
                                        IPickConfigViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(PickConfigDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _getDialogResult = () => viewModel.SelectedConfiguration;

            _addConfigurations = models =>
            {
                foreach (var model in models)
                    viewModel.Configurations.Add(model);
            };


            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionCombobox.Focus();
        }

        (bool ClosedByOK, ConfigModel Payload) IDialog<ConfigModel>.ShowAndAwaitUserResponse(bool modal)
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

        public void PublishConfigurations(IEnumerable<ConfigModel> configurations)
        {
            _addConfigurations(configurations);
        }
    }
}