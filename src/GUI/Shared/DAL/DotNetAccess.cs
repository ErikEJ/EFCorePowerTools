namespace EFCorePowerTools.DAL
{
    using Common.DAL;
    using System;

    public class DotNetAccess : IDotNetAccess
    {
        Version IDotNetAccess.GetExtensionVersion() => typeof(EFCorePowerToolsPackage).Assembly.GetName().Version;
    }
}