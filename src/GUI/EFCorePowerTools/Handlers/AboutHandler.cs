using System;
using System.Collections.Generic;
using ErikEJ.SqlCeToolbox.Helpers;

namespace EFCorePowerTools.Handlers
{
    using Contracts.Views;
    using Microsoft.VisualStudio.Shell;

    internal class AboutHandler
    {
        private readonly EFCorePowerToolsPackage _package;

        public AboutHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        public void ShowDialog()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                var dialog = _package.GetView<IAboutDialog>();
                dialog.ShowAndAwaitUserResponse(false);
                Telemetry.TrackEvent("PowerTools.About");
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }
    }
}