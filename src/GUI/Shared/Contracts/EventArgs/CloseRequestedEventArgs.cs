namespace EFCorePowerTools.Contracts.EventArgs
{
    public class CloseRequestedEventArgs : System.EventArgs
    {
        public CloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public bool? DialogResult { get; }
    }
}
