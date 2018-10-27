namespace EFCorePowerTools.DAL
{
    using System;
    using EnvDTE80;
    using ErikEJ.SqlCeToolbox.Helpers;
    using Shared.DAL;

    public class VisualStudioAccess : IVisualStudioAccess
    {
        private readonly DTE2 _dte2;

        public VisualStudioAccess(DTE2 dte2)
        {
            _dte2 = dte2;
        }

        void IVisualStudioAccess.NavigateToUrl(string url)
        {
            _dte2.ItemOperations.Navigate(url);
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