namespace EFCorePowerTools.DAL
{
    using System;
    using System.Reflection;
    using Shared.DAL;

    public class DotNetAccess : IDotNetAccess
    {
        Version IDotNetAccess.GetExtensionVersion() => typeof(EFCorePowerToolsPackage).Assembly.GetName().Version;

        Version IDotNetAccess.GetAssemblyVersion(string assemblyName)
        {
            try
            {
                var asm = Assembly.Load(assemblyName);
                return asm.GetName().Version;
            }
            catch
            {
                return null;
            }
        }

        bool IDotNetAccess.DoesDbProviderFactoryExist(string providerInvariantName)
        {
            try
            {
                System.Data.Common.DbProviderFactories.GetFactory(providerInvariantName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}