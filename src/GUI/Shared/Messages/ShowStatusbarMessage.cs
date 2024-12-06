namespace EFCorePowerTools.Messages
{
    public class ShowStatusbarMessage
    {
        public ShowStatusbarMessage()
        {
            Type = StatusbarMessageTypes.Status;
        }

        public ShowStatusbarMessage(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Gets or sets - default status bar message will be Progress
        /// </summary>
        public StatusbarMessageTypes Type { get; set; } = StatusbarMessageTypes.Progress;

        /// <summary>
        /// Gets or sets Content to display in statusbar
        /// </summary>
        public string Content { get; set; }
    }
}
