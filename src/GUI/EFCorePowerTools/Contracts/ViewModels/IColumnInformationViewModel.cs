using System.Windows.Input;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IColumnInformationViewModel : IObjectTreeEditableViewModel, IObjectTreeSelectableViewModel, IViewModel
    {
        bool IsPrimaryKey { get; set; }

        bool IsColumn { get; }

        bool IsTableSelected { get; set; }

        bool IsEnabled { get; }

        void SetSelected(bool value);
    }
}