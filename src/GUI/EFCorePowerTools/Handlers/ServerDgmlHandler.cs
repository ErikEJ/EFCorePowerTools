using ErikEJ.SqlCeToolbox.Helpers;

namespace EFCorePowerTools.Handlers
{
    internal class ServerDgmlHandler
    {
        private readonly EFCorePowerToolsPackage _package;

        public ServerDgmlHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        public void GenerateServerDgmlFiles()
        {
            EnvDteHelper.ShowError("This feature is no longer supported, install SQLite Toolbox to get it back.");
            return;
        }
    }
}