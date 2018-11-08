namespace ErikEJ.SqlCeToolbox.Helpers
{
    using EFCorePowerTools.Shared.Models;

    public class CheckListItem
    {
        public TableInformationModel TableInformationModel { get; set; }

        public bool IsChecked { get; set; }

        public string Tag { get; set; }

        public override string ToString()
        {
            return TableInformationModel.UnsafeFullName;
        }
    }
}
