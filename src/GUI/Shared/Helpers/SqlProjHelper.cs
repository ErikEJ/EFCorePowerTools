using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Extensions;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.Helpers
{
    public static class SqlProjHelper
    {
        public static string SetRelativePathForSqlProj(string uiHint, string projectDirectory)
        {
            if (Path.IsPathRooted(uiHint))
            {
                uiHint = PathExtensions.GetRelativePath(projectDirectory, uiHint);
            }

            if (Path.IsPathRooted(uiHint))
            {
                return null;
            }

            return uiHint;
        }

        public static string GetFullPathForSqlProj(string uiHint, string projectDirectory)
        {
            if (string.IsNullOrEmpty(projectDirectory))
            {
                return uiHint;
            }

            if (uiHint.EndsWith(".sqlproj", StringComparison.OrdinalIgnoreCase)
                || uiHint.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                || uiHint.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase))
            {
                return PathHelper.GetAbsPath(uiHint, projectDirectory);
            }

            return uiHint;
        }

        public static async Task<string[]> GetDacpacProjectsInActiveSolutionAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var result = new HashSet<string>();

            var projects = await VS.Solutions.GetAllProjectsAsync();

            foreach (var item in projects)
            {
                var folder = Path.GetDirectoryName(item.FullPath);
                if (!Directory.Exists(folder))
                {
                    continue;
                }

                if (await item.IsSqlDatabaseProjectAsync())
                {
                    result.Add(item.FullPath);
                    continue;
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
                .Concat(result.Where(s => s.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)))
                .Concat(result.Where(s => s.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase)))
                .ToArray();
        }

        public static async Task<string> BuildSqlProjectAsync(string sqlprojPath)
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

            if (!await VS.Build.ProjectIsUpToDateAsync(project))
            {
                var ok = await VS.Build.BuildProjectAsync(project, BuildAction.Build);

                if (!ok)
                {
                    throw new InvalidOperationException("Dacpac build failed");
                }
            }

            var dacpacPath = await project.GetDacpacPathAsync();

            if (!string.IsNullOrEmpty(dacpacPath))
            {
                return dacpacPath;
            }

            throw new InvalidOperationException("Dacpac build failed, please pick the file manually");
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

                if (item.Type == SolutionItemType.PhysicalFile)
                {
                    var file = item as PhysicalFile;
                    var fullPath = file.Parent?.FullPath;

                    if (file.Extension == ".dacpac"
                        && !string.IsNullOrEmpty(fullPath)
                        && !string.IsNullOrEmpty(file.FullPath)
                        && !fullPath.StartsWith(Path.GetDirectoryName(file.FullPath), StringComparison.OrdinalIgnoreCase))
                    {
                        files.Add(file.FullPath);
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