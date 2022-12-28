using System.Diagnostics;
using EFCorePowerTools.Common.DAL;

namespace EFCorePowerTools.DAL
{
    public class DotNetAccess : IDotNetAccess
    {
        string IDotNetAccess.GetExtensionVersion() => FileVersionInfo.GetVersionInfo(typeof(EFCorePowerToolsPackage)?.Assembly.Location).FileVersion ?? null;
    }
}
