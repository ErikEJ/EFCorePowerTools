using Community.VisualStudio.Toolkit;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EFCorePowerTools.Helpers
{
    public class NuGetHelper
    {
        public async Task<bool> InstallPackageAsync(string packageId, Project project, Version version = null)
        {
            var args = $"add {project.FullPath} package {packageId} ";
            if (version != null)
            {
                args += "-v " + version.ToString(3);
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = args,
            };

            var result = await RunProcessAsync(startInfo);

            if (string.IsNullOrWhiteSpace(result))
            {
                return false;
            }

            return true;
        }

        private static async Task<string> RunProcessAsync(ProcessStartInfo startInfo)
        {
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.StandardOutputEncoding = Encoding.UTF8;
            var standardOutput = new StringBuilder();
            var error = string.Empty;

            using (var process = Process.Start(startInfo))
            {
                while (process != null && !process.HasExited)
                {
                    standardOutput.Append(await process.StandardOutput.ReadToEndAsync());
                }
                if (process != null)
                {
                    standardOutput.Append(await process.StandardOutput.ReadToEndAsync());
                }
                if (process != null)
                {
                    error = await process.StandardError.ReadToEndAsync();
                }
            }

            var result = standardOutput.ToString();

            if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(error))
            {
                result = "Error:" + Environment.NewLine + error;
            }

            return result;
        }
    }
}
