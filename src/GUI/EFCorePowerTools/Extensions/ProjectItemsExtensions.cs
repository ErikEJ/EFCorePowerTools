namespace EFCorePowerTools.Extensions
{
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Linq;

    internal static class ProjectItemsExtensions
    {
        public static ProjectItem GetItem(this ProjectItems projectItems, string name)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return projectItems
                .Cast<ProjectItem>()
                .FirstOrDefault(
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                    pi => string.Equals(pi.Name, name, StringComparison.OrdinalIgnoreCase));
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }
    }
}