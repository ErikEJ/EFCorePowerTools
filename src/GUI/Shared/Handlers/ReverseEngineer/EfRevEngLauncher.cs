using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        private readonly string exeName;
        private readonly string zipName;

        public EfRevEngLauncher(ReverseEngineerCommandOptions options, CodeGenerationMode codeGenerationMode)
        {
            this.options = options;
            this.codeGenerationMode = codeGenerationMode;
            var versionSuffix = FileVersionInfo.GetVersionInfo(typeof(EFCorePowerToolsPackage).Assembly.Location).FileVersion;

            string revengVersion;

            switch (codeGenerationMode)
            {
                case CodeGenerationMode.EFCore8:
                    revengVersion = "8";
                    break;

                // TODO: Use EF Core 10 when providers are available.
                case CodeGenerationMode.EFCore9:
                    revengVersion = "9";
                    break;

                default:
                    throw new NotSupportedException("Unsupported code generation mode");
            }

            revengFolder = $"efreveng{revengVersion}.";
            exeName = $"efreveng{revengVersion}0.dll";
            zipName = $"efreveng{revengVersion}0.exe.zip";

            revengRoot = revengFolder;

            revengFolder += versionSuffix;
        }

        public static async Task<ReverseEngineerResult> LaunchExternalRunnerAsync(ReverseEngineerOptions options, CodeGenerationMode codeGenerationMode)
        {
            var databaseObjects = options.Tables;
            if (!databaseObjects.Exists(t => t.ObjectType == ObjectType.Table))
            {
                // No tables selected, so add a dummy table in order to generate an empty DbContext
                databaseObjects.Add(new SerializationTableModel($"Dummy_{new Guid(GuidList.guidDbContextPackagePkgString)}", ObjectType.Table, null, null));
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
                SelectedToBeGenerated = options.SelectedToBeGenerated,
                Tables = databaseObjects,
                UseDatabaseNames = options.UseDatabaseNames,
                UseFluentApiOnly = options.UseFluentApiOnly,
                UseHandleBars = options.UseHandleBars,
                SelectedHandlebarsLanguage = options.SelectedHandlebarsLanguage,
                UseT4 = options.UseT4,
                UseT4Split = options.UseT4Split,
                T4TemplatePath = options.T4TemplatePath != null ? PathHelper.GetAbsPath(options.T4TemplatePath, options.ProjectPath) : null,
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
                UseNoNavigations = options.UseNoNavigations,
                UseNoDefaultConstructor = options.UseNoDefaultConstructor,
                UseManyToManyEntity = options.UseManyToManyEntity,
                RunCleanup = AdvancedOptions.Instance.RunCleanup,
                UseMultipleSprocResultSets = AdvancedOptions.Instance.DiscoverMultipleResultSets,
                OptionsPath = options.OptionsPath,
                MergeDacpacs = AdvancedOptions.Instance.MergeDacpacs,
                UseLegacyResultSetDiscovery = AdvancedOptions.Instance.UseLegacyResultSetDiscovery,
                UseAsyncCalls = options.UseAsyncStoredProcedureCalls,
                PreserveCasingWithRegex = options.PreserveCasingWithRegex,
                UseDateOnlyTimeOnly = options.UseDateOnlyTimeOnly,
                UseSchemaNamespaces = options.UseSchemaNamespaces,
                UseDecimalDataAnnotation = options.UseDecimalDataAnnotationForSprocResult,
                UsePrefixNavigationNaming = options.UsePrefixNavigationNaming,
                UseDatabaseNamesForRoutines = options.UseDatabaseNamesForRoutines,
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

        public async Task<string> GetDiagramAsync(string connectionString, string optionsPath, List<string> schemaList)
        {
            var option = "dgml ";

            var arguments = option + " \"" + optionsPath.Replace("\"", "\\\"") + "\" " + " \"" + connectionString.Replace("\"", "\\\"") + "\" \"" + string.Join(",", schemaList) + "\"";

            if (schemaList.Count == 0)
            {
                arguments = option + " \"" + optionsPath.Replace("\"", "\\\"") + "\" " + " \"" + connectionString.Replace("\"", "\\\"") + "\"";
            }

            var filePath = await GetDiagramInternalAsync(arguments);

            return filePath;
        }

        public async Task<string> GetErDiagramAsync(string optionsPath, string connectionString)
        {
            var arguments = "erdiagram " + " \"" + optionsPath.Replace("\"", "\\\"") + "\" " + " \"" + connectionString.Replace("\"", "\\\"") + "\" ";

            var filePath = await GetDiagramInternalAsync(arguments);

            return filePath;
        }

        public async Task<string> GetReportPathAsync(string path, bool isConnectionString)
        {
            var option = isConnectionString ? "dacpacreportextract " : "dacpacreport ";

            var arguments = option + " \"" + path.Replace("\"", "\\\"") + "\"";

            var filePath = await GetDiagramInternalAsync(arguments);

            return filePath;
        }

        public async Task<string> GetDabConfigPathAsync(string optionsPath, string connectionString)
        {
            var arguments = "dabbuilder " + " \"" + optionsPath.Replace("\"", "\\\"") + "\" " + " \"" + connectionString.Replace("\"", "\\\"") + "\" ";

            var filePath = await GetDiagramInternalAsync(arguments);

            return filePath;
        }

        private static async Task<string> RunProcessAsync(ProcessStartInfo startInfo)
        {
            return await ProcessLauncher.RunProcessAsync(startInfo);
        }

        private async Task<string> GetDiagramInternalAsync(string arguments)
        {
            var startInfo = await CreateStartInfoAsync(arguments);

            try
            {
                File.WriteAllText(Path.Combine(Path.GetTempPath(), "efrevengparams.txt"), startInfo.Arguments, Encoding.UTF8);
            }
            catch
            {
                // Ignore
            }

            var standardOutput = await RunProcessAsync(startInfo);

            return ResultDeserializer.BuildDiagramResult(standardOutput);
        }

        private async Task<List<TableModel>> GetTablesInternalAsync(string arguments)
        {
            var startInfo = await CreateStartInfoAsync(arguments);

            var standardOutput = await RunProcessAsync(startInfo);

            return ResultDeserializer.BuildTableResult(standardOutput);
        }

        private async Task<ProcessStartInfo> CreateStartInfoAsync(string arguments)
        {
            string version = "6.0";

            if (codeGenerationMode == CodeGenerationMode.EFCore8
                || codeGenerationMode == CodeGenerationMode.EFCore9)
            {
                version = "8.0";
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
            File.WriteAllText(path, options.Write(), Encoding.UTF8);

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

            return ResultDeserializer.BuildResult(standardOutput);
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

            var sdks = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return sdks.Exists(s => s.StartsWith($"Microsoft.NETCore.App {version}.", StringComparison.OrdinalIgnoreCase));
        }

        private string DropNetCoreFiles()
        {
            var toDir = Path.Combine(Path.GetTempPath(), revengFolder);
            var fromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Debug.Assert(fromDir != null, nameof(fromDir) + " != null");
            Debug.Assert(toDir != null, nameof(toDir) + " != null");

            var fullPath = Path.Combine(toDir, exeName);

            if (Directory.Exists(toDir)
                && File.Exists(fullPath)
                && Directory.EnumerateFiles(toDir, "*", SearchOption.TopDirectoryOnly).Count() >= 97)
            {
                return fullPath;
            }

            if (Directory.Exists(toDir))
            {
                Directory.Delete(toDir, true);
            }

            Directory.CreateDirectory(toDir);

            using (var archive = ZipFile.Open(Path.Combine(fromDir, zipName), ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(toDir, true);
            }

            using (var archive = ZipFile.Open(Path.Combine(fromDir, "DacFX161.zip"), ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(toDir, true);
            }

            var dirs = Directory.GetDirectories(Path.GetTempPath(), revengRoot + "*");

            foreach (var dir in dirs.Where(dir => !dir.Equals(toDir)))
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

            return fullPath;
        }
    }
}