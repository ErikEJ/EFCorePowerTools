﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
using RevEng.Common.Cli;

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

        public static async Task<string> GetMsBuildSqlProjOutPutAssemblyPathAsync(this Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var assemblyName = await project.GetAttributeAsync("AssemblyName");

            var assemblyNameExe = assemblyName + ".dacpac";
            var assemblyNameDll = assemblyName + ".deps.json";

            var outputPath = await GetOutputPathAsync(project);

            if (string.IsNullOrEmpty(outputPath))
            {
                return null;
            }

            if (File.Exists(Path.Combine(outputPath, assemblyNameExe)) && File.Exists(Path.Combine(outputPath, assemblyNameDll)))
            {
                return Path.Combine(outputPath, assemblyNameExe);
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
                options.Tables?.Any(t => t.ObjectType == ObjectType.Procedure) ?? false,
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

            if (await project.IsNetStandard20Async())
            {
                version = new Version(2, 0);
            }

            if (await project.IsNetStandard21Async())
            {
                version = new Version(2, 1);
            }

            if (await project.IsNet60OrHigherAsync())
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

            // For debugging
            ////project.GetItemInfo(out IVsHierarchy h, out uint itemId, out _);
            ////Microsoft.Internal.VisualStudio.PlatformUI.HierarchyUtilities.TryGetHierarchyProperty<string>(h, itemId, (int)__VSHPROPID5.VSHPROPID_ProjectCapabilities, out string value);
            ////var capabilities = (value ?? string.Empty).Split(' ');

            // https://github.com/VsixCommunity/Community.VisualStudio.Toolkit/issues/160
            return project.IsCapabilityMatch("CSharp & !MSBuild.Sdk.SqlProj.BuildTSqlScript");
        }

        public static async Task<bool> IsLegacyAsync(this Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return await IsNetFrameworkAsync(project) || await IsNetStandard20Async(project);
        }

        public static async Task<bool> IsNet60OrHigherIncluding70Async(this Project project)
        {
            return await IsNet60Async(project) || await IsNet70Async(project);
        }

        public static async Task<bool> IsNet60OrHigherAsync(this Project project)
        {
            return await IsNet60Async(project) || await IsNet70Async(project) || await IsNet80Async(project);
        }

        public static async Task<bool> IsNet70OnlyAsync(this Project project)
        {
            return await IsNet70Async(project);
        }

        public static async Task<bool> IsNetStandardAsync(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETStandard,Version=v2.") ?? false;
        }

        public static async Task<bool> IsNetStandard21Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETStandard,Version=v2.1") ?? false;
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

        public static List<string> GenerateFiles(this Project project, List<Tuple<string, string>> result, string extension)
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
                        var package = packages.FirstOrDefault(p => p.PackageId == lib.Name);

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

        private static async Task<bool> IsNetFrameworkAsync(this Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETFramework,") ?? false;
        }

        private static async Task<bool> IsNetStandard20Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETStandard,Version=v2.0") ?? false;
        }

        private static async Task<bool> IsNet60Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETCoreApp,Version=v6.0") ?? false;
        }

        private static async Task<bool> IsNet70Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETCoreApp,Version=v7.0") ?? false;
        }

        private static async Task<bool> IsNet80Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETCoreApp,Version=v8.0") ?? false;
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
