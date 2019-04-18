namespace EFCorePowerTools.Dialogs
{
    using System;
    using Contracts.ViewModels;
    using Contracts.Views;
    using EnvDTE;
    using Shared.DAL;

    public partial class EfCoreMigrationsDialog : IMigrationOptionsDialog
    {
        private readonly Action<Project> _useProjectForMigration;
        private readonly Action<string> _useOutputPath;

        public EfCoreMigrationsDialog(ITelemetryAccess telemetryAccess,
                                      IMigrationOptionsViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(EfCoreMigrationsDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };

            _useProjectForMigration = viewModel.UseProjectForMigration;
            _useOutputPath = viewModel.UseOutputPath;

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
            _useProjectForMigration(project);
            return this;
        }

        IMigrationOptionsDialog IMigrationOptionsDialog.UseOutputPath(string outputPath)
        {
            _useOutputPath(outputPath);
            return this;
        }
    }
}
