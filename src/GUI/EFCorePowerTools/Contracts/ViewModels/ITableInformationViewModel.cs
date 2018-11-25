namespace EFCorePowerTools.Contracts.ViewModels
{
    using Shared.Models;

    public interface ITableInformationViewModel : IViewModel
    {
        TableInformationModel Model { get; set; }

        bool IsSelected { get; set; }
    }
}