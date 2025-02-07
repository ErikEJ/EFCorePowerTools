using System;
using System.Collections.Generic;
using System.Windows;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;

namespace EFCorePowerTools.Dialogs
{
    public partial class PickConfigDialog : IPickConfigDialog
    {
        private readonly Func<ConfigModel> getDialogResult;
        private readonly Action<IEnumerable<ConfigModel>> addConfigurations;

        public PickConfigDialog(
            ITelemetryAccess telemetryAccess,
            IPickConfigViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(PickConfigDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            getDialogResult = () => viewModel.SelectedConfiguration;

            addConfigurations = models =>
            {
                foreach (var model in models)
                {
                    viewModel.Configurations.Add(model);
                }
            };

            InitializeComponent();
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

            return (closedByOkay, getDialogResult());
        }

        public void PublishConfigurations(IEnumerable<ConfigModel> configurations)
        {
            addConfigurations(configurations);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionCombobox.Focus();
        }
    }
}