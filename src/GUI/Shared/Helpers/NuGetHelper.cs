using System;
using System.Diagnostics;
using Community.VisualStudio.Toolkit;

namespace EFCorePowerTools.Helpers
{
    public class NuGetHelper
    {
        public bool InstallPackage(string packageId, Project project, Version version = null)
        {
            var args = $"add {project.FullPath} package {packageId} ";
            if (version != null)
            {
                args += " -v " + version.ToString(3);
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = args,
            };

            var result = RunProcess(startInfo);

            return result;
        }

        private static bool RunProcess(ProcessStartInfo startInfo)
        {
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            var completed = true;

            using (var process = Process.Start(startInfo))
            {
                while (process != null && !process.HasExited)
                {
                    completed = false;
                }

                if (process != null && process.HasExited)
                {
                    completed = process.ExitCode == 0;
                }
            }

            return completed;
        }
    }
}
