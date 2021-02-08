namespace EFCorePowerTools.Contracts.Views
{
    using RevEng.Shared;

    public class PickTablesDialogResult
    {
        public SerializationTableModel[] Objects { get; set; }

        public Schema[] CustomReplacers { get; set; }
    }
}