using System;

namespace EFCorePowerTools.Messages
{
    public class StatusbarEventArgs : EventArgs
    {
        public StatusbarEventType EventType { get; set; }

        public StatusbarEventArgs(StatusbarEventType eventType)
        {
            EventType = eventType;
        }
    }
}
