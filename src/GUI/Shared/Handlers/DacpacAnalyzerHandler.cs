using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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
                var dacpacPath = await SqlProjHelper.BuildSqlProjAsync(projectPath);

                var reportPath = await GetDacpacReportAsync(dacpacPath);

                ShowReport(reportPath);

                Telemetry.TrackEvent("PowerTools.GenerateDacpacReport");
            }
            catch (Exception exception)
            {
                package.LogError(new List<string>(), exception);
            }
        }

        private async Task<string> GetDacpacReportAsync(string dacpacPath)
        {
            var launcher = new EfRevEngLauncher(null, CodeGenerationMode.EFCore6);
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
