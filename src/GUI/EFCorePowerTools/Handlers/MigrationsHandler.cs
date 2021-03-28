using EFCorePowerTools.Extensions;
using EnvDTE;
using System;
using System.Collections.Generic;

namespace EFCorePowerTools.Handlers
{
    using Contracts.Views;
    using EFCorePowerTools.Helpers;
    using EFCorePowerTools.Locales;
    using Microsoft.VisualStudio.Shell;

    internal class MigrationsHandler
    {
        private readonly EFCorePowerToolsPackage _package;

        public MigrationsHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        public async System.Threading.Tasks.Task ManageMigrationsAsync(string outputPath, Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

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
                    EnvDteHelper.ShowError(SharedLocale.SelectedProjectTypeNoTargetFrameworkMoniker);
                    return;
                }

                if (!project.IsNetCore31OrHigher())
                {
                    EnvDteHelper.ShowError($"{SharedLocale.SupportedFramework}: {project.Properties.Item("TargetFrameworkMoniker").Value}");
                    return;
                }

                var result = await project.ContainsEfCoreDesignReferenceAsync();

                if (string.IsNullOrEmpty(result.Item2))
                {
                    EnvDteHelper.ShowError(SharedLocale.EFCoreVersionNotFound);
                    return;
                }

                if (!result.Item1)
                {
                    if (!Version.TryParse(result.Item2, out Version version))
                    {
                        EnvDteHelper.ShowError(string.Format(MigrationsLocale.CannotSupportVersion, version));
                        return;
                    }
                    var nugetHelper = new NuGetHelper();
                    nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", project, version);
                    EnvDteHelper.ShowError(string.Format(SharedLocale.InstallingEfCoreDesignPackage, version));
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