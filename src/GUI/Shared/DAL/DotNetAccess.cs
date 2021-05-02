namespace EFCorePowerTools.DAL
{
    using Shared.DAL;
    using System;

    public class DotNetAccess : IDotNetAccess
    {
        Version IDotNetAccess.GetExtensionVersion() => typeof(EFCorePowerToolsPackage).Assembly.GetName().Version;
    }
}