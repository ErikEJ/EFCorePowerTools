﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.Helpers;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.Handlers
{
    internal class DacpacAnalyzerHandler
    {
        private readonly EFCorePowerToolsPackage package;

        public DacpacAnalyzerHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
        }

        public async Task GenerateAsync(string projectPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                await VS.StatusBar.ShowProgressAsync("Generating DACPAC Analysis report...", 1, 3);

                var dacpacPath = await SqlProjHelper.BuildSqlProjAsync(projectPath);

                await VS.StatusBar.ShowProgressAsync("Generating DACPAC Analysis report...", 2, 3);

                var reportPath = await GetDacpacReportAsync(dacpacPath);

                await VS.StatusBar.ShowProgressAsync("Generating DACPAC Analysis report...", 3, 3);

                ShowReport(reportPath);

                Telemetry.TrackEvent("PowerTools.GenerateDacpacReport");
            }
            catch (Exception exception)
            {
                package.LogError(new List<string>(), exception);
            }
            finally
            {
                await VS.StatusBar.ClearAsync();
            }
        }

        private async Task<string> GetDacpacReportAsync(string dacpacPath)
        {
            var launcher = new EfRevEngLauncher(null, CodeGenerationMode.EFCore8);
            return await launcher.GetReportPathAsync(dacpacPath);
        }

        private void ShowReport(string path)
        {
            if (File.Exists(path))
            {
                Process.Start(path);
            }
        }
    }
}
