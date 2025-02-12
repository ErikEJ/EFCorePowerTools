using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace PowerToolsExtensionPack
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PowerToolsExtensionPackPackage.PackageGuidString)]
    public sealed class PowerToolsExtensionPackPackage : AsyncPackage
    {
        /// <summary>
        /// PowerToolsExtensionPackPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "a8e23dd0-b09e-4edb-9ab5-17add7e721d4";
    }
}