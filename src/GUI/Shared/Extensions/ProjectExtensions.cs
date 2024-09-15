using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Handlers.ReverseEngineer;
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

                project = (Project)await SolutionItem.FromHierarchyAsync(startupProject, (uint)VSConstants.VSITEMID.Root);

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
            var assemblyNameDacpac = assemblyName + ".dacpac";

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

            if (File.Exists(Path.Combine(outputPath, assemblyNameDacpac)))
            {
                return Path.Combine(outputPath, assemblyNameDacpac);
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
                .Where(f => (f.IndexOf("\\bin\\", StringComparison.OrdinalIgnoreCase) < 0) && (f.IndexOf("\\obj\\", StringComparison.OrdinalIgnoreCase) < 0)));

#pragma warning disable S2583 // Conditionally executed code should be reachable
            if (result.Count == 0)
            {
                result.Add(Path.Combine(projectPath, "efpt.config.json"));
            }
#pragma warning restore S2583 // Conditionally executed code should be reachable

            return result.OrderBy(s => s).ToList();
        }

        public static string GetCliConfigFile(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var projectPath = Path.GetDirectoryName(project.FullPath);

            if (string.IsNullOrEmpty(projectPath))
            {
                return null;
            }

            var path = Path.Combine(projectPath,  RevEng.Common.Constants.ConfigFileName);

            if (File.Exists(path))
            {
                return path;
            }

            return null;
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

            const string efptRenamingJson = "efpt.renaming.json";
            const string efptRenamingNavJson = "efpt.property-renaming.json";
            return Path.Combine(renamingPath, navigationsFile ? efptRenamingNavJson : efptRenamingJson);
        }

        public static async Task<List<NuGetPackage>> GetNeededPackagesAsync(this Project project, ReverseEngineerOptions options)
        {
            var neededPackages = Providers.GetNeededPackages(
                options.DatabaseType,
                options.UseSpatial,
                options.UseNodaTime,
                options.UseDateOnlyTimeOnly,
                options.UseHierarchyId,
                AdvancedOptions.Instance.DiscoverMultipleResultSets,
                options.Tables?.Exists(t => t.ObjectType == ObjectType.Procedure) ?? false,
                options.CodeGenerationMode);

            await IsInstalledAsync(project, neededPackages);

            return neededPackages;
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

            if (await project.IsNetStandardAsync())
            {
                version = new Version(2, 0);
            }

            if (await project.IsNet60OrHigherAsync())
            {
                version = new Version(6, 0);
            }

            if (await project.IsNet80Async())
            {
                version = new Version(8, 0);
            }

            if (await project.IsNet90Async())
            {
                version = new Version(9, 0);
            }

            return version;
        }

        public static async Task<bool> CanUseReverseEngineerAsync(this Project project)
        {
            return project.IsCSharpProject()
                && (await project.IsNet60OrHigherAsync() || await project.IsNetStandardAsync());
        }

        public static bool IsCSharpProject(this Project project)
        {
            if (project == null)
            {
                return false;
            }

            // For debugging
            ////project.GetItemInfo(out IVsHierarchy h, out uint itemId, out _);
            ////Microsoft.Internal.VisualStudio.PlatformUI.HierarchyUtilities.TryGetHierarchyProperty<string>(h, itemId, (int)__VSHPROPID5.VSHPROPID_ProjectCapabilities, out string value);
            ////var capabilities = (value ?? string.Empty).Split(' ');

            return project.IsCapabilityMatch("CSharp & CPS & !MSBuild.Sdk.SqlProj.BuildTSqlScript");
        }

        public static bool IsCSharpProjectPlain(this Project project)
        {
            if (project == null)
            {
                return false;
            }

            return project.IsCapabilityMatch("CSharp & CPS");
        }

        public static bool IsSqlDatabaseProject(this Project project)
        {
            if (project == null)
            {
                return false;
            }

            return project.IsMsBuildSqlProjProject()
                || project.FullPath.EndsWith(".sqlproj", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsMsBuildSqlProjProject(this Project project)
        {
            if (project == null)
            {
                return false;
            }

            return project.IsCapabilityMatch("CSharp & CPS & MSBuild.Sdk.SqlProj.BuildTSqlScript");
        }

        public static async Task<bool> IsNet60OrHigherAsync(this Project project)
        {
            var targetFrameworkMonikers = await GetTargetFrameworkMonikersAsync(project);

            return IsNet60(targetFrameworkMonikers)
                || IsNet70(targetFrameworkMonikers)
                || IsNet80(targetFrameworkMonikers)
                || IsNet90(targetFrameworkMonikers);
        }

        public static async Task<bool> IsNet70OnlyAsync(this Project project)
        {
            var targetFrameworkMonikers = await GetTargetFrameworkMonikersAsync(project);

            return IsNet70(targetFrameworkMonikers);
        }

        public static async Task<bool> IsNetStandardAsync(this Project project)
        {
            var targetFrameworkMonikers = await GetTargetFrameworkMonikersAsync(project);

            if (targetFrameworkMonikers == null)
            {
                return false;
            }

            return targetFrameworkMonikers.IndexOf(".NETStandard,Version=v2.", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static async Task<bool> IsInstalledAsync(this Project project, NuGetPackage package)
        {
            var projectAssetsFile = await project.GetAttributeAsync("ProjectAssetsFile");

            if (projectAssetsFile != null && File.Exists(projectAssetsFile))
            {
                var lockFile = LockFileUtilities.GetLockFile(projectAssetsFile, NuGet.Common.NullLogger.Instance);

                if (lockFile != null)
                {
                    foreach (var lib in lockFile.Libraries)
                    {
                        if (lib.Name == package.PackageId)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static List<string> GenerateFiles(this Project project, List<Tuple<string, string>> result, string extension, bool addToProject = false)
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

                if (addToProject)
                {
                    var itemPath = Path.Combine(Path.GetDirectoryName(project.FullPath), item.Item1 + extension);
                    File.WriteAllText(itemPath, item.Item2, Encoding.UTF8);
                    list.Add(itemPath);
                }
                else
                {
                    var filePath = Path.Combine(
                        Path.GetTempPath(),
                        item.Item1 + extension);

                    if (File.Exists(filePath))
                    {
                        File.SetAttributes(filePath, FileAttributes.Normal);
                    }

                    File.WriteAllText(filePath, item.Item2, Encoding.UTF8);
                    File.SetAttributes(filePath, FileAttributes.ReadOnly);

                    list.Add(filePath);
                }
            }

            return list;
        }

        private static async Task<string> GetTargetFrameworkMonikersAsync(Project project)
        {
            string result = null;

            project.GetItemInfo(out IVsHierarchy hierarchy, out uint itemId, out _);

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                var vsProject = (IVsProject)hierarchy;

                if (vsProject != null)
                {
                    var props = vsProject.ToProject().Properties;
                    result = props.Item("TargetFrameworkMonikers")?.Value?.ToString();
                }
            }
            catch
            {
                // Ignore
            }

            if (string.IsNullOrEmpty(result))
            {
                result = await project.GetAttributeAsync("TargetFrameworkMoniker");
            }

            return result;
        }

        private static EnvDTE.Project ToProject(this IVsProject vsProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var vsHierarchy = (IVsHierarchy)vsProject;
            var hr = vsHierarchy.GetProperty(
                (uint)VSConstants.VSITEMID.Root,
                (int)__VSHPROPID.VSHPROPID_ExtObject,
                out var project);
            Marshal.ThrowExceptionForHR(hr);

            return (EnvDTE.Project)project;
        }

        private static async Task IsInstalledAsync(Project project, List<NuGetPackage> packages)
        {
            var projectAssetsFile = await project.GetAttributeAsync("ProjectAssetsFile");

            if (projectAssetsFile != null && File.Exists(projectAssetsFile))
            {
                var lockFile = LockFileUtilities.GetLockFile(projectAssetsFile, NuGet.Common.NullLogger.Instance);

                if (lockFile != null)
                {
                    foreach (var lib in lockFile.Libraries)
                    {
                        var package = packages.Find(p => p.PackageId == lib.Name);

                        if (package != null)
                        {
                            package.Installed = true;
                        }
                    }
                }
            }
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

        private static async Task<bool> IsNet80Async(this Project project)
        {
            var targetFrameworkMonikers = await GetTargetFrameworkMonikersAsync(project);

            return IsNet80(targetFrameworkMonikers);
        }

        private static async Task<bool> IsNet90Async(this Project project)
        {
            var targetFrameworkMonikers = await GetTargetFrameworkMonikersAsync(project);

            return IsNet90(targetFrameworkMonikers);
        }

        private static bool IsNet60(string targetFrameworkMonikers)
        {
            return FrameworkCheck(targetFrameworkMonikers, "6");
        }

        private static bool IsNet70(string targetFrameworkMonikers)
        {
            return FrameworkCheck(targetFrameworkMonikers, "7");
        }

        private static bool IsNet80(string targetFrameworkMonikers)
        {
            return FrameworkCheck(targetFrameworkMonikers, "8");
        }

        private static bool IsNet90(string targetFrameworkMonikers)
        {
            return FrameworkCheck(targetFrameworkMonikers, "9");
        }

        private static bool FrameworkCheck(string targetFrameworkMonikers, string version)
        {
            if (string.IsNullOrEmpty(targetFrameworkMonikers))
            {
                return false;
            }

            return targetFrameworkMonikers.IndexOf($".NETCoreApp,Version=v{version}.0", StringComparison.OrdinalIgnoreCase) >= 0;
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
