using Microsoft.VisualStudio.ComponentModelHost;
using NuGet.VisualStudio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EFCorePowerTools.Helpers
{
    public class NuGetHelper
    {
        public void InstallPackage(string packageId, EFCorePowerToolsPackage package, Version version = null)
        {
            var dte2 = package.GetService<Microsoft.VisualStudio.Shell.Interop.SDTE>() as EnvDTE80.DTE2;
            var project = dte2.SelectedItems.Item(0).Project;

            var componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SComponentModel));
            var nuGetInstaller = componentModel.GetService<IVsPackageInstaller>();
            nuGetInstaller?.InstallPackage(null, project, packageId, version, false);
        }

        public async Task InstallPackageAsync(string packageId, EFCorePowerToolsPackage package, CancellationToken ct = default(CancellationToken))
        {
            await Task.Run(() => InstallPackage(packageId, package), ct);
        }
    }
}
