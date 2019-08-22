using EFCorePowerTools.Extensions;
using EnvDTE;
using ErikEJ.SqlCeToolbox.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace EFCorePowerTools.Handlers
{
    using Contracts.Views;

    internal class MigrationsHandler
    {
        private readonly EFCorePowerToolsPackage _package;

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
                    throw new ArgumentNullException(nameof(project));
                }

                if (project.Properties.Item("TargetFrameworkMoniker") == null)
                {
                    EnvDteHelper.ShowError("The selected project type has no TargetFrameworkMoniker");
                    return;
                }

                if (!project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETFramework")
                    && !project.IsNetCore())
                {
                    EnvDteHelper.ShowError("Currently only .NET Framework and .NET Core 2.0 projects are supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                    return;
                }

                var outputFolder = Path.GetDirectoryName(outputPath);

                if (!project.IsNetCore() && !File.Exists(Path.Combine(outputFolder, "Microsoft.EntityFrameworkCore.dll")))
                {
                    EnvDteHelper.ShowError("EF Core is not installed in the current project");
                    return;
                }

                if (project.IsNetCore())
                {
                    var result = project.ContainsEfCoreDesignReference();

                    if (string.IsNullOrEmpty(result.Item2))
                    {
                        EnvDteHelper.ShowError("EF Core 2.1 or later not found in project");
                        return;
                    }

                    if (!result.Item1 && result.Item2 != "3.0.0.0")
                    {
                        var version = new Version(result.Item2);
                        var nugetHelper = new NuGetHelper();
                        nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", project, version);
                        EnvDteHelper.ShowError($"Installing EFCore.Design version {version}, please retry the command");
                        return;
                    }
                }                

                var migrationsDialog = _package.GetView<IMigrationOptionsDialog>();
                migrationsDialog.UseProjectForMigration(project)
                                .UseOutputPath(outputPath);

                migrationsDialog.ShowAndAwaitUserResponse(true);
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }

    }
}