using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace EFCorePowerTools.Handlers
{
    public class ProcessLauncher
    {
        public string GetOutput(string outputPath, string projectPath, bool isNetCore, GenerationType generationType, string contextName, string migrationIdentifier, string nameSpace)
        {
            var launchPath = isNetCore ? DropNetCoreFiles() : DropFiles(outputPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(Path.GetDirectoryName(launchPath) ?? throw new InvalidOperationException(), "efpt.exe"),
                Arguments = "\"" + outputPath + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            if (generationType == GenerationType.Ddl)
            {
                startInfo.Arguments = "ddl \"" + outputPath + "\"";
            }
            if (generationType == GenerationType.MigrationStatus)
            {
                startInfo.Arguments = "migrationstatus \"" + outputPath + "\"";
            }
            if (generationType == GenerationType.MigrationApply)
            {
                startInfo.Arguments = "migrate \"" + outputPath + "\" " + contextName;
            }
            if (generationType == GenerationType.MigrationAdd)
            {
                startInfo.Arguments = "addmigration \"" + outputPath + "\" " + "\"" + projectPath + "\" " + contextName + " " + migrationIdentifier + " " + nameSpace;
            }

            if (isNetCore)
            {
                startInfo.WorkingDirectory = launchPath;
                startInfo.FileName = "dotnet";
                startInfo.Arguments = " efpt.dll \"" + outputPath + "\"";
                if (generationType == GenerationType.Ddl
                    || generationType == GenerationType.MigrationApply
                    || generationType == GenerationType.MigrationAdd
                    || generationType == GenerationType.MigrationStatus)
                {
                    startInfo.Arguments = " efpt.dll " + startInfo.Arguments;
                }
            }

            var standardOutput = new StringBuilder();
            using (var process = Process.Start(startInfo))
            {
                while (process != null && !process.HasExited)
                {
                    standardOutput.Append(process.StandardOutput.ReadToEnd());
                }
                if (process != null) standardOutput.Append(process.StandardOutput.ReadToEnd());
            }
            return standardOutput.ToString();
        }

        private string DropFiles(string outputPath)
        {
            var toDir = Path.GetDirectoryName(outputPath);
            var fromDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "efpt");

            Debug.Assert(fromDir != null, nameof(fromDir) + " != null");
            Debug.Assert(toDir != null, nameof(toDir) + " != null");

            var testVersion = string.Empty;
            var testFile = Path.Combine(toDir, "Microsoft.EntityFrameworkCore.dll");

            if (File.Exists(testFile))
            {
                var fvi = FileVersionInfo.GetVersionInfo(testFile);
                var version = Version.Parse(fvi.FileVersion);

                if (version.ToString(3) == "2.0.1") testVersion = "2.0.1";
                if (version.ToString(3) == "2.0.2") testVersion = "2.0.2";
                if (version.ToString(3) == "2.0.3") testVersion = "2.0.3";
            }

            File.Copy(Path.Combine(fromDir, "efpt.exe"), Path.Combine(toDir, "efpt.exe"), true);
            if (!string.IsNullOrEmpty(testVersion))
            {
                File.Copy(Path.Combine(fromDir, testVersion, "efpt.exe.config"), Path.Combine(toDir, "efpt.exe.config"), true);
                File.Copy(Path.Combine(fromDir, testVersion, "Microsoft.EntityFrameworkCore.Design.dll"), Path.Combine(toDir, "Microsoft.EntityFrameworkCore.Design.dll"), true);
            }
            else
            {
                File.Copy(Path.Combine(fromDir, "efpt.exe.config"), Path.Combine(toDir, "efpt.exe.config"), true);
                File.Copy(Path.Combine(fromDir, "Microsoft.EntityFrameworkCore.Design.dll"), Path.Combine(toDir, "Microsoft.EntityFrameworkCore.Design.dll"), true);
            }
            return outputPath;
        }

        private string DropNetCoreFiles()
        {
            var toDir = Path.Combine(Path.GetTempPath(), "efpt");
            var fromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Debug.Assert(fromDir != null, nameof(fromDir) + " != null");
            Debug.Assert(toDir != null, nameof(toDir) + " != null");

            if (Directory.Exists(toDir))
            {
                Directory.Delete(toDir, true);
            }

            Directory.CreateDirectory(toDir);

            ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efpt.exe.zip"), toDir);

            return toDir;
        }
    }
}
