using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EFCorePowerTools.Handlers
{
    public class ProcessLauncher
    {
        private readonly bool _isNetCore;
        private readonly bool _isNetCore21;

        public ProcessLauncher(bool isNetCore, bool isNetCore21)
        {
            _isNetCore = isNetCore;
            _isNetCore21 = isNetCore21;
        }

        public Task<string> GetOutputAsync(string outputPath, string projectPath, GenerationType generationType, string contextName, string migrationIdentifier, string nameSpace)
        {
            return Task.Factory.StartNew(() => GetOutput(outputPath, projectPath, generationType, contextName, migrationIdentifier, nameSpace));
        }

        public Task<string> GetOutputAsync(string outputPath, GenerationType generationType, string contextName)
        {
            return Task.Factory.StartNew(() => GetOutput(outputPath, null, generationType, contextName, null, null));
        }

        public string GetOutput(string outputPath, GenerationType generationType, string contextName)
        {
            return GetOutput(outputPath, null, generationType, contextName, null, null);
        }

        public List<Tuple<string, string>> BuildModelResult(string modelInfo)
        {
            var result = new List<Tuple<string, string>>();

            var contexts = modelInfo.Split(new[] { "DbContext:" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var context in contexts)
            {
                var parts = context.Split(new[] { "DebugView:" + Environment.NewLine }, StringSplitOptions.None);
                result.Add(new Tuple<string, string>(parts[0].Trim(), parts[1].Trim()));
            }

            return result;
        }

        private string GetOutput(string outputPath, string projectPath, GenerationType generationType, string contextName, string migrationIdentifier, string nameSpace)
        {
            var launchPath = _isNetCore ? DropNetCoreFiles() : DropFiles(outputPath);

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
            if (generationType == GenerationType.MigrationScript)
            {
                startInfo.Arguments = "scriptmigration \"" + outputPath + "\" " + contextName;
            }

            if (_isNetCore)
            {
                startInfo.WorkingDirectory = launchPath;
                startInfo.FileName = "dotnet";
                if (generationType == GenerationType.Ddl
                    || generationType == GenerationType.MigrationApply
                    || generationType == GenerationType.MigrationAdd
                    || generationType == GenerationType.MigrationStatus)
                {
                    startInfo.Arguments = " efpt.dll " + startInfo.Arguments;
                }
                else
                {
                    startInfo.Arguments = " efpt.dll \"" + outputPath + "\"";
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
            Version version = null;
            var testFile = Path.Combine(toDir, "Microsoft.EntityFrameworkCore.dll");


            if (File.Exists(testFile))
            {
                var fvi = FileVersionInfo.GetVersionInfo(testFile);
                version = Version.Parse(fvi.FileVersion);

                if (version.ToString(3) == "2.0.0") testVersion = "2.0.0";
                if (version.ToString(3) == "2.0.1") testVersion = "2.0.1";
                if (version.ToString(3) == "2.0.2") testVersion = "2.0.2";
                if (version.ToString(3) == "2.0.3") testVersion = "2.0.3";
                if (version.ToString(3) == "2.1.0") testVersion = "2.1.0";
                if (version.ToString(3) == "2.1.4") testVersion = "2.1.4";
            }
            else
            {
                throw new Exception(
                    $"Unable to find Microsoft.EntityFrameworkCore.dll in folder {toDir}.");
            }

            if (string.IsNullOrEmpty(testVersion))
            {
                throw new Exception(
                    $"Unable to find a supported version of Microsoft.EntityFrameworkCore.dll in folder {toDir}. Version found: {version}");
            }
            if (testVersion == "2.1.0" || testVersion == "2.1.4")
            {
                File.Copy(Path.Combine(fromDir, testVersion, "efpt.exe"), Path.Combine(toDir, "efpt.exe"), true);
            }
            else
            {
                File.Copy(Path.Combine(fromDir, "2.0.0", "efpt.exe"), Path.Combine(toDir, "efpt.exe"), true);
            }
            File.Copy(Path.Combine(fromDir, testVersion, "efpt.exe.config"), Path.Combine(toDir, "efpt.exe.config"), true);
            File.Copy(Path.Combine(fromDir, testVersion, "Microsoft.EntityFrameworkCore.Design.dll"), Path.Combine(toDir, "Microsoft.EntityFrameworkCore.Design.dll"), true);

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

            if (_isNetCore21)
            {
                ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efpt21.exe.zip"), toDir);
            }
            else
            {
                ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efpt.exe.zip"), toDir);
            }
            return toDir;
        }
    }
}
