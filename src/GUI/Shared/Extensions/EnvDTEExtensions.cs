using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace EFCorePowerTools.Extensions
{
    internal static class EnvDteExtensions
    {
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

        public static async Task<string[]> GetDacpacFilesInActiveSolutionAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var result = new HashSet<string>();

            var projects = await VS.Solutions.GetAllProjectsAsync();

            foreach (var item in projects)
            {
                var folder = Path.GetDirectoryName(item.FullPath);
                if (Directory.Exists(folder))
                {
                    AddFiles(result, folder);
                }

                try
                {
                    await AddLinkedFilesAsync(result, item);
                }
                catch
                {
                    // Ignore
                }
            }

            if (result.Count == 0)
            {
                return null;
            }

            return result
                .Where(s => s.EndsWith(".sqlproj", StringComparison.OrdinalIgnoreCase))
                .Concat(result.Where(s => s.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase)))
                .ToArray();
        }

        public static async Task<string> BuildSqlProjAsync(string sqlprojPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (sqlprojPath.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase))
            {
                return sqlprojPath;
            }

            var project = await GetProjectAsync(sqlprojPath);
            if (project == null)
            {
                return null;
            }

            var assemblyPath = await project.GetOutPutAssemblyPathAsync();

            var searchPath = Path.GetDirectoryName(assemblyPath);

            if (string.IsNullOrEmpty(searchPath))
            {
                searchPath = Path.Combine(Path.GetDirectoryName(project.FullPath), "bin");
            }

            if (!Directory.Exists(searchPath))
            {
                return null;
            }

            var files = Directory.GetFiles(searchPath, "*.dacpac", SearchOption.AllDirectories)
                .Where(f => !f.EndsWith("\\msdb.dacpac", StringComparison.OrdinalIgnoreCase)
                    && !f.EndsWith("\\master.dacpac", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!await VS.Build.ProjectIsUpToDateAsync(project))
            {
                var ok = await VS.Build.BuildProjectAsync(project, BuildAction.Rebuild);

                if (!ok)
                {
                    throw new InvalidOperationException("Dacpac build failed");
                }
            }

            if (files.Count == 1)
            {
                return files[0];
            }

            throw new InvalidOperationException("Dacpac build failed, please pick the file manually");
        }

        private static void AddFiles(HashSet<string> result, string path)
        {
            foreach (var file in Directory.GetFiles(path, "*.sqlproj", SearchOption.AllDirectories))
            {
                result.Add(file);
            }

            foreach (var file in Directory.GetFiles(path, "*.dacpac", SearchOption.AllDirectories))
            {
                result.Add(file);
            }
        }

        private static async System.Threading.Tasks.Task LinkedFilesSearchAsync(IEnumerable<SolutionItem> projectItems, HashSet<string> files)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (var item in projectItems)
            {
                if (item.Children.Any())
                {
                    await LinkedFilesSearchAsync(item.Children, files);
                }

                if (item.Type == SolutionItemType.PhysicalFile && item is Project)
                {
                    var project = item as Project;
                    var isLink = await project.GetAttributeAsync("IsLink");
                    var extension = await project.GetAttributeAsync("Extension");
                    if (isLink != null && isLink == "true" &&
                        extension != null && extension.Equals(".dacpac", StringComparison.OrdinalIgnoreCase))
                    {
                        var fullPath = item.FullPath;
                        if (!string.IsNullOrEmpty(fullPath))
                        {
                            files.Add(fullPath);
                        }
                    }
                }
            }
        }

        private static async System.Threading.Tasks.Task AddLinkedFilesAsync(HashSet<string> result, Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (project.Children == null)
            {
                return;
            }

            await LinkedFilesSearchAsync(project.Children, result);
        }

        private static async Task<Project> GetProjectAsync(string projectItemPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return (await VS.Solutions.GetAllProjectsAsync())
                .SingleOrDefault(p => p.FullPath == projectItemPath);
        }
    }
}
