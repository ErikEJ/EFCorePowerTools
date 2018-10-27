namespace EFCorePowerTools.Shared.DAL
{
    using System;

    public interface IVisualStudioAccess
    {
        void NavigateToUrl(string url);

        bool IsDdexProviderInstalled(Guid id);
        bool IsSqLiteDbProviderInstalled();
    }
}