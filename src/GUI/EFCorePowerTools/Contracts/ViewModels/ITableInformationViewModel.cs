namespace EFCorePowerTools.Contracts.ViewModels
{
    using RevEng.Shared;
    using System.Collections.ObjectModel;

    public interface ITableInformationViewModel : IViewModel
    {
        string Name { get; set; }

        bool HasPrimaryKey { get; set; }

        bool ShowKeylessWarning { get;  }

        ObjectType ObjectType { get; set; }

        ObservableCollection<IColumnInformationViewModel> Columns { get; }

        bool IsProcedure { get; }

        bool IsSelected { get; set; }

        bool IsTable { get; }

        bool IsVisible { get; set; }
    }
}