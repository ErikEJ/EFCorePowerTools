namespace EFCorePowerTools.Shared.DAL
{
    using System;

    public interface IDotNetAccess
    {
        Version GetExtensionVersion();

        Version GetAssemblyVersion(string assemblyName);

        bool DoesDbProviderFactoryExist(string providerInvariantName);
    }
}