using System.Windows;
using System.Windows.Controls;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;

namespace EFCorePowerTools.Dialogs
{
    public partial class MainShell : IShell
    {
        public MainShell(IShellViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();

            // Will cause dialogs to center on this control
            Owner = Application.Current.MainWindow;
        }
        public Grid MainGrid
        {
            get { return mainGrid; }
        }

    }
}
