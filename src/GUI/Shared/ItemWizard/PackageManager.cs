using System.Diagnostics;

namespace EFCorePowerTools.ItemWizard
{
    internal static class PackageManager
    {
        private static EFCorePowerToolsPackage package;

        public static EFCorePowerToolsPackage Package
        {
            get
            {
                Debug.Assert(package != null, "PackageManager.Package: package is null and someone is trying to access it!");
                return package;
            }

            set
            {
                package = value;
            }
        }
    }
}