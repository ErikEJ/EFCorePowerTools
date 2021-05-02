using System.Windows.Input;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IObjectTreeSelectableViewModel
    {
        bool? IsSelected { get; }

        ICommand SetSelectedCommand { get; }
    }
}