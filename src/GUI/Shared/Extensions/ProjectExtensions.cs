using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.ProjectModel;
using RevEng.Common;

namespace EFCorePowerTools.Extensions
{
    internal static class ProjectExtensions
    {
        public const int SOk = 0;

        public static async Task<string> GetStartupProjectOutputPathAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IVsSolutionBuildManager buildManager;
            IVsHierarchy startupProject;
            Project project;

            try
            {
                buildManager = await VS.Services.GetSolutionBuildManagerAsync();

                ErrorHandler.ThrowOnFailure(buildManager.get_StartupProject(out startupProject));

                project = (Project)await SolutionItem.FromHierarchyAsync(startupProject,
                    (uint)VSConstants.VSITEMID.Root);

                return await project.GetOutPutAssemblyPathAsync();
            }
            catch
            {
                return null;
            }
        }

        public static async Task<string> GetOutPutAssemblyPathAsync(this Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var assemblyName = await project.GetAttributeAsync("AssemblyName");

            var assemblyNameExe = assemblyName + ".exe";
            var assemblyNameDll = assemblyName + ".dll";

            var outputPath = await GetOutputPathAsync(project);

            if (string.IsNullOrEmpty(outputPath))
            {
                return null;
            }

            if (File.Exists(Path.Combine(outputPath, assemblyNameExe)))
            {
                return Path.Combine(outputPath, assemblyNameExe);
            }

            if (File.Exists(Path.Combine(outputPath, assemblyNameDll)))
            {
                return Path.Combine(outputPath, assemblyNameDll);
            }

            return null;
        }

        public static List<string> GetConfigFiles(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = new List<string>();

            var projectPath = Path.GetDirectoryName(project.FullPath);

            if (string.IsNullOrEmpty(projectPath))
            {
                return result;
            }

            var files = Directory.GetFiles(projectPath, "efpt.*config.json", SearchOption.AllDirectories);
            result.AddRange(files
                .Where(f => !f.Contains("\\bin\\") && !f.Contains("\\obj\\")));

            if (result.Count == 0)
            {
                result.Add(Path.Combine(projectPath, "efpt.config.json"));
            }

            return result.OrderBy(s => s).ToList();
        }

        public static string GetRenamingPath(this Project project, string optionsPath, bool navigationsFile = false)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string renamingPath;

            if (string.IsNullOrEmpty(optionsPath))
            {
                renamingPath = project.FullPath;

                if (string.IsNullOrEmpty(renamingPath))
                {
                    return null;
                }
            }
            else
            {
                renamingPath = Path.GetDirectoryName(optionsPath);
            }

            return Path.Combine(renamingPath, navigationsFile ? RuleFiles.PropertyFilename : RuleFiles.RenamingFilename);
        }

        public static async Task<Tuple<bool, string>> ContainsEfCoreReferenceAsync(this Project project,
            DatabaseType dbType)
        {
            var providerPackage = "Microsoft.EntityFrameworkCore.SqlServer";
            if (dbType == DatabaseType.SQLite)
            {
                providerPackage = "Microsoft.EntityFrameworkCore.Sqlite";
            }

            if (dbType == DatabaseType.Npgsql)
            {
                providerPackage = "Npgsql.EntityFrameworkCore.PostgreSQL";
            }

            if (dbType == DatabaseType.Mysql)
            {
                providerPackage = "Pomelo.EntityFrameworkCore.MySql";
            }

            if (dbType == DatabaseType.Oracle)
            {
                providerPackage = "Oracle.EntityFrameworkCore";
            }

            if (dbType == DatabaseType.Firebird)
            {
                providerPackage = "FirebirdSql.EntityFrameworkCore.Firebird";
            }

            if ((await ContainsReferenceAsync(project, providerPackage)).Item1)
            {
                return new Tuple<bool, string>(true, providerPackage);
            }

            return new Tuple<bool, string>(false, providerPackage);
        }

        public static async Task<Tuple<bool, string>> ContainsEfSchemaCompareReferenceAsync(this Project project)
        {
            return await ContainsReferenceAsync(project, "EfCore.SchemaCompare");
        }

        public static async Task<Tuple<bool, string>> ContainsEfCoreDesignReferenceAsync(this Project project)
        {
            return await ContainsReferenceAsync(project, "Microsoft.EntityFrameworkCore.Design");
        }

        public static async Task<Version> GetEFCoreVersionHintAsync(this Project project)
        {
            var version = new Version(3, 0);

            if (await project.IsNetStandard20Async())
            {
                version = new Version(2, 0);
            }

            if (await project.IsNetStandard21Async())
            {
                version = new Version(2, 1);
            }

            if (await project.IsNet60Async())
            {
                version = new Version(6, 0);
            }

            if (await project.IsNet70Async())
            {
                version = new Version(6, 0);
            }

            return version;
        }

        public static bool IsCSharpProject(this Project project)
        {
            if (project == null)
            {
                return false;
            }

            return project.IsCapabilityMatch("CSharp");
        }

        public static async Task<bool> IsLegacyAsync(this Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return await IsNetFrameworkAsync(project) || await IsNetStandard20Async(project);
        }

        public static async Task<bool> IsNetCore31OrHigherIncluding70Async(this Project project)
        {
            return await IsNetCore31Async(project) || await IsNet60Async(project) || await IsNet70Async(project);
        }

        public static async Task<bool> IsNetCore31OrHigherAsync(this Project project)
        {
            return await IsNetCore31Async(project) || await IsNet60Async(project);
        }

        public static async Task<bool> IsNetStandard21Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETStandard,Version=v2.1") ??
                   false;
        }

        public static List<string> GenerateFiles(this Project project, List<Tuple<string, string>> result,
            string extension)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var list = new List<string>();

            foreach (var item in result)
            {
                if (item.Item1.IndexOfAny(Path.GetInvalidPathChars()) >= 0
                    || item.Item1.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    VSHelper.ShowError($"{SharedLocale.InvalidName}: {item.Item1}");
                    return list;
                }

                var filePath = Path.Combine(
                    Path.GetTempPath(),
                    item.Item1 + extension);

                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                }

                File.WriteAllText(filePath, item.Item2);
                File.SetAttributes(filePath, FileAttributes.ReadOnly);

                list.Add(filePath);
            }

            return list;
        }

        private static async Task<Tuple<bool, string>> ContainsReferenceAsync(Project project, string designPackage)
        {
            var corePackage = "Microsoft.EntityFrameworkCore";

            bool hasDesign = false;
            string coreVersion = string.Empty;
            var projectAssetsFile = await project.GetAttributeAsync("ProjectAssetsFile");

            if (projectAssetsFile != null && File.Exists(projectAssetsFile))
            {
                var lockFile = LockFileUtilities.GetLockFile(projectAssetsFile, NuGet.Common.NullLogger.Instance);

                if (lockFile != null)
                {
                    foreach (var lib in lockFile.Libraries)
                    {
                        if (lib.Name.Equals(corePackage))
                        {
                            coreVersion = lib.Version.ToString();
                        }

                        if (lib.Name.Equals(designPackage))
                        {
                            hasDesign = true;
                        }
                    }
                }
            }

            return new Tuple<bool, string>(hasDesign, coreVersion);
        }

        private static async Task<bool> IsNetFrameworkAsync(this Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETFramework,") ?? false;
        }

        private static async Task<bool> IsNetStandard20Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETStandard,Version=v2.0") ??
                   false;
        }

        private static async Task<bool> IsNetCore31Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETCoreApp,Version=v3.1") ??
                   false;
        }

        private static async Task<bool> IsNet60Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETCoreApp,Version=v6.0") ??
                   false;
        }

        private static async Task<bool> IsNet70Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETCoreApp,Version=v7.0") ??
                   false;
        }

        private static async Task<string> GetOutputPathAsync(Project project)
        {
            var outputPath = await project.GetAttributeAsync("OutputPath");
            var fullName = project.FullPath;

            var absoluteOutputPath = RevEng.Common.PathHelper.GetAbsPath(outputPath, fullName);

            return absoluteOutputPath;
        }
    }
}
