namespace EFCorePowerTools.Contracts.EventArgs
{
    using System;

    public class CloseRequestedEventArgs : EventArgs
    {
        public bool? DialogResult { get; }

        public CloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }
    }
}