using Community.VisualStudio.Toolkit;
using EnvDTE80;
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
    internal static class EnvDTEExtensions
    {
        public static async Task<string[]> GetDacpacFilesInActiveSolution()
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
                    await AddLinkedFiles(result, item);
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

        public static async Task<string> GetStartupProjectOutputPath()
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

                return await project.GetOutPutAssemblyPath();

            }
            catch
            {
                return null;
            }
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
        /// Looks for *.dacpac items which are added to the solution as links
        /// </summary>
        /// <param name="result">A collection with file paths. New unique paths will be added there</param>
        /// <param name="project"></param>
        private static async System.Threading.Tasks.Task AddLinkedFiles(HashSet<string> result, Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (project.Children == null) return;

            await LinkedFilesSearch(project.Children, result);
        }

        /// <summary>
        /// Recursively walks over the project item tree and looks for *.dacpac linked files
        /// </summary>
        /// <param name="projectItems"></param>
        /// <param name="files"></param>
        private static async System.Threading.Tasks.Task LinkedFilesSearch(IEnumerable<SolutionItem>  projectItems, HashSet<string> files)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (var item in projectItems)
            {
                if (item.Children.Count() > 0)
                {
                    await LinkedFilesSearch(item.Children, files);
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

        public static async Task<string> BuildSqlProjAsync(this DTE2 dte, string sqlprojPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (sqlprojPath.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase)) return sqlprojPath;

            if (sqlprojPath.EndsWith(".edmx", StringComparison.OrdinalIgnoreCase)) return sqlprojPath;

            var project = await GetProject(sqlprojPath);
            if (project == null) return null;

            var searchPath = Path.Combine(Path.GetDirectoryName(project.FullPath), "bin");

            var files = Directory.GetFiles(searchPath, "*.dacpac", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                File.Delete(file);
            }

            var buildStartTime = DateTime.Now;
            await project.BuildAsync();

            files = Directory.GetFiles(searchPath, "*.dacpac", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var lastWriteTime = File.GetLastWriteTime(file);
                if (lastWriteTime > buildStartTime)
                {
                    return file;
                }
            }

            return null;
        }

        private static async Task<Project> GetProject(string projectItemPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return (await VS.Solutions.GetAllProjectsAsync())
                .Where(p => p.FullPath == projectItemPath)
                .SingleOrDefault();
        }
    }
}
