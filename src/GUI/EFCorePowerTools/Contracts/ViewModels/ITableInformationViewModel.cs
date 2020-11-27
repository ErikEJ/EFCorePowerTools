namespace EFCorePowerTools.Contracts.ViewModels
{
    using RevEng.Shared;
    using System.Collections.ObjectModel;

    public interface ITableInformationViewModel : IViewModel
    {
        string Name { get; set; }

        bool HasPrimaryKey { get; set; }

        ObjectType ObjectType { get; set; }

        ObservableCollection<IColumnInformationViewModel> Columns { get; }

        bool IsProcedure { get; }

        bool IsSelected { get; set; }

        bool IsTableWithKey { get; }

        bool IsTableWithoutKey { get; }

        bool IsView { get; }

        bool IsVisible { get; set; }
    }
}