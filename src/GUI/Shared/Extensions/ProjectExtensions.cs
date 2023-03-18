using System;
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

        public static async Task<string> GetOutPutAssemblyPathAsync(this Project project, bool lookForDacpac = false)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var assemblyName = await project.GetAttributeAsync("AssemblyName");

            var assemblyNameExe = assemblyName + ".exe";
            var assemblyNameDll = assemblyName + ".dll";

            if (lookForDacpac)
            {
                assemblyNameExe = assemblyName + ".dacpac";
            }

            var outputPath = await GetOutputPathAsync(project);

            if (string.IsNullOrEmpty(outputPath))
            {
                return null;
            }

            if (File.Exists(Path.Combine(outputPath, assemblyNameExe)))
            {
                return Path.Combine(outputPath, assemblyNameExe);
            }

            if (!lookForDacpac && File.Exists(Path.Combine(outputPath, assemblyNameDll)))
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

            const string efptRenamingJson = "efpt.renaming.json";
            const string efptRenamingNavJson = "efpt.property-renaming.json";
            return Path.Combine(renamingPath, navigationsFile ? efptRenamingNavJson : efptRenamingJson);
        }

        public static async Task<List<NuGetPackage>> GetNeededPackagesAsync(this Project project, ReverseEngineerOptions options)
        {
            var neededPackages = GetNeededPackages(options.DatabaseType, options);

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

            // For debugging
            ////project.GetItemInfo(out IVsHierarchy h, out uint itemId, out _);
            ////Microsoft.Internal.VisualStudio.PlatformUI.HierarchyUtilities.TryGetHierarchyProperty<string>(h, itemId, (int)__VSHPROPID5.VSHPROPID_ProjectCapabilities, out string value);
            ////var capabilities = (value ?? string.Empty).Split(' ');

            // https://github.com/VsixCommunity/Community.VisualStudio.Toolkit/issues/160
            return project.IsCapabilityMatch("CSharp & !TestContainer & !MSBuild.Sdk.SqlProj.BuildTSqlScript");
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

        public static async Task<bool> IsNet60OrHigherAsync(this Project project)
        {
            return await IsNet60Async(project) || await IsNet70Async(project);
        }

        public static async Task<bool> IsNetCore31OrHigherAsync(this Project project)
        {
            return await IsNetCore31Async(project) || await IsNet60Async(project);
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

        private static List<NuGetPackage> GetNeededPackages(DatabaseType databaseType, ReverseEngineerOptions options)
        {
            // TODO Update versions here when adding provider updates
            var packages = new List<NuGetPackage>();

            if (databaseType == DatabaseType.SQLServer || databaseType == DatabaseType.SQLServerDacpac)
            {
                var pkgVersion = "7.0.4";
                switch (options.CodeGenerationMode)
                {
                    case CodeGenerationMode.EFCore3:
                        pkgVersion = "3.1.32";
                        break;
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.15";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Microsoft.EntityFrameworkCore.SqlServer",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                    IsMainProviderPackage = true,
                    UseMethodName = "SqlServer",
                });

                if (options.UseSpatial)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                        IsMainProviderPackage = false,
                        UseMethodName = "NetTopologySuite",
                    });
                }

                if (options.UseNodaTime)
                {
                    pkgVersion = "7.0.0";
                    switch (options.CodeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore3:
                            pkgVersion = "3.1.2";
                            break;
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "6.0.1";
                            break;
                    }

                    packages.Add(new NuGetPackage
                    {
                        PackageId = "SimplerSoftware.EntityFrameworkCore.SqlServer.NodaTime",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                        IsMainProviderPackage = false,
                        UseMethodName = "NodaTime",
                    });
                }

                if (options.UseHierarchyId)
                {
                    pkgVersion = "4.0.0";
                    switch (options.CodeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore3:
                            pkgVersion = "1.2.0";
                            break;
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "3.0.1";
                            break;
                    }

                    packages.Add(new NuGetPackage
                    {
                        PackageId = "EntityFrameworkCore.SqlServer.HierarchyId",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                        IsMainProviderPackage = false,
                        UseMethodName = "HierarchyId",
                    });
                }

                if (options.UseDateOnlyTimeOnly)
                {
                    pkgVersion = "7.0.1";
                    switch (options.CodeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "6.0.1";
                            break;
                    }

                    if (options.CodeGenerationMode == CodeGenerationMode.EFCore6
                        || options.CodeGenerationMode == CodeGenerationMode.EFCore7)
                    {
                        packages.Add(new NuGetPackage
                        {
                            PackageId = "ErikEJ.EntityFrameworkCore.SqlServer.DateOnlyTimeOnly",
                            Version = pkgVersion,
                            DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                            IsMainProviderPackage = false,
                            UseMethodName = "DateOnlyTimeOnly",
                        });
                    }
                }

                if (options.Tables.Any(t => t.ObjectType == ObjectType.Procedure)
                    && (options.CodeGenerationMode == CodeGenerationMode.EFCore6
                        || options.CodeGenerationMode == CodeGenerationMode.EFCore7)
                    && AdvancedOptions.Instance.DiscoverMultipleResultSets)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Dapper",
                        Version = "2.0.123",
                        DatabaseTypes = new List<DatabaseType> { DatabaseType.SQLServer, DatabaseType.SQLServerDacpac },
                        IsMainProviderPackage = false,
                        UseMethodName = null,
                    });
                }
            }

            if (databaseType == DatabaseType.SQLite)
            {
                var pkgVersion = "7.0.4";
                switch (options.CodeGenerationMode)
                {
                    case CodeGenerationMode.EFCore3:
                        pkgVersion = "3.1.32";
                        break;
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.15";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Microsoft.EntityFrameworkCore.Sqlite",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Sqlite",
                });

                if (options.UseNodaTime)
                {
                    pkgVersion = "7.0.0";
                    switch (options.CodeGenerationMode)
                    {
                        case CodeGenerationMode.EFCore6:
                            pkgVersion = "6.0.0";
                            break;
                    }

                    if (options.CodeGenerationMode == CodeGenerationMode.EFCore6
                        || options.CodeGenerationMode == CodeGenerationMode.EFCore7)
                    {
                        packages.Add(new NuGetPackage
                        {
                            PackageId = "EntityFrameworkCore.Sqlite.NodaTime",
                            Version = pkgVersion,
                            DatabaseTypes = new List<DatabaseType> { databaseType },
                            IsMainProviderPackage = false,
                            UseMethodName = "NodaTime",
                        });
                    }
                }
            }

            if (databaseType == DatabaseType.Npgsql)
            {
                var pkgVersion = "7.0.3";
                switch (options.CodeGenerationMode)
                {
                    case CodeGenerationMode.EFCore3:
                        pkgVersion = "3.1.18";
                        break;
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.8";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Npgsql.EntityFrameworkCore.PostgreSQL",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Npgsql",
                });

                if (options.UseSpatial)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { databaseType },
                        IsMainProviderPackage = false,
                        UseMethodName = "NetTopologySuite",
                    });
                }

                if (options.UseNodaTime)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Npgsql.EntityFrameworkCore.PostgreSQL.NodaTime",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { databaseType },
                        IsMainProviderPackage = false,
                        UseMethodName = "NodaTime",
                    });
                }
            }

            if (databaseType == DatabaseType.Mysql)
            {
                var pkgVersion = "7.0.0";
                switch (options.CodeGenerationMode)
                {
                    case CodeGenerationMode.EFCore3:
                        pkgVersion = "3.2.7";
                        break;
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.0.2";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Pomelo.EntityFrameworkCore.MySql",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Mysql",
                });

                if (options.UseSpatial)
                {
                    packages.Add(new NuGetPackage
                    {
                        PackageId = "Pomelo.EntityFrameworkCore.MySql.NetTopologySuite",
                        Version = pkgVersion,
                        DatabaseTypes = new List<DatabaseType> { databaseType },
                        IsMainProviderPackage = false,
                        UseMethodName = "NetTopologySuite",
                    });
                }
            }

            if (databaseType == DatabaseType.Oracle)
            {
                var pkgVersion = "7.21.9";
                switch (options.CodeGenerationMode)
                {
                    case CodeGenerationMode.EFCore3:
                        pkgVersion = "3.21.90";
                        break;
                    case CodeGenerationMode.EFCore6:
                        pkgVersion = "6.21.90";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "Oracle.EntityFrameworkCore",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Oracle",
                });
            }

            if (databaseType == DatabaseType.Firebird)
            {
                var pkgVersion = "9.1.1";
                switch (options.CodeGenerationMode)
                {
                    case CodeGenerationMode.EFCore3:
                        pkgVersion = "7.10.1";
                        break;
                }

                packages.Add(new NuGetPackage
                {
                    PackageId = "FirebirdSql.EntityFrameworkCore.Firebird",
                    Version = pkgVersion,
                    DatabaseTypes = new List<DatabaseType> { databaseType },
                    IsMainProviderPackage = true,
                    UseMethodName = "Firebird",
                });
            }

            return packages;
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

        private static async Task<bool> IsNetCore31Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETCoreApp,Version=v3.1") ?? false;
        }

        private static async Task<bool> IsNet60Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETCoreApp,Version=v6.0") ?? false;
        }

        private static async Task<bool> IsNet70Async(this Project project)
        {
            return (await project.GetAttributeAsync("TargetFrameworkMoniker"))?.Contains(".NETCoreApp,Version=v7.0") ?? false;
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
