namespace EFCorePowerTools.Contracts.ViewModels
{
    using RevEng.Shared;

    public interface IColumnInformationViewModel : IViewModel
    {
        string Name { get; set; }

        bool IsSelected { get; set; }

        bool IsPrimaryKey { get; set; }

        bool IsColumn { get; }
    }
}