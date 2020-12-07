namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using ReverseEngineer20.ReverseEngineer;
    using Shared.Models;

    public interface IPickTablesDialog : IDialog<PickTablesDialogResult>
    {
        IPickTablesDialog AddTables(IEnumerable<TableModel> tables, IEnumerable<Schema> customReplacers);

        IPickTablesDialog PreselectTables(IEnumerable<SerializationTableModel> tables);
    }
}