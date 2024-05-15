using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
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

        public async Task GenerateAsync(string path, bool isConnectionString)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            try
            {
                await VS.StatusBar.ShowProgressAsync("Generating DACPAC Analysis report...", 1, 3);

                string dacpacPath;
                if (isConnectionString)
                {
                    var builder = new SqlConnectionStringBuilderHelper().GetBuilder(path);
                    dacpacPath = builder.ConnectionString;
                }
                else
                {
                    dacpacPath = await SqlProjHelper.BuildSqlProjAsync(path);
                }

                await VS.StatusBar.ShowProgressAsync("Generating DACPAC Analysis report...", 2, 3);

                var reportPath = await GetDacpacReportAsync(dacpacPath, isConnectionString);

                await VS.StatusBar.ShowProgressAsync("Generating DACPAC Analysis report...", 3, 3);

                if (File.Exists(reportPath))
                {
                    await OpenVsWebBrowserAsync(reportPath);
                }

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

        private async Task<string> GetDacpacReportAsync(string path, bool isConnectionString)
        {
            var launcher = new EfRevEngLauncher(null, CodeGenerationMode.EFCore8);
            return await launcher.GetReportPathAsync(path, isConnectionString);
        }

        private void OpenWebBrowser(string path)
        {
            Process.Start(path);
        }

        private async Task OpenVsWebBrowserAsync(string path)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var service = await VS.GetServiceAsync<SVsWebBrowsingService, IVsWebBrowsingService>();

            if (service == null)
            {
                OpenWebBrowser(path);
                return;
            }

            service.Navigate(path, (uint)__VSWBNAVIGATEFLAGS.VSNWB_ForceNew, out var frame);
            frame.Show();
        }
    }
}
