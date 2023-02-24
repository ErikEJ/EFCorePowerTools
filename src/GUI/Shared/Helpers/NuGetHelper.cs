using System;
using System.Diagnostics;
using Community.VisualStudio.Toolkit;

namespace EFCorePowerTools.Helpers
{
    public class NuGetHelper
    {
        public void InstallPackage(string packageId, Project project, Version version = null)
        {
            var args = $"add \"{project.FullPath}\" package {packageId} ";
            if (version != null)
            {
                args += "-v " + version.ToString(3);
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
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
