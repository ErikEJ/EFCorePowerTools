using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace EFCorePowerTools.Helpers
{
    internal static class VSHelper
    {
        public static async Task<bool> IsDebugModeAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IVsDebugger debugger = await VS.Services.GetDebuggerAsync();

            DBGMODE[] mode = new DBGMODE[1];
            ErrorHandler.ThrowOnFailure(debugger.GetMode(mode));

            if (mode[0] != DBGMODE.DBGMODE_Design)
            {
                return true;
            }

            return false;
        }
        public static VSConstants.MessageBoxResult ShowError(string errorText)
        {
            return VS.MessageBox.ShowError("EF Core Power Tools", errorText);
        }

        public static VSConstants.MessageBoxResult ShowMessage(string messageText)
        {
            return VS.MessageBox.Show("EF Core Power Tools", messageText);
        }
    }
}
