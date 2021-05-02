namespace EFCorePowerTools.Shared.DAL
{
    using Microsoft.VisualStudio.Data.Services;
    using Models;
    using System;

    public interface IVisualStudioAccess
    {
        DatabaseConnectionModel PromptForNewDatabaseConnection();

        DatabaseDefinitionModel PromptForNewDatabaseDefinition();

        void RemoveDatabaseConnection(IVsDataConnection dataConnection);

        void ShowMessage(string message);

        void StartStatusBarAnimation(ref object icon);
        void StopStatusBarAnimation(ref object icon);

        void SetStatusBarText(string text);
        void ShowError(string error);

        void OpenFile(string fileName);
    }
}