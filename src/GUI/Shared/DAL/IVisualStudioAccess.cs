namespace EFCorePowerTools.Shared.DAL
{
    using Microsoft.VisualStudio.Data.Services;
    using Models;
    using System.Threading.Tasks;

    public interface IVisualStudioAccess
    {
        DatabaseConnectionModel PromptForNewDatabaseConnection();

        DatabaseDefinitionModel PromptForNewDatabaseDefinition();

        void RemoveDatabaseConnection(IVsDataConnection dataConnection);

        void ShowMessage(string message);

        Task StartStatusBarAnimationAsync();
        Task StopStatusBarAnimationAsync();

        Task SetStatusBarTextAsync(string text);
        void ShowError(string error);

        void OpenFile(string fileName);
    }
}