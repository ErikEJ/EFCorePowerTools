namespace EFCorePowerTools.Contracts.ViewModels
{
    using RevEng.Shared;
    using System.Collections.ObjectModel;

    public interface IObjectTreeRootItemViewModel : IViewModel
    {
        bool? IsSelected { get; set; }

        bool IsVisible { get; }

        ObservableCollection<ITableInformationViewModel> Objects { get; }
    }
}