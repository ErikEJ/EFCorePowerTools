using System;
using System.Collections.Generic;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Helpers;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.Handlers
{
    internal class AboutHandler
    {
        private readonly EFCorePowerToolsPackage package;

        public AboutHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
        }

        public void ShowDialog()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                var dialog = package.GetView<IAboutDialog>();
                dialog.ShowAndAwaitUserResponse(true);
                Telemetry.TrackEvent("PowerTools.About");
            }
            catch (Exception exception)
            {
                EFCorePowerToolsPackage.LogError(new List<string>(), exception);
            }
        }
    }
}