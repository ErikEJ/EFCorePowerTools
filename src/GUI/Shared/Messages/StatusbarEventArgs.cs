using System;

namespace EFCorePowerTools.Messages
{
    public class StatusbarEventArgs : EventArgs
    {
        public StatusbarEventType EventType { get; set; }

#pragma warning disable SA1201 // Elements should appear in the correct order
        public StatusbarEventArgs(StatusbarEventType eventType)
#pragma warning restore SA1201 // Elements should appear in the correct order
        {
            EventType = eventType;
        }
    }
}
