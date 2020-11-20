namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using Shared.Models;

    public interface IPickTablesDialog : IDialog<TableModel[]>
    {
        IPickTablesDialog AddTables(IEnumerable<TableModel> tables);

        IPickTablesDialog PreselectTables(IEnumerable<TableModel> tables);
    }
}