namespace ErikEJ.SqlCeToolbox.Helpers
{
    public class CheckListItem
    {
        public string Label { get; set; }
        public bool IsChecked { get; set; }
        public string Tag { get; set; }
        public override string ToString()
        {
            return Label;
        }
    }
}
