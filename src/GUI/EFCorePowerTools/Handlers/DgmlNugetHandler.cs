using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.IO;
using System.Reflection;
using System.Text;

namespace EFCorePowerTools.Handlers
{
    internal class DgmlNugetHandler
    {
        private readonly EFCorePowerToolsPackage _package;

        public DgmlNugetHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        public async System.Threading.Tasks.Task InstallDgmlNugetAsync(Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            _package.Dte2.StatusBar.Text = DgmlLocale.InstallingPackage;
            var nuGetHelper = new NuGetHelper();
            await nuGetHelper.InstallPackageAsync("ErikEJ.EntityFrameworkCore.DgmlBuilder", project);
            _package.Dte2.StatusBar.Text = DgmlLocale.PackageInstalled;
            var path = Path.GetTempFileName() + ".txt";
            File.WriteAllText(path, GetReadme(), Encoding.UTF8);
            var window = _package.Dte2.ItemOperations.OpenFile(path);
            window.Document.Activate();
            Telemetry.TrackEvent("PowerTools.InstallDgmlNuget");
        }

        private string GetReadme()
        {
            var resourceName = "EFCorePowerTools.DgmlBuilder.readme.txt";

            Stream stream = null;
            try
            {
                stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                using (StreamReader reader = new StreamReader(stream))
                {
                    stream = null;
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }
    }
}
