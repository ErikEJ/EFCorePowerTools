using EnvDTE;
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

                if (project.Properties.Item("TargetFrameworkMoniker") == null)
                {
                    EnvDteHelper.ShowError("The selected project type has no TargetFrameworkMoniker");
                    return;
                }

                if (!project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETFramework")
                    && !project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v2.0"))
                {
                    EnvDteHelper.ShowError("Currently only .NET Framework and .NET Core 2.0 projects are supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                    return;
                }

                bool isNetCore = project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v2.");

                //TODO 3 different actions from dialog
                var processResult = _processLauncher.GetOutput(outputPath, isNetCore, GenerationType.MigrationStatus, null, null);

                if (processResult.StartsWith("Error:"))
                {
                    throw new ArgumentException(processResult, nameof(processResult));
                }

                //TODO Show status somewhere? - Dialog/ToolWindow ?
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }
    }
}