using System;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;

namespace EFCorePowerTools.Dialogs
{
    public partial class EfCoreMigrationsDialog : IMigrationOptionsDialog
    {
        private readonly Action<Project> useProjectForMigration;
        private readonly Action<string> useOutputPath;

        public EfCoreMigrationsDialog(
            ITelemetryAccess telemetryAccess,
            IMigrationOptionsViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(EfCoreMigrationsDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };

            useProjectForMigration = viewModel.UseProjectForMigration;
            useOutputPath = viewModel.UseOutputPath;

            InitializeComponent();
        }

        (bool ClosedByOK, object Payload) IDialog<object>.ShowAndAwaitUserResponse(bool modal)
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

        IMigrationOptionsDialog IMigrationOptionsDialog.UseProjectForMigration(Project project)
        {
            useProjectForMigration(project);
            return this;
        }

        IMigrationOptionsDialog IMigrationOptionsDialog.UseOutputPath(string outputPath)
        {
            useOutputPath(outputPath);
            return this;
        }
    }
}
