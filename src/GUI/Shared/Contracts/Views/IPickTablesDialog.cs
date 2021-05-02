namespace EFCorePowerTools.Contracts.Views
{
    using RevEng.Shared;
    using System.Collections.Generic;

    public interface IPickTablesDialog : IDialog<PickTablesDialogResult>
    {
        IPickTablesDialog AddTables(IEnumerable<TableModel> tables, IEnumerable<Schema> customReplacers);

        IPickTablesDialog PreselectTables(IEnumerable<SerializationTableModel> tables);
    }
}