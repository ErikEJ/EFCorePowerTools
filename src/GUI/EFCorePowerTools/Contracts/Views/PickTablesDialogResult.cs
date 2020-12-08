namespace EFCorePowerTools.Contracts.Views
{
    using ReverseEngineer20.ReverseEngineer;
    using Shared.Models;

    public class PickTablesDialogResult
    {
        public SerializationTableModel[] Objects { get; set; }

        public Schema[] CustomReplacers { get; set; }
    }
}