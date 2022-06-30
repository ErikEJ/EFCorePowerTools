using System.Collections.ObjectModel;
using RevEng.Common;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface IObjectTreeRootItemViewModel : IObjectTreeSelectableViewModel, IViewModel
    {
        bool IsVisible { get; }

        ObservableCollection<ISchemaInformationViewModel> Schemas { get; }

        ObjectTypeIcon ObjectTypeIcon { get; }

        ObjectType ObjectType { get; set; }
    }
}