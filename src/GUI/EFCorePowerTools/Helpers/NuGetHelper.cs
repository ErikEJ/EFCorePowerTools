using System;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using NuGet.VisualStudio;

namespace ErikEJ.SqlCeToolbox.Helpers
{
    public class NuGetHelper
    {
        public void InstallPackage(string packageId, Project project, Version version = null)
        {
            var componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SComponentModel));
            var nuGetInstaller = componentModel.GetService<IVsPackageInstaller>();
            nuGetInstaller?.InstallPackage(null, project, packageId, version, false);
        }

        public Task InstallPackageAsync(string packageId, Project project, CancellationToken ct = default(CancellationToken))
        {
            return Task.Factory.StartNew(() => InstallPackage(packageId, project), ct); 
        }
    }
}
