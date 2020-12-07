namespace EFCorePowerTools.Contracts.ViewModels
{
    using RevEng.Shared;
    using System.Collections.ObjectModel;

    public interface IObjectTreeRootItemViewModel : IObjectTreeSelectableViewModel, IViewModel
    {
        bool IsVisible { get; }

        ObservableCollection<ISchemaInformationViewModel> Schemas { get; }

        ObjectTypeIcon ObjectTypeIcon { get; }

        ObjectType ObjectType { get; set; }
    }
}