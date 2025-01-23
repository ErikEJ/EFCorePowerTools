using System.Windows;
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
            //InitializeMessengerWithStatusbar(Statusbar, ReverseEngineerLocale.StatusbarGeneratingFiles);

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

            if (!IsPageLoaded)
            {
                Statusbar.Status.ShowStatus(ReverseEngineerLocale.StatusbarGeneratingFiles);
            }
        }

        private void TextChangedEventHandler(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Statusbar.Status.ShowStatus(); // Will reset status bar
        }
    }
}
