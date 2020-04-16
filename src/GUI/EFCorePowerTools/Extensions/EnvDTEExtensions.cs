using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EFCorePowerTools.Extensions
{
    internal static class EnvDTEExtensions
    {
        public static string[] GetDacpacFilesInActiveSolution(this DTE dte, string[] projectPaths)
        {
            var result = new List<string>();

            if (!dte.Solution.IsOpen)
                return null;

            foreach (var projectPath in projectPaths)
            {
                var folder = Path.GetDirectoryName(projectPath);
                if (Directory.Exists(folder))
                {
                    AddFiles(result, folder);
                }                
            }

            if (result.Count == 0)
                return null;

            return result
                .Where(s => s.EndsWith(".sqlproj", StringComparison.OrdinalIgnoreCase))
                .Concat(result.Where(s => s.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase)))
                .ToArray();
        }

        private static void AddFiles(List<string> result, string path)
        {
            var files = DirSearch(path, "*.sqlproj");
            foreach (var file in files)
            {
                if (!result.Contains(file))
                    result.Add(file);
            }

            files = DirSearch(path, "*.dacpac");
            foreach (var file in files)
            {
                if (!result.Contains(file))
                    result.Add(file);
            }
        }

        public static string BuildSqlProj(this DTE dte, string sqlprojPath)
        {
            if (sqlprojPath.EndsWith(".dacpac")) return sqlprojPath;

            var project = GetProject(dte, sqlprojPath);
            if (project == null) return null;

            var searchPath = Path.Combine(Path.GetDirectoryName(project.FullName), "bin");

            var files = DirSearch(searchPath, "*.dacpac");
            foreach (var file in files)
            {
                File.Delete(file);
            }

            var buildStartTime = DateTime.Now;
            if (!project.TryBuild()) return null;

            files = DirSearch(searchPath, "*.dacpac");
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

        private static Project GetProject(DTE dte, string projectItemPath)
        {
            var projects = Projects(dte);
            foreach (var project in projects)
            {
                // Accessing project.FullName might throw a NotImplementedException (from COM).
                // In that case, the best match we can find is using the UniqueName.
                try
                {
                    if (project.FullName == projectItemPath)
                    {
                        return project;
                    }
                }
                catch (NotImplementedException)
                {
                    if (projectItemPath.EndsWith(project.UniqueName))
                    {
                        return project;
                    }
                }
            }
            return null;
        }

        private static IList<Project> Projects(DTE dte)
        {
            Projects projects = dte.Solution.Projects;
            List<Project> list = new List<Project>();
            var item = projects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                {
                    continue;
                }

                if (project.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                {
                    list.AddRange(GetSolutionFolderProjects(project));
                }
                else
                {
                    list.Add(project);
                }
            }

            return list;
        }

        private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            List<Project> list = new List<Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                {
                    continue;
                }

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                {
                    list.AddRange(GetSolutionFolderProjects(subProject));
                }
                else
                {
                    list.Add(subProject);
                }
            }
            return list;
        }

        public static List<string> DirSearch(string sDir, string pattern)
        {
            var files = new List<String>();

            try
            {
                foreach (string f in Directory.GetFiles(sDir, pattern))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d, pattern));
                }
            }
            catch
            {
            }

            return files;
        }


    }
}
