using System.Collections.Generic;
using RevEng.Common;
using RevEng.Common.TableColumnRenaming;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IPickTablesDialog : IDialog<PickTablesDialogResult>
    {
        IPickTablesDialog AddTables(IEnumerable<TableModel> tables, IEnumerable<Schema> customReplacers);

        IPickTablesDialog PreselectTables(IEnumerable<SerializationTableModel> tables);
        IPickTablesDialog SqliteToolboxInstall(bool installed);
    }
}
