using RevEng.Common;

namespace EFCorePowerTools.Contracts.Views
{
    public class PickTablesDialogResult
    {
        public SerializationTableModel[] Objects { get; set; }

        public Schema[] CustomReplacers { get; set; }
    }
}