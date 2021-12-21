using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                return null;

            return result
                .Where(s => s.EndsWith(".sqlproj", StringComparison.OrdinalIgnoreCase))
                .Concat(result.Where(s => s.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase)))
                .ToArray();
        }

        public static async Task<string> BuildSqlProjAsync(string sqlprojPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (sqlprojPath.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase)) return sqlprojPath;

            var project = await GetProjectAsync(sqlprojPath);
            if (project == null) return null;

            var searchPath = Path.Combine(Path.GetDirectoryName(project.FullPath), "bin");

            var files = Directory.GetFiles(searchPath, "*.dacpac", SearchOption.AllDirectories);

            if (!await VS.Build.ProjectIsUpToDateAsync(project))
            {
                var ok = await VS.Build.BuildProjectAsync(project, BuildAction.Rebuild);

                if (!ok)
                {
                    throw new Exception("Dacpac build failed");
                }
            }

            if (files.Length == 1)
            {
                return files[0];
            }

            throw new Exception("Dacpac build failed, please pick the file manually");
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

        /// <summary>
        /// Recursively walks over the project item tree and looks for *.dacpac linked files
        /// </summary>
        /// <param name="projectItems"></param>
        /// <param name="files"></param>
        private static async System.Threading.Tasks.Task LinkedFilesSearchAsync(IEnumerable<SolutionItem> projectItems, HashSet<string> files)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (var item in projectItems)
            {
                if (item.Children.Count() > 0)
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
                            files.Add(fullPath);
                    }
                }
            }
        }

        /// <summary>
        /// Looks for *.dacpac items which are added to the solution as links
        /// </summary>
        /// <param name="result">A collection with file paths. New unique paths will be added there</param>
        /// <param name="project"></param>
        private static async System.Threading.Tasks.Task AddLinkedFilesAsync(HashSet<string> result, Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (project.Children == null) return;

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
