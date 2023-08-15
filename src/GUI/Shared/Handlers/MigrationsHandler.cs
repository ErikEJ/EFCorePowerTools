using System;
using System.Collections.Generic;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.Handlers
{
    internal class MigrationsHandler
    {
        private readonly EFCorePowerToolsPackage package;

        public MigrationsHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
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

                if (await project.GetAttributeAsync("TargetFrameworkMoniker") == null)
                {
                    VSHelper.ShowError(SharedLocale.SelectedProjectTypeNoTargetFrameworkMoniker);
                    return;
                }

                if (!await project.IsNet60OrHigherAsync())
                {
                    VSHelper.ShowError($"{SharedLocale.SupportedFramework}: {await project.GetAttributeAsync("TargetFrameworkMoniker")}");
                    return;
                }

                var result = await project.ContainsEfCoreDesignReferenceAsync();
                if (string.IsNullOrEmpty(result.Item2))
                {
                    VSHelper.ShowError(SharedLocale.EFCoreVersionNotFound);
                    return;
                }

                if (!Version.TryParse(result.Item2, out Version version))
                {
                    VSHelper.ShowError(string.Format(ModelAnalyzerLocale.CurrentEFCoreVersion, result.Item2));
                    if (!result.Item1)
                    {
                        return;
                    }
                }

                if (!result.Item1)
                {
                    var nugetHelper = new NuGetHelper();
                    nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", project, version);
                    VSHelper.ShowError(string.Format(SharedLocale.InstallingEfCoreDesignPackage, version));
                    return;
                }

                var migrationsDialog = package.GetView<IMigrationOptionsDialog>();
                migrationsDialog.UseProjectForMigration(project)
                                .UseOutputPath(outputPath);

                migrationsDialog.ShowAndAwaitUserResponse(true);
            }
            catch (Exception exception)
            {
                package.LogError(new List<string>(), exception);
            }
        }
    }
}
