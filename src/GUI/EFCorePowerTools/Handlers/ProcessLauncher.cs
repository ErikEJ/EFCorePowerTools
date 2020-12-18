using EFCorePowerTools.Extensions;
using EnvDTE;
using NuGet.ProjectModel;
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
        private readonly Project _project;

        public ProcessLauncher(Project project)
        {
            if (!project.IsNetCore30OrHigher())
            {
                throw new ArgumentException("Only .NET Core 3.0, 3.1 and 5.0 are supported");
            }
            _project = project;
        }

        public Task<string> GetOutputAsync(string outputPath, string projectPath, GenerationType generationType, string contextName, string migrationIdentifier, string nameSpace)
        {
            return GetOutputInternalAsync(outputPath, projectPath, generationType, contextName, migrationIdentifier, nameSpace);
        }

        public Task<string> GetOutputAsync(string outputPath, GenerationType generationType, string contextName)
        {
            return GetOutputInternalAsync(outputPath, null, generationType, contextName, null, null);
        }

        public List<Tuple<string, string>> BuildModelResult(string modelInfo)
        {
            var result = new List<Tuple<string, string>>();

            var contexts = modelInfo.Split(new[] { "DbContext:" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var context in contexts)
            {
                if (context.StartsWith("info:")) continue;
                if (context.StartsWith("dbug:")) continue;
                if (context.StartsWith("warn:")) continue;

                var parts = context.Split(new[] { "DebugView:" + Environment.NewLine }, StringSplitOptions.None);
                result.Add(new Tuple<string, string>(parts[0].Trim(), parts.Length > 1 ? parts[1].Trim() : string.Empty));
            }

            return result;
        }

        private async Task<string> GetOutputInternalAsync(string outputPath, string projectPath, GenerationType generationType, string contextName, string migrationIdentifier, string nameSpace)
        {
            var launchPath = await DropNetCoreFilesAsync(outputPath);

            if (_project.IsNetCore30OrHigher() && outputPath.EndsWith(".exe"))
            {
                outputPath = outputPath.Remove(outputPath.Length - 4, 4);
                outputPath += ".dll";
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(Path.GetDirectoryName(launchPath) ?? throw new InvalidOperationException(), "efpt.exe"),
                Arguments = "\"" + outputPath + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
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

            //TODO Consider improving by getting Startup project!
            // See EF Core .psm1 file

            var fileRoot =  Path.Combine(Path.GetDirectoryName(outputPath), Path.GetFileNameWithoutExtension(outputPath));
            var efptPath = Path.Combine(launchPath, "efpt.dll");

            var depsFile =  fileRoot + ".deps.json";
            var runtimeConfig = fileRoot + ".runtimeconfig.json";

            var projectAssetsFile = await _project.GetCspPropertyAsync("ProjectAssetsFile");
            var runtimeFrameworkVersion = await _project.GetCspPropertyAsync("RuntimeFrameworkVersion");

            var dotNetParams = $"exec --depsfile \"{depsFile}\" ";

            if (projectAssetsFile != null && File.Exists(projectAssetsFile) )
            {
                var lockFile = LockFileUtilities.GetLockFile(projectAssetsFile, NuGet.Common.NullLogger.Instance);

                if (lockFile != null)
                {
                    foreach (var packageFolder in lockFile.PackageFolders)
                    {
                        var path = packageFolder.Path.TrimEnd('\\');
                        dotNetParams += $"--additionalprobingpath \"{path}\" ";
                    }
                }
            }

            if (File.Exists(runtimeConfig))
            {
                dotNetParams += $"--runtimeconfig \"{runtimeConfig}\" ";
            }
            else if (runtimeFrameworkVersion != null)
            {
                dotNetParams += $"--fx-version {runtimeFrameworkVersion} ";
            }

            dotNetParams += $"\"{efptPath}\" ";

            startInfo.WorkingDirectory = Path.GetDirectoryName(outputPath);
            startInfo.FileName = "dotnet";
            if (generationType == GenerationType.Ddl
                || generationType == GenerationType.MigrationApply
                || generationType == GenerationType.MigrationAdd
                || generationType == GenerationType.MigrationStatus
                || generationType == GenerationType.MigrationScript)
            {
                startInfo.Arguments = dotNetParams + " " + startInfo.Arguments;
            }
            else
            {
                startInfo.Arguments = dotNetParams + " \"" + outputPath + "\"";
            }

            Debug.WriteLine(startInfo.Arguments);

            var standardOutput = new StringBuilder();
            using (var process = System.Diagnostics.Process.Start(startInfo))
            {
                while (process != null && !process.HasExited)
                {
                    standardOutput.Append(await process.StandardOutput.ReadToEndAsync());
                }
                if (process != null) standardOutput.Append(await process.StandardOutput.ReadToEndAsync());
            }
            return standardOutput.ToString();
        }

        private async Task<string> DropNetCoreFilesAsync(string outputPath)
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

            var versionInfo = await _project.ContainsEfCoreDesignReferenceAsync();

            if (versionInfo.Item2.StartsWith("5.", StringComparison.OrdinalIgnoreCase))
            {
                ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efpt50.exe.zip"), toDir);
            }
            else
            {
                ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efpt30.exe.zip"), toDir);
            }

            return toDir;
        }
    }
}
