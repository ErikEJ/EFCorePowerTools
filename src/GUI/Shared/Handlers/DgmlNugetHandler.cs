using System.IO;
using System.Reflection;
using System.Text;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.Handlers
{
    internal static class DgmlNugetHandler
    {
        public static async System.Threading.Tasks.Task InstallDgmlNugetAsync(Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            await VS.StatusBar.ShowMessageAsync(DgmlLocale.InstallingPackage);
            var nuGetHelper = new NuGetHelper();
            nuGetHelper.InstallPackage("ErikEJ.EntityFrameworkCore.DgmlBuilder", project);
            await VS.StatusBar.ShowMessageAsync(DgmlLocale.PackageInstalled);
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) + ".txt";
            File.WriteAllText(path, GetReadme(), Encoding.UTF8);
            var window = await VS.Documents.OpenInPreviewTabAsync(path);
            await window.WindowFrame.ShowAsync();
            Telemetry.TrackEvent("PowerTools.InstallDgmlNuget");
        }

        public static void UnzipT4Templates(Project project)
        {
            var path = Path.GetDirectoryName(project.FullPath);

            ReverseEngineerHelper.DropT4Templates(path);
        }

        private static string GetReadme()
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
                {
                    stream.Dispose();
                }
            }
        }
    }
}