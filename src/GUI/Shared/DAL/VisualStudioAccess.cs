using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Helpers;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.DAL
{
    public class VisualStudioAccess : IVisualStudioAccess
    {
        DatabaseConnectionModel IVisualStudioAccess.PromptForNewDatabaseConnection()
        {
            DatabaseConnectionModel info = null;

            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                // Switch to main thread
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                info = await VsDataHelper.PromptForInfoAsync();
            });

#pragma warning disable S2583 // Conditionally executed code should be reachable
            if (info == null)
            {
                return null;
            }
#pragma warning restore S2583 // Conditionally executed code should be reachable

            if (info.DatabaseType == DatabaseType.Undefined)
            {
                return null;
            }

            return new DatabaseConnectionModel
            {
                ConnectionName = info.ConnectionName,
                ConnectionString = info.ConnectionString,
                DatabaseType = info.DatabaseType,
                DataConnection = info.DataConnection,
            };
        }

        DatabaseConnectionModel IVisualStudioAccess.PromptForNewDacpacDatabaseDefinition()
        {
            var fileName = VsDataHelper.PromptForDacpac();

            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            return new DatabaseConnectionModel
            {
                FilePath = fileName,
                DatabaseType = DatabaseType.SQLServerDacpac,
            };
        }

        async System.Threading.Tasks.Task IVisualStudioAccess.RemoveDatabaseConnectionAsync(IVsDataConnection dataConnection)
        {
            await VsDataHelper.RemoveDataConnectionAsync(dataConnection);
        }

        void IVisualStudioAccess.ShowMessage(string message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            VSHelper.ShowMessage(message);
        }

        async System.Threading.Tasks.Task IVisualStudioAccess.StartStatusBarAnimationAsync()
        {
            await VS.StatusBar.StartAnimationAsync(StatusAnimation.Build);
        }

        async System.Threading.Tasks.Task IVisualStudioAccess.StopStatusBarAnimationAsync()
        {
            await VS.StatusBar.EndAnimationAsync(StatusAnimation.Build);
        }

        async System.Threading.Tasks.Task IVisualStudioAccess.SetStatusBarTextAsync(string text)
        {
            await VS.StatusBar.ShowMessageAsync(text);
        }

        void IVisualStudioAccess.ShowError(string error)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            VSHelper.ShowError(error);
        }

        void IVisualStudioAccess.OpenFile(string fileName)
        {
            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                // Switch to main thread
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                await VS.Documents.OpenAsync(fileName);
            });
        }
    }
}
