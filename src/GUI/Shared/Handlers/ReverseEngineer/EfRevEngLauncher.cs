﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Extensions;
using RevEng.Common;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class EfRevEngLauncher
    {
        private readonly ReverseEngineerCommandOptions options;
        private readonly CodeGenerationMode codeGenerationMode;
        private readonly string revengFolder;
        private readonly string revengRoot;
        private readonly ResultDeserializer resultDeserializer;

        public EfRevEngLauncher(ReverseEngineerCommandOptions options, CodeGenerationMode codeGenerationMode)
        {
            this.options = options;
            this.codeGenerationMode = codeGenerationMode;
            var versionSuffix = FileVersionInfo.GetVersionInfo(typeof(EFCorePowerToolsPackage).Assembly.Location).FileVersion;

            switch (codeGenerationMode)
            {
                case CodeGenerationMode.EFCore6:
                    revengFolder = "efreveng6.";
                    break;
                case CodeGenerationMode.EFCore7:
                    revengFolder = "efreveng7.";
                    break;
                default:
                    throw new NotSupportedException();
            }

            revengRoot = revengFolder;

            revengFolder += versionSuffix;
            resultDeserializer = new ResultDeserializer();
        }

        public static async Task<ReverseEngineerResult> LaunchExternalRunnerAsync(ReverseEngineerOptions options, CodeGenerationMode codeGenerationMode, Project project)
        {
            var databaseObjects = options.Tables;
            if (!databaseObjects.Any(t => t.ObjectType == ObjectType.Table))
            {
                // No tables selected, so add a dummy table in order to generate an empty DbContext
                databaseObjects.Add(new SerializationTableModel($"Dummy_{new Guid(GuidList.guidDbContextPackagePkgString)}", ObjectType.Table, null));
            }

            var commandOptions = new ReverseEngineerCommandOptions
            {
                ConnectionString = options.ConnectionString,
                ContextClassName = options.ContextClassName,
                CustomReplacers = options.CustomReplacers,
                Dacpac = options.Dacpac,
                DatabaseType = options.DatabaseType,
                DefaultDacpacSchema = options.DefaultDacpacSchema,
                IncludeConnectionString = options.IncludeConnectionString,
                OutputPath = options.OutputPath,
                ContextNamespace = options.ContextNamespace,
                ModelNamespace = options.ModelNamespace,
                OutputContextPath = options.OutputContextPath,
                UseSchemaFolders = options.UseSchemaFolders,
                ProjectPath = options.ProjectPath,
                ProjectRootNamespace = options.ProjectRootNamespace,
                SelectedHandlebarsLanguage = options.SelectedHandlebarsLanguage,
                SelectedToBeGenerated = options.SelectedToBeGenerated,
                Tables = databaseObjects,
                UseDatabaseNames = options.UseDatabaseNames,
                UseFluentApiOnly = options.UseFluentApiOnly,
                UseHandleBars = options.UseHandleBars,
                UseT4 = options.UseT4,
                UseInflector = options.UseInflector,
                UseLegacyPluralizer = options.UseLegacyPluralizer,
                UncountableWords = options.UncountableWords,
                UseSpatial = options.UseSpatial,
                UseHierarchyId = options.UseHierarchyId,
                UseDbContextSplitting = options.UseDbContextSplitting,
                UseNodaTime = options.UseNodaTime,
                UseBoolPropertiesWithoutDefaultSql = options.UseBoolPropertiesWithoutDefaultSql,
                UseNullableReferences = options.UseNullableReferences,
                UseNoObjectFilter = options.UseNoObjectFilter,
                UseNoDefaultConstructor = options.UseNoDefaultConstructor,
                UseManyToManyEntity = options.UseManyToManyEntity,
                RunCleanup = AdvancedOptions.Instance.RunCleanup,
                UseMultipleSprocResultSets = AdvancedOptions.Instance.DiscoverMultipleResultSets,
                OptionsPath = options.OptionsPath,
                MergeDacpacs = AdvancedOptions.Instance.MergeDacpacs,
                UseLegacyResultSetDiscovery = AdvancedOptions.Instance.UseLegacyResultSetDiscovery,
                UseAsyncCalls = AdvancedOptions.Instance.PreferAsyncCalls,
                PreserveCasingWithRegex = options.PreserveCasingWithRegex,
                UseDateOnlyTimeOnly = options.UseDateOnlyTimeOnly,
                UseSchemaNamespaces = options.UseSchemaNamespaces,
            };

            var launcher = new EfRevEngLauncher(commandOptions, codeGenerationMode);
            return await launcher.GetOutputAsync();
        }

        public async Task<List<TableModel>> GetTablesAsync(string connectionString, DatabaseType databaseType, SchemaInfo[] schemas, bool mergeDacpacs)
        {
            var arguments = mergeDacpacs.ToString() + " " + ((int)databaseType).ToString() + " \"" + connectionString.Replace("\"", "\\\"") + "\"";

            if (schemas != null)
            {
                arguments += $" \"{string.Join(",", schemas.Select(s => s.Name.Replace("\"", "\\\"")))}\"";
            }

            return await GetTablesInternalAsync(arguments);
        }

        public async Task<string> GetDgmlAsync(string connectionString, DatabaseType databaseType)
        {
            var arguments = "dgml " + ((int)databaseType).ToString() + " \"" + connectionString.Replace("\"", "\\\"") + "\"";

            var filePath = await GetDgmlInternalAsync(arguments);

            return filePath;
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

        private async Task<string> GetDgmlInternalAsync(string arguments)
        {
            var startInfo = await CreateStartInfoAsync(arguments);

            var standardOutput = await RunProcessAsync(startInfo);

            return resultDeserializer.BuildDgmlResult(standardOutput);
        }

        private async Task<List<TableModel>> GetTablesInternalAsync(string arguments)
        {
            var startInfo = await CreateStartInfoAsync(arguments);

            var standardOutput = await RunProcessAsync(startInfo);

            return resultDeserializer.BuildTableResult(standardOutput);
        }

        private async Task<ProcessStartInfo> CreateStartInfoAsync(string arguments)
        {
            string version = "3.1";

            if (codeGenerationMode == CodeGenerationMode.EFCore6 || codeGenerationMode == CodeGenerationMode.EFCore7)
            {
                version = "6.0";
            }

            if (!await IsDotnetInstalledAsync(version))
            {
                throw new InvalidOperationException($"Reverse engineer error: Unable to launch 'dotnet' version {version}. Do you have the runtime installed? Check with 'dotnet --list-runtimes'");
            }

            var launchPath = DropNetCoreFiles();

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"\"{launchPath}\" {arguments}",
            };
            return startInfo;
        }

        private async Task<ReverseEngineerResult> GetOutputAsync()
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) + ".json";
            File.WriteAllText(path, options.Write());

            var launchPath = DropNetCoreFiles();

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"\"{launchPath}\" \"{path}\"",
            };

            var standardOutput = await RunProcessAsync(startInfo);

            try
            {
                File.Delete(path);
            }
            catch
            {
                // Ignore
            }

            return resultDeserializer.BuildResult(standardOutput);
        }

        private async Task<bool> IsDotnetInstalledAsync(string version)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "--list-runtimes",
            };

            var result = await RunProcessAsync(startInfo);

            if (string.IsNullOrWhiteSpace(result))
            {
                return false;
            }

            var sdks = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            return sdks.Any(s => s.StartsWith($"Microsoft.NETCore.App {version}.", StringComparison.OrdinalIgnoreCase));
        }

        private string DropNetCoreFiles()
        {
            var toDir = Path.Combine(Path.GetTempPath(), revengFolder);
            var fromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Debug.Assert(fromDir != null, nameof(fromDir) + " != null");
            Debug.Assert(toDir != null, nameof(toDir) + " != null");

            var fullPath = Path.Combine(toDir, GetExeName());

            if (Directory.Exists(toDir)
                && File.Exists(fullPath)
                && Directory.EnumerateFiles(toDir, "*", SearchOption.TopDirectoryOnly).Count() >= 106)
            {
                return fullPath;
            }

            if (Directory.Exists(toDir))
            {
                Directory.Delete(toDir, true);
            }

            Directory.CreateDirectory(toDir);
            var cpuArch = RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "arm" : string.Empty;

            string zipName;

            switch (codeGenerationMode)
            {
                case CodeGenerationMode.EFCore6:
                    zipName = $"efreveng60{cpuArch}.exe.zip";
                    break;
                case CodeGenerationMode.EFCore7:
                    zipName = $"efreveng70{cpuArch}.exe.zip";
                    break;
                default:
                    throw new NotSupportedException();
            }

            using (var archive = ZipFile.Open(Path.Combine(fromDir, zipName), ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(toDir, true);
            }

            using (var archive = ZipFile.Open(Path.Combine(fromDir, "DacFX161.zip"), ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(toDir, true);
            }

            var dirs = Directory.GetDirectories(Path.GetTempPath(), revengRoot + "*");

            foreach (var dir in dirs)
            {
                if (!dir.Equals(toDir))
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch
                    {
                        // Ignore
                    }
                }
            }

            return fullPath;
        }

        private string GetExeName()
        {
            switch (codeGenerationMode)
            {
                case CodeGenerationMode.EFCore6:
                    return "efreveng60.dll";
                case CodeGenerationMode.EFCore7:
                    return "efreveng70.dll";
                default:
                    throw new NotSupportedException("Unsupported code generation mode");
            }
        }
    }
}
