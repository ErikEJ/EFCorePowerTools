using System.Collections.Generic;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;

namespace EFCorePowerTools.ItemWizard
{
    public class ReverseEngineerWizardLauncher : IWizard
    {
        // Update vstemplate version reference if main assembly version changes
        // In \src\GUI\WizardItemTemplate\WizardItemTemplate.vstemplate
        public void RunStarted(
            object automationObject,
            Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind,
            object[] customParams)
        {
            if (PackageManager.Package != null)
            {
                var handler = new ReverseEngineerHandler(PackageManager.Package);
                handler.ReverseEngineerCodeFirstAsync().FireAndForget();
            }
        }

        // This method is only called for item templates,
        // not for project templates.
        public bool ShouldAddProjectItem(string filePath)
        {
            return false;
        }

        // This method is called after the project is created.
        public void RunFinished()
        {
            // Ignore
        }

        // This method is only called for item templates,
        // not for project templates.
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            // Never called due to false above
        }

        // This method is called before opening any item that
        // has the OpenInEditor attribute.
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            // Never called, as this is diabled
        }

        public void ProjectFinishedGenerating(Project project)
        {
            // Never called, this is an item template
        }
    }
}
