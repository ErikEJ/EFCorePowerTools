using System.Windows;
using System.Windows.Controls;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;

namespace EFCorePowerTools.Wizard
{
    public partial class Wiz4_StatusDialog : WizardResultPageFunction
    {
        private readonly WizardDataViewModel wizardViewModel;

        public Wiz4_StatusDialog(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            this.wizardViewModel = viewModel;

            Loaded += WizardPage4_Loaded;

            InitializeComponent();

            Loaded += (s, e) => OnPageVisible(s, null);
        }

        private void WizardPage4_Loaded(object sender, RoutedEventArgs e)
        {
            WindowTitle = "Status";
        }

#pragma warning disable SA1202 // Elements should be ordered by access
        protected override void OnPageVisible(object sender, StatusbarEventArgs e)
#pragma warning restore SA1202 // Elements should be ordered by access
        {
            IsPageLoaded = wizardViewModel.IsPage4Initialized;

            if (wizardViewModel.ErrorMessage != null)
            {
                wizardViewModel.GenerateStatus = wizardViewModel.ErrorMessage;
                Statusbar.Status.ShowStatusError("Error occurred");
                PreviousButton.IsEnabled = false;
                FinishButton.IsEnabled = true;
                return;
            }

            if (!IsPageLoaded)
            {
                // When generating we'll initialize the page to known state
                wizardViewModel.GenerateStatus = string.Empty;
                Statusbar.Status.ShowStatus(ReverseEngineerLocale.StatusbarGeneratingFiles);
                PreviousButton.IsEnabled = false;
                FinishButton.IsEnabled = false;
            }
        }

        private void TextChangedEventHandler(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && !string.IsNullOrEmpty(textBox.Text) && PreviousButton != null && FinishButton != null)
            {
                // If here then we have status update - enabled buttons
                Statusbar.Status.ShowStatus(); // Will reset status bar
                PreviousButton.IsEnabled = true;
                FinishButton.IsEnabled = true;
            }
        }
    }
}