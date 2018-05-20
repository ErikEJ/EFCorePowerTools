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
                    EnvDteHelper.ShowError("Currently only .NET Framework and .NET Core 2.0 projects are supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                    return;
                }

                //if (!project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETFramework")
                //    && !project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v2.0"))
                //{
                //    EnvDteHelper.ShowError("Currently only .NET Framework and .NET Core 2.0 projects are supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                //    return;
                //}

                bool isNetCore = project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v2.");

                var processResult = _processLauncher.GetOutput(outputPath, isNetCore, GenerationType.MigrationStatus, null, null);

                if (processResult.StartsWith("Error:"))
                {
                    throw new ArgumentException(processResult, nameof(processResult));
                }

                var result = BuildModelResult(processResult);

                var message = string.Empty;

                foreach (var item in result)
                {
                    message += $"{item.Item1} : {item.Item2}{Environment.NewLine}"; 
                }

                EnvDteHelper.ShowMessage(message);

                //TODO Pass status to Dialog and show

                //TODO Handle 2 different actions from dialog here
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }

        //TODO Avoid duplication
        private List<Tuple<string, string>> BuildModelResult(string modelInfo)
        {
            var result = new List<Tuple<string, string>>();

            var contexts = modelInfo.Split(new[] { "DbContext:" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var context in contexts)
            {
                var parts = context.Split(new[] { "DebugView:" + Environment.NewLine }, StringSplitOptions.None);
                result.Add(new Tuple<string, string>(parts[0].Trim(), parts[1].Trim()));
            }

            return result;
        }
    }
}