using EnvDTE;
using ErikEJ.SqlCeToolbox.Dialogs;
using ErikEJ.SqlCeToolbox.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

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
            try
            {
                if (_package.Dte2.Mode == vsIDEMode.vsIDEModeDebug)
                {
                    EnvDteHelper.ShowError("Cannot generate code while debugging");
                    return;
                }

                var databaseList = EnvDteHelper.GetDataConnections(_package);

                var psd = new PickServerDatabaseDialog(databaseList, _package, new Dictionary<string, string>());
                var diagRes = psd.ShowModal();
                if (!diagRes.HasValue || !diagRes.Value) return;

                _package.Dte2.StatusBar.Text = "Loading schema information...";

                var dbInfo = psd.SelectedDatabase.Value;

                if (dbInfo.DatabaseType == DatabaseType.SQLCE35)
                {
                    EnvDteHelper.ShowError($"Unsupported provider: {dbInfo.ServerVersion}");
                    return;
                }

                var ptd = new PickTablesDialog();
                using (var repository = RepositoryHelper.CreateRepository(dbInfo))
                {
                    ptd.Tables = repository.GetAllTableNamesForExclusion();
                }

                var res = ptd.ShowModal();
                if (!res.HasValue || !res.Value) return;

                var name = RepositoryHelper.GetClassBasis(dbInfo.ConnectionString, dbInfo.DatabaseType);

                var path = Path.Combine(Path.GetTempPath(),
                    name + ".schema.dgml");

                if (File.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                }

                using (var repository = RepositoryHelper.CreateRepository(dbInfo))
                {
                    var generator = RepositoryHelper.CreateGenerator(repository, path, dbInfo.DatabaseType);
                    generator.GenerateSchemaGraph(dbInfo.ConnectionString, ptd.Tables);
                    File.SetAttributes(path, FileAttributes.ReadOnly);
                    _package.Dte2.ItemOperations.OpenFile(path);
                    _package.Dte2.ActiveDocument.Activate();
                }
                Telemetry.TrackEvent("PowerTools.GenerateSchemaDgml");
            }
            catch (Exception ex)
            {
                _package.LogError(new List<string>(), ex);
            }
        }
    }
}