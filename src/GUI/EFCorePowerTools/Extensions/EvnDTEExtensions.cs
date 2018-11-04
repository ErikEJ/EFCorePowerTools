using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;

namespace EFCorePowerTools.Extensions
{
    internal static class EvnDTEExtensions
    {
        public static string[] GetDacpacFilesInActiveSolution(this DTE dte)
        {
            var result = new List<string>();

            if (!dte.Solution.IsOpen)
                return null;

            TryGetInitialPath(dte, out var path);

            if (path != null)
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

            if (result.Count == 0)
                return null;

            return result.ToArray();
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

            if (!project.TryBuild()) return null;

            files = DirSearch(searchPath, "*.dacpac");
            foreach (var file in files)
            {
                if (File.GetLastWriteTime(file) > DateTime.Now.AddSeconds(-2))
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
                if (project.FullName == projectItemPath)
                {
                    return project;
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

        private static bool TryGetInitialPath(DTE dte, out string path)
        {
            try
            {
                path = GetInitialFolder(dte);
                return true;
            }
            catch
            {
                path = null;
                return false;
            }
        }

        private static string GetInitialFolder(DTE dte)
        {
            if (!dte.Solution.IsOpen)
                return null;
            return Path.GetDirectoryName(dte.Solution.FullName);
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
