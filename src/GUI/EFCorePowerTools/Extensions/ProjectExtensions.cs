using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using EnvDTE;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using NuGet.ProjectModel;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VSLangProj;

namespace EFCorePowerTools.Extensions
{
    internal static class ProjectExtensions
    {
        public const int SOk = 0;

        public static bool TryBuild(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var dte = project.DTE;
            var configuration = dte.Solution.SolutionBuild.ActiveConfiguration.Name;

            dte.Solution.SolutionBuild.BuildProject(configuration, project.UniqueName, true);

            return dte.Solution.SolutionBuild.LastBuildInfo == 0;
        }

        public static string GetOutPutAssemblyPath(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var assemblyName = project.Properties.Item("AssemblyName").Value.ToString();

            var assemblyNameExe = assemblyName + ".exe";
            var assemblyNameDll = assemblyName + ".dll";

            var outputPath = GetOutputPath(project);

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

            var projectPath = project.Properties.Item("FullPath")?.Value.ToString();

            if (string.IsNullOrEmpty(projectPath))
            {
                return result;
            }

            var files = Directory.GetFiles(projectPath, "efpt.*config.json", SearchOption.AllDirectories);
            result.AddRange(files
                .Where(f => !f.Contains("\\bin\\") && !f.Contains("\\obj\\")));
                
            if (result.Count() == 0)
            {
                result.Add(Path.Combine(projectPath, "efpt.config.json"));
            }

            return result.OrderBy(s => s).ToList();
        }

        public static string GetRenamingPath(this Project project, string optionsPath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string renamingPath;

            if (string.IsNullOrEmpty(optionsPath))
            {
                renamingPath = project.Properties.Item("FullPath")?.Value.ToString();

                if (string.IsNullOrEmpty(renamingPath))
                {
                    return null;
                }
            }
            else
            {
                renamingPath = Path.GetDirectoryName(optionsPath);
            }

            return Path.Combine(renamingPath, "efpt.renaming.json");
        }

        public static async System.Threading.Tasks.Task<string> GetCspPropertyAsync(this Project project, string propertyName)
        {
            var unconfiguredProject = GetUnconfiguredProject(project);
            var configuredProject = await unconfiguredProject.GetSuggestedConfiguredProjectAsync();
            var properties = configuredProject.Services.ProjectPropertiesProvider.GetCommonProperties();
            return await properties.GetEvaluatedPropertyValueAsync(propertyName);
        }

        private static UnconfiguredProject GetUnconfiguredProject(EnvDTE.Project project)
        {
            var context = project as IVsBrowseObjectContext;
            return context?.UnconfiguredProject;
        }

        public static Tuple<bool, string> ContainsEfCoreReference(this Project project, DatabaseType dbType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var providerPackage = "Microsoft.EntityFrameworkCore.SqlServer";
            if (dbType == DatabaseType.SQLCE40)
            {
                providerPackage = "EntityFrameworkCore.SqlServerCompact40";
            }
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

            var vsProject = project.Object as VSProject;
            if (vsProject == null) return new Tuple<bool, string>(false, providerPackage);
            for (var i = 1; i < vsProject.References.Count + 1; i++)
            {
                if (vsProject.References.Item(i).Name.Equals(providerPackage))
                {
                    return new Tuple<bool, string>(true, providerPackage);
                }
            }
            return new Tuple<bool, string>(false, providerPackage);
        }

        public static async System.Threading.Tasks.Task<Tuple<bool, string>> ContainsEfSchemaCompareReferenceAsync(this Project project)
        {
            return await ContainsReferenceAsync(project, "EfCore.SchemaCompare");
        }

        public static async System.Threading.Tasks.Task<Tuple<bool, string>> ContainsEfCoreDesignReferenceAsync(this Project project)
        {
            return await ContainsReferenceAsync(project, "Microsoft.EntityFrameworkCore.Design");
        }

        private static async System.Threading.Tasks.Task<Tuple<bool, string>> ContainsReferenceAsync(Project project, string designPackage)
        {
            var corePackage = "Microsoft.EntityFrameworkCore";

            bool hasDesign = false;
            string coreVersion = string.Empty;
            var projectAssetsFile = await project.GetCspPropertyAsync("ProjectAssetsFile");

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

        public static bool IsNetFramework(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETFramework,");
        }

        public static bool IsNetCore31OrHigher(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return IsNetCore31(project) || IsNet50(project);
        }

        private static bool IsNetCore31(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v3.1");
        }

        private static bool IsNet50(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v5.0");
        }

        private static string GetOutputPath(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var configManager = project.ConfigurationManager;
            if (configManager == null) return null;

            var activeConfig = configManager.ActiveConfiguration;
            var outputPath = activeConfig.Properties.Item("OutputPath").Value.ToString();
            var fullName = project.FullName;

            var absoluteOutputPath = RevEng.Shared.PathHelper.GetAbsPath(outputPath, fullName);

            return absoluteOutputPath;
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
                    EnvDteHelper.ShowError($"{SharedLocale.InvalidName}: {item.Item1}");
                    return list;
                }

                var filePath = Path.Combine(Path.GetTempPath(),
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
    }
}