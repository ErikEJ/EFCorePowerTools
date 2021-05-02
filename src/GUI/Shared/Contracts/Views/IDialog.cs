namespace EFCorePowerTools.Contracts.Views
{
    public interface IDialog<TPayload> : IView
    {
        /// <summary>
        /// Shows the dialog (either <paramref name="modal"/> or not) and awaits the users response.
        /// </summary>
        /// <param name="modal"><b>True</b>, if the dialog should be modal, otherwise false.</param>
        /// <returns>A value tuple containing if the user closed the dialog with <b>OK</b>, and any optional payload.</returns>
        (bool ClosedByOK, TPayload Payload) ShowAndAwaitUserResponse(bool modal);
    }
}