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
        public string GetOutput(string outputPath, bool isNetCore, GenerationType generationType, string migrationIdentifier, string contextName)
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
                startInfo.Arguments = "migrate \"" + outputPath + "\" " + migrationIdentifier;
            }
            if (generationType == GenerationType.MigrationAdd)
            {
                startInfo.Arguments = "addmigration \"" + outputPath + "\" " + contextName + " " + migrationIdentifier;
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

            var version = "2.0.0";
            var testFile = Path.Combine(toDir, "Microsoft.EntityFrameworkCore.dll");

            if (File.Exists(testFile))
            {
                var fvi = FileVersionInfo.GetVersionInfo(testFile);
                version = fvi.FileVersion;
                if (version.StartsWith("2.0.1"))
                {
                    version = "2.0.1";
                }
            }

            File.Copy(Path.Combine(fromDir, "efpt.exe"), Path.Combine(toDir, "efpt.exe"), true);
            if (version == "2.0.1")
            {
                File.Copy(Path.Combine(fromDir, "2.0.1", "efpt.exe.config"), Path.Combine(toDir, "efpt.exe.config"), true);
                File.Copy(Path.Combine(fromDir, "2.0.1", "Microsoft.EntityFrameworkCore.Design.dll"), Path.Combine(toDir, "Microsoft.EntityFrameworkCore.Design.dll"), true);
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
