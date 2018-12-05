namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using Shared.Models;

    public interface IPickTablesDialog : IDialog<TableInformationModel[]>
    {
        IPickTablesDialog AddTables(IEnumerable<TableInformationModel> tables);

        IPickTablesDialog PreselectTables(IEnumerable<TableInformationModel> tables);
    }
}