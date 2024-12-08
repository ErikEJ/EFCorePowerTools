namespace EFCorePowerTools.Messages
{
    public class ShowStatusbarMessage
    {
        public ShowStatusbarMessage()
        {
            Type = StatusbarMessageTypes.Status;
        }

        public ShowStatusbarMessage(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets or sets - default status bar message will be Progress
        /// </summary>
        public StatusbarMessageTypes Type { get; set; } = StatusbarMessageTypes.Progress;

        /// <summary>
        /// Gets or sets message to display in statusbar
        /// </summary>
        public string Message { get; set; }

        public string Content { get; set; }
    }
}
