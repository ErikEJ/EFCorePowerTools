namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using Shared.Models;

    public interface IPickTablesDialog : IDialog<SerializationTableModel[]>
    {
        IPickTablesDialog AddTables(IEnumerable<TableModel> tables);

        IPickTablesDialog PreselectTables(IEnumerable<SerializationTableModel> tables);
    }
}