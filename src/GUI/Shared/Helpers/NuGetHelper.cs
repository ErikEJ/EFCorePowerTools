using System.Diagnostics;
using Community.VisualStudio.Toolkit;
using NuGet.Versioning;

namespace EFCorePowerTools.Helpers
{
    public class NuGetHelper
    {
        public void InstallPackage(string packageId, Project project, NuGetVersion version = null)
        {
            var args = $"add \"{project.FullPath}\" package {packageId} ";
            if (version != null)
            {
                args += "-v " + version.ToString();
            }

            var startInfo = new ProcessStartInfo
            {
#pragma warning disable S4036 // OS commands should not rely on PATH resolution
                FileName = "dotnet",
#pragma warning restore S4036 // OS commands should not rely on PATH resolution
                Arguments = args,
            };

            RunProcess(startInfo);
        }

        private static void RunProcess(ProcessStartInfo startInfo)
        {
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            Process.Start(startInfo);
        }
    }
}