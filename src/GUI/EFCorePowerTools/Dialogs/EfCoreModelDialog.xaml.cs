using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;

namespace EFCorePowerTools.Dialogs
{
    public partial class EfCoreModelDialog : IModelingOptionsDialog
    {
        private readonly Func<ModelingOptionsModel> getDialogResult;
        private readonly Action<ModelingOptionsModel> applyPresets;
        private readonly Action<TemplateTypeItem, IList<TemplateTypeItem>> setTemplateTypes;

        public EfCoreModelDialog(
            ITelemetryAccess telemetryAccess,
            IModelingOptionsViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(EfCoreModelDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            getDialogResult = () => viewModel.Model;
            applyPresets = viewModel.ApplyPresets;
            setTemplateTypes = (templateType, templateTypes) =>
            {
                foreach (var item in templateTypes)
                {
                    viewModel.TemplateTypeList.Add(item);
                }

                viewModel.SelectedTemplateType = templateType.Key;
            };

            Loaded += Window_Loaded;

            InitializeComponent();
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

            return (closedByOkay, getDialogResult());
        }

        IModelingOptionsDialog IModelingOptionsDialog.ApplyPresets(ModelingOptionsModel presets)
        {
            applyPresets(presets);
            return this;
        }

        public void PublishTemplateTypes(TemplateTypeItem templateType, IList<TemplateTypeItem> allowedTemplateTypes)
        {
            setTemplateTypes(templateType, allowedTemplateTypes);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FirstTextBox.Focus();
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

        private void DocLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri),
            };
            process.Start();
        }
    }
}
