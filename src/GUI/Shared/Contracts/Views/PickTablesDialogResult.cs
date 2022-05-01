namespace EFCorePowerTools.Contracts.Views
{
    using RevEng.Common;

    public class PickTablesDialogResult
    {
        public SerializationTableModel[] Objects { get; set; }

        public Schema[] CustomReplacers { get; set; }
    }
}