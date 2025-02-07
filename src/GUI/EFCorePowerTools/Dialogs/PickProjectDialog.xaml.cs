using System;
using System.Collections.Generic;
using System.Windows;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Contracts.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;

namespace EFCorePowerTools.Dialogs
{
    public partial class PickProjectDialog : IPickProjectDialog
    {
        private readonly Func<ProjectModel> getDialogResult;
        private readonly Action<IEnumerable<ProjectModel>> addProjects;

        public PickProjectDialog(
            ITelemetryAccess telemetryAccess,
            IPickProjectViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(PickProjectDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            getDialogResult = () => viewModel.SelectedProject;

            addProjects = models =>
            {
                foreach (var model in models)
                {
                    viewModel.Projects.Add(model);
                }
            };

            InitializeComponent();
        }

        (bool ClosedByOK, ProjectModel Payload) IDialog<ProjectModel>.ShowAndAwaitUserResponse(bool modal)
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

        public void PublishProjects(IEnumerable<ProjectModel> projects)
        {
            addProjects(projects);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectCombobox.Focus();
        }
    }
}