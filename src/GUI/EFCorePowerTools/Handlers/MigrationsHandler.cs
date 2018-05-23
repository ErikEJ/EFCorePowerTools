using EnvDTE;
using ErikEJ.SqlCeToolbox.Dialogs;
using ErikEJ.SqlCeToolbox.Helpers;
using System;
using System.Collections.Generic;

namespace EFCorePowerTools.Handlers
{
    internal class MigrationsHandler
    {
        private readonly EFCorePowerToolsPackage _package;
        private readonly ProcessLauncher _processLauncher = new ProcessLauncher();

        public MigrationsHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        public void ManageMigrations(string outputPath, Project project)
        {
            try
            {

                if (string.IsNullOrEmpty(outputPath))
                {
                    throw new ArgumentException(outputPath, nameof(outputPath));
                }

                if (project == null)
                {
                    throw new ArgumentNullException(outputPath, nameof(outputPath));
                }

                if (project.Properties.Item("TargetFrameworkMoniker") == null)
                {
                    EnvDteHelper.ShowError("The selected project type has no TargetFrameworkMoniker");
                    return;
                }

                if (!project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETFramework"))
                {
                    EnvDteHelper.ShowError("Currently only .NET Framework is supported (.NET Core coming soon) - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                    return;
                }

                //TODO Enable NetCore support
                //if (!project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETFramework")
                //    && !project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v2.0"))
                //{
                //    EnvDteHelper.ShowError("Currently only .NET Framework and .NET Core 2.0 projects are supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                //    return;
                //}

                bool isNetCore = project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v2.");

                var msd = new EfCoreMigrationsDialog(_package, outputPath, isNetCore, project)
                {
                    ProjectName = project.Name
                };

                msd.ShowModal();
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }

    }
}