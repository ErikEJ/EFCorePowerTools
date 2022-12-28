using System;
using System.Diagnostics;
using EFCorePowerTools.Common.DAL;

namespace EFCorePowerTools.DAL
{
    public class DotNetAccess : IDotNetAccess
    {
        Version IDotNetAccess.GetExtensionVersion() => new Version(FileVersionInfo.GetVersionInfo(typeof(EFCorePowerToolsPackage).Assembly.Location).FileVersion);
    }
}
