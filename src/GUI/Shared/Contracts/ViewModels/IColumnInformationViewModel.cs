using System.Collections.ObjectModel;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IColumnInformationViewModel : IObjectTreeEditableViewModel, IObjectTreeSelectableViewModel, IViewModel
    {
        ObservableCollection<IColumnChildrenViewModel> Children { get; }

        bool IsPrimaryKey { get; set; }

        bool IsForeignKey { get; set; }

        bool IsColumn { get; }

        bool IsTableSelected { get; set; }

        bool IsEnabled { get; }

        void SetSelected(bool value);
    }
}
