namespace ErikEJ.SqlCeToolbox.Helpers
{
    using ReverseEngineer20.ReverseEngineer;

    public class CheckListItem
    {
        public TableInformation TableInformation { get; set; }

        public bool IsChecked { get; set; }

        public string Tag { get; set; }

        public override string ToString()
        {
            return TableInformation.UnsafeFullName;
        }
    }
}
