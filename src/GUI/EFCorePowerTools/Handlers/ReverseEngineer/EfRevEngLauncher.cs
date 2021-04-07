using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class EfRevEngLauncher
    {
        private readonly ReverseEngineerCommandOptions options;
        private readonly bool useEFCore5;
        private readonly string revengFolder;
        private readonly ResultDeserializer resultDeserializer;

        public static ReverseEngineerResult LaunchExternalRunner(ReverseEngineerOptions options, bool useEFCore5)
        {
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
                ProjectPath = options.ProjectPath,
                ProjectRootNamespace = options.ProjectRootNamespace,
                SelectedHandlebarsLanguage = options.SelectedHandlebarsLanguage,
                SelectedToBeGenerated = options.SelectedToBeGenerated,
                Tables = options.Tables,
                UseDatabaseNames = options.UseDatabaseNames,
                UseFluentApiOnly = options.UseFluentApiOnly,
                UseHandleBars = options.UseHandleBars,
                UseInflector = options.UseInflector,
                UseLegacyPluralizer = options.UseLegacyPluralizer,
                UseSpatial = options.UseSpatial,
                UseDbContextSplitting = options.UseDbContextSplitting,
                UseNodaTime = options.UseNodaTime,
                UseBoolPropertiesWithoutDefaultSql = options.UseBoolPropertiesWithoutDefaultSql,
                UseNullableReferences = options.UseNullableReferences,
                UseNoConstructor = options.UseNoConstructor,
                UseNoNavigations = options.UseNoNavigations,
                UseNoObjectFilter = options.UseNoObjectFilter,
                ProceduresReturnList = options.ProceduresReturnList,
            };

            var launcher = new EfRevEngLauncher(commandOptions, useEFCore5);
            return launcher.GetOutput();
        }

        public EfRevEngLauncher(ReverseEngineerCommandOptions options, bool useEFCore5)
        {
            this.options = options;
            this.useEFCore5 = useEFCore5;
            var versionSuffix = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            revengFolder = useEFCore5 ? "efreveng5." : "efreveng3.";
            revengFolder += versionSuffix;
            resultDeserializer = new ResultDeserializer();
        }

        public List<TableModel> GetDacpacTables(string dacpacPath)
        {
            var arguments = "\"" + dacpacPath + "\"";
            return GetTablesInternal(arguments);
        }

        public List<TableModel> GetTables(string connectionString, DatabaseType databaseType, SchemaInfo[] schemas)
        {
            var arguments = ((int)databaseType).ToString() + " \"" + connectionString.Replace("\"", "\\\"") + "\"";

            if (schemas != null)
            {
                arguments += $" \"{string.Join(",", schemas.Select(s => s.Name.Replace("\"", "\\\"")))}\"";
            }

            return GetTablesInternal(arguments);
        }

        private List<TableModel> GetTablesInternal(string arguments)
        {
            if (!IsDotnetInstalled())
            {
                throw new Exception($"Reverse engineer error: Unable to launch 'dotnet' version 3.1. Do you have the runtime installed? Check with 'dotnet --list-runtimes'");
            }

            var launchPath = DropNetCoreFiles();

            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(launchPath ?? throw new InvalidOperationException(), GetExeName()),
                Arguments = arguments,
            };

            var standardOutput = RunProcess(startInfo);

            return resultDeserializer.BuildTableResult(standardOutput);
        }

        private ReverseEngineerResult GetOutput()
        {
            var path = Path.GetTempFileName() + ".json";
            File.WriteAllText(path, options.Write());

            var launchPath = DropNetCoreFiles();

            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(launchPath ?? throw new InvalidOperationException(), GetExeName()),
                Arguments = "\"" + path + "\"",
            };

            var standardOutput = RunProcess(startInfo);

            try
            {
                File.Delete(path);
            }
            catch { }

            return resultDeserializer.BuildResult(standardOutput);
        }

        private string GetExeName()
        {
            return useEFCore5 ? "efreveng50.exe" : "efreveng.exe";
        }

        private bool IsDotnetInstalled()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "--list-runtimes",
            };

            var result = RunProcess(startInfo);

            if (string.IsNullOrWhiteSpace(result))
            {
                return false;
            }

            var isInstalled = false;
            var sdks = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in sdks)
            {
                isInstalled = item.StartsWith("Microsoft.NETCore.App 3.1.", StringComparison.OrdinalIgnoreCase);
                if (isInstalled)
                {
                    break;
                }
            }

            return isInstalled;
        }

        private static string RunProcess(ProcessStartInfo startInfo)
        {
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.StandardOutputEncoding = Encoding.UTF8;
            var standardOutput = new StringBuilder();

            try
            {
                using (var process = Process.Start(startInfo))
                {
                    while (process != null && !process.HasExited)
                    {
                        standardOutput.Append(process.StandardOutput.ReadToEnd());
                    }
                    if (process != null)
                    {
                        standardOutput.Append(process.StandardOutput.ReadToEnd());
                    }
                }

                return standardOutput.ToString();
            }
            catch
            {
                return null;
            }
        }

        private string DropNetCoreFiles()
        {
            var toDir = Path.Combine(Path.GetTempPath(), revengFolder);
            var fromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Debug.Assert(fromDir != null, nameof(fromDir) + " != null");
            Debug.Assert(toDir != null, nameof(toDir) + " != null");

            if (Directory.Exists(toDir)
                && Directory.EnumerateFiles(toDir, "*", SearchOption.TopDirectoryOnly).Count() > 100)
            {
                return toDir;
            }

            if (Directory.Exists(toDir))
            {
                Directory.Delete(toDir, true);
            }

            Directory.CreateDirectory(toDir);

            ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efreveng.exe.zip"), toDir);

            if (useEFCore5)
            {
                using (var archive = ZipFile.Open(Path.Combine(fromDir, "efreveng50.exe.zip"), ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(toDir, true);
                }
            }

            var dirs = Directory.GetDirectories(Path.GetTempPath(), useEFCore5 ? "efreveng5*" : "efreveng3*");

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

            return toDir;
        }
    }
}