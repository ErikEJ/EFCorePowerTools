using System.Threading.Tasks;
using EFCorePowerTools.Common.Models;
using Microsoft.VisualStudio.Data.Services;

namespace EFCorePowerTools.Common.DAL
{
    public interface IVisualStudioAccess
    {
        DatabaseConnectionModel PromptForNewDatabaseConnection();

        DatabaseConnectionModel PromptForNewDacpacDatabaseDefinition();

        Task RemoveDatabaseConnectionAsync(IVsDataConnection dataConnection);

        void ShowMessage(string message);

        Task StartStatusBarAnimationAsync();
        Task StopStatusBarAnimationAsync();

        Task SetStatusBarTextAsync(string text);
        void ShowError(string error);

        void OpenFile(string fileName);
    }
}
