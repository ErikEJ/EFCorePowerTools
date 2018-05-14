namespace EFCorePowerTools.Extensions
{
    using System;
    using System.Linq;
    using EnvDTE;
 
    internal static class ProjectItemsExtensions
    {
        public static ProjectItem GetItem(this ProjectItems projectItems, string name)
        {
            return projectItems
                .Cast<ProjectItem>()
                .FirstOrDefault(
                    pi => string.Equals(pi.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}