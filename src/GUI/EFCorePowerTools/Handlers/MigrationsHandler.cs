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

        public async System.Threading.Tasks.Task ManageMigrationsAsync(string outputPath, Project project)
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

                if (!project.IsNetCore30OrHigher())
                {
                    EnvDteHelper.ShowError("Only .NET Core 3.0+ projects are supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                    return;
                }

                var outputFolder = Path.GetDirectoryName(outputPath);

                var result = await project.ContainsEfCoreDesignReferenceAsync();

                if (string.IsNullOrEmpty(result.Item2))
                {
                    EnvDteHelper.ShowError("EF Core 3.1 or later not found in project");
                    return;
                }

                if (!result.Item1)
                {
                    if (!Version.TryParse(result.Item2, out Version version))
                    {
                        EnvDteHelper.ShowError($"Cannot support version {version}, notice that previews are not supported.");
                        return;
                    }
                    var nugetHelper = new NuGetHelper();
                    nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", project, version);
                    EnvDteHelper.ShowError($"Installing EFCore.Design version {version}, please retry the command");
                    return;
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