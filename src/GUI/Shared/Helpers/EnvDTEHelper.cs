using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace EFCorePowerTools.Helpers
{
    internal class EnvDteHelper
    {
        internal static string[] GetProjectFilesInSolution(EFCorePowerToolsPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IVsSolution sol = package.GetService<IVsSolution>();
            uint numProjects;
            ErrorHandler.ThrowOnFailure(sol.GetProjectFilesInSolution((uint)__VSGETPROJFILESFLAGS.GPFF_SKIPUNLOADEDPROJECTS, 0, null, out numProjects));
            string[] projects = new string[numProjects];
            ErrorHandler.ThrowOnFailure(sol.GetProjectFilesInSolution((uint)__VSGETPROJFILESFLAGS.GPFF_SKIPUNLOADEDPROJECTS, numProjects, projects, out numProjects));
            //GetProjectFilesInSolution also returns solution folders, so we want to do some filtering
            //things that don't exist on disk certainly can't be project files
            return projects.Where(p => !string.IsNullOrEmpty(p) && File.Exists(p)).ToArray();
        }

        public static VSConstants.MessageBoxResult ShowError(string errorText)
        {
            return VS.Notifications.ShowError("EF Core Power Tools", errorText);
        }

        public static VSConstants.MessageBoxResult ShowMessage(string messageText)
        {
            return VS.Notifications.ShowMessage("EF Core Power Tools", messageText);
        }
    }
}
