using System;
using System.Collections.Generic;
using ErikEJ.SqlCeToolbox.Helpers;

namespace EFCorePowerTools.Handlers
{
    internal class AboutHandler
    {
        private readonly EFCorePowerToolsPackage _package;

        public AboutHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        public void ShowDialog()
        {
            try
            {
                var dialog = new AboutDialog(_package);
                dialog.ShowDialog();
                Telemetry.TrackEvent("PowerTools.About");
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }
    }
}