namespace EFCorePowerTools.DAL
{
    using System;
    using EnvDTE80;
    using ErikEJ.SqlCeToolbox.Helpers;
    using Microsoft.VisualStudio.Shell.Interop;
    using ReverseEngineer20;
    using Shared.DAL;
    using Shared.Models;

    public class VisualStudioAccess : IVisualStudioAccess
    {
        private readonly EFCorePowerToolsPackage _package;
        private readonly DTE2 _dte2;

        public VisualStudioAccess(EFCorePowerToolsPackage package,
                                  DTE2 dte2)
        {
            _package = package;
            _dte2 = dte2;
        }

        DatabaseConnectionModel IVisualStudioAccess.PromptForNewDatabaseConnection()
        {
            var info = EnvDteHelper.PromptForInfo(_package);
            if (info.DatabaseType == DatabaseType.Undefined)
                return null;

            return new DatabaseConnectionModel
            {
                ConnectionName = info.Caption,
                ConnectionString = info.ConnectionString,
                DatabaseType = info.DatabaseType
            };
        }

        void IVisualStudioAccess.ShowMessage(string message)
        {
            EnvDteHelper.ShowMessage(message);
        }

        bool IVisualStudioAccess.IsDdexProviderInstalled(Guid id)
        {
            try
            {
                return EnvDteHelper.DdexProviderIsInstalled(id);
            }
            catch
            {
                return false;
            }
        }

        bool IVisualStudioAccess.IsSqLiteDbProviderInstalled() => EnvDteHelper.IsSqLiteDbProviderInstalled();

        void IVisualStudioAccess.StartStatusBarAnimation(ref object icon)
        {
            var statusBar = (IVsStatusbar)_package.GetService<SVsStatusbar>();
            statusBar.Animation(1, ref icon);
        }

        void IVisualStudioAccess.StopStatusBarAnimation(ref object icon)
        {
            var statusBar = (IVsStatusbar)_package.GetService<SVsStatusbar>();
            statusBar.Animation(0, ref icon);
        }

        void IVisualStudioAccess.SetStatusBarText(string text)
        {
            _package.Dte2.StatusBar.Text = text;
        }

        void IVisualStudioAccess.ShowError(string error)
        {
            EnvDteHelper.ShowError(error);
        }

        void IVisualStudioAccess.OpenFile(string fileName)
        {
            _package.Dte2.ItemOperations.OpenFile(fileName);
        }
    }
}