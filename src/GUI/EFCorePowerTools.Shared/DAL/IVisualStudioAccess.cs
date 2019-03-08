namespace EFCorePowerTools.Shared.DAL
{
    using System;
    using Models;

    public interface IVisualStudioAccess
    {
        void NavigateToUrl(string url);
        DatabaseConnectionModel PromptForNewDatabaseConnection();
        void ShowMessage(string message);

        bool IsDdexProviderInstalled(Guid id);
        bool IsSqLiteDbProviderInstalled();

        void StartStatusBarAnimation(ref object icon);
        void StopStatusBarAnimation(ref object icon);

        void SetStatusBarText(string text);
        void ShowError(string error);

        void OpenFile(string fileName);
    }
}