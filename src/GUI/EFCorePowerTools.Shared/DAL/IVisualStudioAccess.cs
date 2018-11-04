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
    }
}