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
            if (project.IsNetCore() && !project.IsNetCore21OrHigher())
            {
                throw new ArgumentException("Only .NET Core 2.1, 2.2 and 3.0 and 2.2 are supported");
            }
            _project = project;
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
                if (context.StartsWith("info:")) continue;

                var parts = context.Split(new[] { "DebugView:" + Environment.NewLine }, StringSplitOptions.None);
                result.Add(new Tuple<string, string>(parts[0].Trim(), parts.Length > 1 ? parts[1].Trim() : string.Empty));
            }

            return result;
        }

        private string GetOutput(string outputPath, string projectPath, GenerationType generationType, string contextName, string migrationIdentifier, string nameSpace)
        {
            var launchPath = _project.IsNetCore21OrHigher() ? DropNetCoreFiles() : DropFiles(outputPath);

            //Fix for "Bad IL format" with .NET Core 3.0 - test again after release
            if (_project.IsNetCore30() && outputPath.EndsWith(".exe"))
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

            if (_project.IsNetCore())
            {
                //TODO Consider improving by getting Startup project!
                // See EF Core .psm1 file

                var fileRoot =  Path.Combine(Path.GetDirectoryName(outputPath), Path.GetFileNameWithoutExtension(outputPath));
                var efptPath = Path.Combine(launchPath, "efpt.dll");

                var depsFile =  fileRoot + ".deps.json";
                var runtimeConfig = fileRoot + ".runtimeconfig.json";

                var projectAssetsFile = _project.GetCspProperty("ProjectAssetsFile");
                var runtimeFrameworkVersion = _project.GetCspProperty("RuntimeFrameworkVersion");

                var dotNetParams = $"exec --depsfile \"{depsFile}\" ";

                if (projectAssetsFile != null && File.Exists(projectAssetsFile.ToString()) )
                {
                    var lockFile = LockFileUtilities.GetLockFile(projectAssetsFile.ToString(), NuGet.Common.NullLogger.Instance);

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
                    || generationType == GenerationType.MigrationStatus)
                {
                    startInfo.Arguments = dotNetParams + " " + startInfo.Arguments;
                }
                else
                {
                    startInfo.Arguments = dotNetParams + " \"" + outputPath + "\"";
                }
            }

            var standardOutput = new StringBuilder();
            using (var process = System.Diagnostics.Process.Start(startInfo))
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

            var testVersion = GetEfCoreSupportedVersion(toDir);

            if (string.IsNullOrEmpty(testVersion))
            {
                throw new Exception(
                    $"Unable to find a supported version of Microsoft.EntityFrameworkCore.dll in folder {toDir}.");
            }
            File.Copy(Path.Combine(fromDir, testVersion, "efpt.exe"), Path.Combine(toDir, "efpt.exe"), true);
            File.Copy(Path.Combine(fromDir, testVersion, "efpt.exe.config"), Path.Combine(toDir, "efpt.exe.config"), true);
            File.Copy(Path.Combine(fromDir, testVersion, "Microsoft.EntityFrameworkCore.Design.dll"), Path.Combine(toDir, "Microsoft.EntityFrameworkCore.Design.dll"), true);

            return outputPath;
        }

        private string GetEfCoreSupportedVersion(string toDir)
        {
            var version = GetEfCoreVersion(toDir);

            var checkVer = version.ToString(3);

            if (checkVer == "2.1.0"
                || checkVer == "2.1.4"
                || checkVer == "2.2.0"
                || checkVer == "2.2.1"
                || checkVer == "2.2.2"
                || checkVer == "2.2.4"
                )
            {
                return version.ToString(3);
            }

            return string.Empty;
        }

        private Version GetEfCoreVersion(string folder)
        {
            var testFile = Path.Combine(folder, "Microsoft.EntityFrameworkCore.dll");

            if (File.Exists(testFile))
            {
                var fvi = FileVersionInfo.GetVersionInfo(testFile);
                return Version.Parse(fvi.FileVersion);
            }
            else
            {
                throw new Exception(
                    $"Unable to find Microsoft.EntityFrameworkCore.dll in folder {folder}.");
            }
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

            if (_project.IsNetCore21())
            {
                ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efpt21.exe.zip"), toDir);
            }
            else if (_project.IsNetCore22())
            {
                ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efpt22.exe.zip"), toDir);
            }
            else if (_project.IsNetCore30())
            {
                ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efpt30.exe.zip"), toDir);
            }
            return toDir;
        }
    }
}
