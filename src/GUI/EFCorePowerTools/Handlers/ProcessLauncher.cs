﻿using EFCorePowerTools.Extensions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
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

        public Task<string> GetOutputAsync(string outputPath, GenerationType generationType, string contextNames, string connectionString)
        {
            return GetOutputInternalAsync(outputPath, null, generationType, contextNames, connectionString, null);
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
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var launchPath = await DropNetCoreFilesAsync();

            var startupOutputPath = _project.DTE.GetStartupProjectOutputPath() ?? outputPath;

            outputPath = FixExtension(outputPath);

            startupOutputPath = FixExtension(startupOutputPath);

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

            var outputs = " \"" + outputPath + "\" \"" + startupOutputPath + "\" ";

            startInfo.Arguments = outputs;

            switch (generationType)
            {
                case GenerationType.Dgml:
                    break;
                case GenerationType.Ddl:
                    startInfo.Arguments = "ddl" + outputs;
                    break;
                case GenerationType.DebugView:
                    break;
                case GenerationType.MigrationStatus:
                    startInfo.Arguments = "migrationstatus" + outputs;
                    break;
                case GenerationType.MigrationApply:
                    startInfo.Arguments = "migrate" + outputs + contextName;
                    break;
                case GenerationType.MigrationAdd:
                    startInfo.Arguments = "addmigration" + outputs + "\"" + projectPath + "\" " + contextName + " " + migrationIdentifier + " " + nameSpace;
                    break;
                case GenerationType.MigrationScript:
                    startInfo.Arguments = "scriptmigration" + outputs + contextName;
                    break;
                case GenerationType.DbContextList:
                    startInfo.Arguments = "contextlist" + outputs;
                    break;
                case GenerationType.DbContextCompare:
                    startInfo.Arguments = "schemacompare" + outputs + "\"" + migrationIdentifier + "\" " + contextName;
                    break;
                default:
                    break;
            }

            var fileRoot = Path.Combine(Path.GetDirectoryName(outputPath), Path.GetFileNameWithoutExtension(outputPath));
            var efptPath = Path.Combine(launchPath, "efpt.dll");

            var depsFile = fileRoot + ".deps.json";
            var runtimeConfig = fileRoot + ".runtimeconfig.json";

            var projectAssetsFile = await _project.GetCspPropertyAsync("ProjectAssetsFile");
            var runtimeFrameworkVersion = await _project.GetCspPropertyAsync("RuntimeFrameworkVersion");

            var dotNetParams = $"exec --depsfile \"{depsFile}\" ";

            if (projectAssetsFile != null && File.Exists(projectAssetsFile))
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
            startInfo.Arguments = dotNetParams + " " + startInfo.Arguments;

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

        private static string FixExtension(string startupOutputPath)
        {
            if (startupOutputPath.EndsWith(".exe"))
            {
                startupOutputPath = startupOutputPath.Remove(startupOutputPath.Length - 4, 4);
                startupOutputPath += ".dll";
            }

            return startupOutputPath;
        }

        private async Task<string> DropNetCoreFilesAsync()
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
