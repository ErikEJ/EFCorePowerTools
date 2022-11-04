using RevEng.Common;
using RevEng.Common.TableColumnRenaming;

namespace EFCorePowerTools.Contracts.Views
{
    public class PickTablesDialogResult
    {
        public SerializationTableModel[] Objects { get; set; }

        public Schema[] CustomReplacers { get; set; }
    }
}