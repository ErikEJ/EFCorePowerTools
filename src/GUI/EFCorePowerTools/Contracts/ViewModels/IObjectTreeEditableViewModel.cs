namespace EFCorePowerTools.Contracts.ViewModels
{
    using System.Windows.Input;

    public interface IObjectTreeEditableViewModel
    {
        string Name { get; set; }

        string NewName { get; set; }

        string DisplayName { get; }

        bool IsEditing { get; set; }
        
        ICommand StartEditCommand { get; }

        ICommand ConfirmEditCommand { get; }

        ICommand CancelEditCommand { get; }
    }
}