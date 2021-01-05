using ErikEJ.SqlCeToolbox.Helpers;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.Handlers
{
    internal class ServerDgmlHandler
    {
        public ServerDgmlHandler()
        {
        }

        public void GenerateServerDgmlFiles()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            EnvDteHelper.ShowError("This feature is no longer supported, install SQLite Toolbox to get it back.");
            return;
        }
    }
}