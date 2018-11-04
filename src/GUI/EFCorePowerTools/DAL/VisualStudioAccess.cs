namespace EFCorePowerTools.DAL
{
    using System;
    using EnvDTE80;
    using ErikEJ.SqlCeToolbox.Helpers;
    using Shared.DAL;
    using Shared.Enums;
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

        void IVisualStudioAccess.NavigateToUrl(string url)
        {
            _dte2.ItemOperations.Navigate(url);
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
    }
}