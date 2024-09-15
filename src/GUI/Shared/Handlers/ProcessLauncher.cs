using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using Microsoft.VisualStudio.Shell;
using NuGet.ProjectModel;

namespace EFCorePowerTools.Handlers
{
    public class ProcessLauncher
    {
        private readonly Project project;

        public ProcessLauncher(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            this.project = project;
        }

        public static async Task<string> RunProcessAsync(ProcessStartInfo startInfo)
        {
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.StandardOutputEncoding = Encoding.UTF8;
            startInfo.StandardErrorEncoding = Encoding.UTF8;

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

        public Task<string> GetOutputAsync(string outputPath, GenerationType generationType, string contextNames, string connectionString)
        {
            return GetOutputInternalAsync(outputPath, generationType, contextNames, connectionString);
        }

        public Task<string> GetOutputAsync(string outputPath, GenerationType generationType, string contextName)
        {
            return GetOutputInternalAsync(outputPath, generationType, contextName, null);
        }

        public List<Tuple<string, string>> BuildModelResult(string modelInfo)
        {
            var result = new List<Tuple<string, string>>();

            var contexts = modelInfo.Split(new[] { "DbContext:" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var context in contexts)
            {
                if (context.StartsWith("info:", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (context.StartsWith("dbug:", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (context.StartsWith("warn:", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (context.IndexOf("DebugView:", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                var parts = context.Split(new[] { "DebugView:" + Environment.NewLine }, StringSplitOptions.None);
                result.Add(new Tuple<string, string>(parts[0].Trim(), parts.Length > 1 ? parts[1].Trim() : string.Empty));
            }

            return result;
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

        private static void ExtractTool(string toDir, string fromDir, string zipName, RevEng.Common.CodeGenerationMode codeGenerationMode)
        {
            ZipFile.ExtractToDirectory(Path.Combine(fromDir, zipName), toDir);
            Telemetry.TrackFrameworkUse(nameof(ProcessLauncher), codeGenerationMode);
        }

        private async Task<string> GetOutputInternalAsync(string outputPath, GenerationType generationType, string contextName, string connectionString)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var launchPath = await DropNetCoreFilesAsync();

            var startupOutputPath = await ProjectExtensions.GetStartupProjectOutputPathAsync() ?? outputPath;

            outputPath = FixExtension(outputPath);

            startupOutputPath = FixExtension(startupOutputPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(Path.GetDirectoryName(launchPath) ?? throw new InvalidOperationException(), "efpt.exe"),
                Arguments = "\"" + outputPath + "\"",
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
                case GenerationType.DbContextList:
                    startInfo.Arguments = "contextlist" + outputs;
                    break;
                case GenerationType.DbContextCompare:
                    startInfo.Arguments = "schemacompare" + outputs + "\"" + connectionString + "\" " + contextName;
                    break;
                default:
                    break;
            }

            var fileRoot = Path.Combine(Path.GetDirectoryName(outputPath), Path.GetFileNameWithoutExtension(outputPath));
            var efptPath = Path.Combine(launchPath, "efpt.dll");

            var depsFile = fileRoot + ".deps.json";
            var runtimeConfig = fileRoot + ".runtimeconfig.json";

            var projectAssetsFile = await project.GetAttributeAsync("ProjectAssetsFile");
            var runtimeFrameworkVersion = await project.GetAttributeAsync("RuntimeFrameworkVersion");

            var dotNetParams = new StringBuilder();

            dotNetParams.Append($"exec --depsfile \"{depsFile}\" ");

            if (projectAssetsFile != null && File.Exists(projectAssetsFile))
            {
                var lockFile = LockFileUtilities.GetLockFile(projectAssetsFile, NuGet.Common.NullLogger.Instance);

                if (lockFile != null)
                {
                    foreach (var packageFolder in lockFile.PackageFolders)
                    {
                        var path = packageFolder.Path.TrimEnd('\\');
                        dotNetParams.Append($"--additionalprobingpath \"{path}\" ");
                    }
                }
            }

            if (File.Exists(runtimeConfig))
            {
                dotNetParams.Append($"--runtimeconfig \"{runtimeConfig}\" ");
            }
            else if (!string.IsNullOrEmpty(runtimeFrameworkVersion))
            {
                dotNetParams.Append($"--fx-version {runtimeFrameworkVersion} ");
            }

            dotNetParams.Append($"\"{efptPath}\" ");

            startInfo.WorkingDirectory = Path.GetDirectoryName(outputPath);
            startInfo.FileName = "dotnet";
            startInfo.Arguments = dotNetParams.ToString() + " " + startInfo.Arguments;

            try
            {
                File.WriteAllText(Path.Combine(Path.GetTempPath(), "efptparams.txt"), startInfo.Arguments, Encoding.UTF8);
            }
            catch
            {
                // Ignore
            }

            return await RunProcessAsync(startInfo);
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

            var versionInfo = await project.ContainsEfCoreDesignReferenceAsync();

            var isNet7 = await project.IsNet70OnlyAsync();

            if (versionInfo.Item2.StartsWith("6.", StringComparison.OrdinalIgnoreCase))
            {
                ExtractTool(toDir, fromDir, "efpt60.exe.zip", RevEng.Common.CodeGenerationMode.EFCore6);
            }
            else if (versionInfo.Item2.StartsWith("8.", StringComparison.OrdinalIgnoreCase))
            {
                ExtractTool(toDir, fromDir, "efpt80.exe.zip", RevEng.Common.CodeGenerationMode.EFCore8);
            }
            else if (!isNet7 && versionInfo.Item2.StartsWith("7.", StringComparison.OrdinalIgnoreCase))
            {
                ExtractTool(toDir, fromDir, "efpt70.exe.zip", RevEng.Common.CodeGenerationMode.EFCore7);
            }
            else if (isNet7 && versionInfo.Item2.StartsWith("7.", StringComparison.OrdinalIgnoreCase))
            {
                ExtractTool(toDir, fromDir, "efpt70sc.exe.zip", RevEng.Common.CodeGenerationMode.EFCore7);
            }
            else if (versionInfo.Item2.StartsWith("9.", StringComparison.OrdinalIgnoreCase))
            {
                ExtractTool(toDir, fromDir, "efpt90.exe.zip", RevEng.Common.CodeGenerationMode.EFCore9);
            }

            return toDir;
        }
    }
}
