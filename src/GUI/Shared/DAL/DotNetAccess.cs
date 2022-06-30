using System;
using EFCorePowerTools.Common.DAL;

namespace EFCorePowerTools.DAL
{
    public class DotNetAccess : IDotNetAccess
    {
        Version IDotNetAccess.GetExtensionVersion() => typeof(EFCorePowerToolsPackage).Assembly.GetName().Version;
    }
}