namespace EFCorePowerTools.DAL
{
    using EFCorePowerTools.Helpers;
    using Microsoft.VisualStudio.Data.Services;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using RevEng.Shared;
    using Shared.DAL;
    using Shared.Models;

    public class VisualStudioAccess : IVisualStudioAccess
    {
        private readonly EFCorePowerToolsPackage _package;

        public VisualStudioAccess(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        DatabaseConnectionModel IVisualStudioAccess.PromptForNewDatabaseConnection()
        {
            var info = VsDataHelper.PromptForInfo(_package);
            if (info.DatabaseType == DatabaseType.Undefined)
                return null;

            return new DatabaseConnectionModel
            {
                ConnectionName = info.ConnectionName,
                ConnectionString = info.ConnectionString,
                DatabaseType = info.DatabaseType,
                DataConnection = info.DataConnection,
            };
        }

        DatabaseDefinitionModel IVisualStudioAccess.PromptForNewDatabaseDefinition()
        {
            var fileName = VsDataHelper.PromptForDacpac();

            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            return new DatabaseDefinitionModel
            {
                FilePath = fileName,
            };
        }

        void IVisualStudioAccess.RemoveDatabaseConnection(IVsDataConnection dataConnection)
        {
            VsDataHelper.RemoveDataConnection(_package, dataConnection);
        }

        void IVisualStudioAccess.ShowMessage(string message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            EnvDteHelper.ShowMessage(message);
        }

        void IVisualStudioAccess.StartStatusBarAnimation(ref object icon)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            var statusBar = (IVsStatusbar)_package.GetService<SVsStatusbar>();
            statusBar.Animation(1, ref icon);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        void IVisualStudioAccess.StopStatusBarAnimation(ref object icon)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            var statusBar = (IVsStatusbar)_package.GetService<SVsStatusbar>();
            statusBar.Animation(0, ref icon);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        void IVisualStudioAccess.SetStatusBarText(string text)
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate {
                // Switch to main thread
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _package.Dte2.StatusBar.Text = text;
            });
            
        }

        void IVisualStudioAccess.ShowError(string error)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            EnvDteHelper.ShowError(error);
        }

        void IVisualStudioAccess.OpenFile(string fileName)
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate {
                // Switch to main thread
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _package.Dte2.ItemOperations.OpenFile(fileName);
            });
            
        }
    }
}