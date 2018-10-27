namespace EFCorePowerTools.Dialogs
{
    using Contracts.ViewModels;
    using Contracts.Views;
    using Shared.DAL;

    public partial class AboutDialog : IAboutDialog
    {
        public AboutDialog(ITelemetryAccess telemetryAccess,
                           IAboutViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(AboutDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) => Close();

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
    }
}