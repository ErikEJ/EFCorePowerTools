using System;
using System.Windows.Controls;
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

            ToggleOptions();
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

        private void chkSmartMapping_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                if (checkBox.IsChecked ?? true)
                {
                    AdvancedOptions.Instance.MapUsedTypes = true;
                    ToggleOptions();
                }
                else
                {
                    AdvancedOptions.Instance.MapUsedTypes = false;
                    ToggleOptions();
                }
            }
        }

        private void ToggleOptions()
        {
            var toggle = AdvancedOptions.Instance.MapUsedTypes;

            if (chkSpatial != null)
            {
                chkSpatial.IsEnabled = !toggle;
                chkDateAndTime.IsEnabled = !toggle;
                chkHierarchy.IsEnabled = !toggle;
            }
        }
    }
}
