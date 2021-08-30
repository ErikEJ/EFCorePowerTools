using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;

namespace EFCorePowerTools.Handlers
{
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

                if (await project.GetAttributeAsync("TargetFrameworkMoniker") == null)
                {
                    EnvDteHelper.ShowError(SharedLocale.SelectedProjectTypeNoTargetFrameworkMoniker);
                    return;
                }

                if (!await project.IsNetCore31OrHigher())
                {
                    EnvDteHelper.ShowError($"{SharedLocale.SupportedFramework}: {await project.GetAttributeAsync("TargetFrameworkMoniker")}");
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
                    var dte2 = await _package.GetServiceAsync(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as EnvDTE80.DTE2;

                    if (dte2 != null)
                    {
                        var dteProject = dte2.SelectedItems.Item(0).Project; var nugetHelper = new NuGetHelper();
                        nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", dteProject, version);
                        EnvDteHelper.ShowError(string.Format(SharedLocale.InstallingEfCoreDesignPackage, version));
                    }
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