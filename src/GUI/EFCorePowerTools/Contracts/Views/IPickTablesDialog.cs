namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using RevEng.Shared;

    public interface IPickTablesDialog : IDialog<PickTablesDialogResult>
    {
        IPickTablesDialog AddTables(IEnumerable<TableModel> tables, IEnumerable<Schema> customReplacers);

        IPickTablesDialog PreselectTables(IEnumerable<SerializationTableModel> tables);
    }
}