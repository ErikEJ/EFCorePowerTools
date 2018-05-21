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
                    EnvDteHelper.ShowError("Currently only .NET Framework is supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
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

                _package.Dte2.StatusBar.Text = "Getting Migration Status";
                var processResult = _processLauncher.GetOutput(outputPath, isNetCore, GenerationType.MigrationStatus, null, null, null);

                if (processResult.StartsWith("Error:"))
                {
                    throw new ArgumentException(processResult, nameof(processResult));
                }

                _package.Dte2.StatusBar.Text = "Showing Migration Status";

                var result = BuildModelResult(processResult);
                var msd = new EfCoreMigrationsDialog(result, _package, outputPath, isNetCore, project)
                {
                    ProjectName = project.Name
                };

                msd.ShowModal();

                _package.Dte2.StatusBar.Text = string.Empty;
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }

        public static SortedDictionary<string, string> BuildModelResult(string modelInfo)
        {
            var result = new SortedDictionary<string, string>();

            var contexts = modelInfo.Split(new[] { "DbContext:" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var context in contexts)
            {
                var parts = context.Split(new[] { "DebugView:" + Environment.NewLine }, StringSplitOptions.None);
                result.Add(parts[0].Trim(), parts[1].Trim());
            }

            return result;
        }
    }
}