using EnvDTE;
using ErikEJ.SqlCeToolbox.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace EFCorePowerTools.Handlers
{
    using System.Linq;
    using Contracts.Views;
    using ReverseEngineer20;
    using Shared.Models;

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

                var psd = _package.GetView<IPickServerDatabaseDialog>();
                psd.PublishConnections(databaseList.Select(m => new DatabaseConnectionModel
                {
                    ConnectionName = m.Value.Caption,
                    ConnectionString = m.Value.ConnectionString,
                    DatabaseType = m.Value.DatabaseType
                }));

                var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
                if (!pickDataSourceResult.ClosedByOK)
                    return;
                
                _package.Dte2.StatusBar.Text = "Loading schema information...";

                // Reload the database list, in case the user has added a new database in the dialog
                databaseList = EnvDteHelper.GetDataConnections(_package);

                DatabaseInfo dbInfo = null;
                if (pickDataSourceResult.Payload.Connection != null)
                {
                    dbInfo = databaseList.Single(m => m.Value.ConnectionString == pickDataSourceResult.Payload.Connection?.ConnectionString).Value;
                }

                if (dbInfo == null)
                {
                    // User didn't select a database, should be impossible, though
                    return;
                }

                if (dbInfo.DatabaseType == DatabaseType.SQLCE35 
                    || dbInfo.DatabaseType == DatabaseType.Mysql
                    || dbInfo.DatabaseType == DatabaseType.Npgsql)
                {
                    EnvDteHelper.ShowError($"Unsupported provider: {dbInfo.ServerVersion}");
                    return;
                }

                var predefinedTables = new List<TableInformationModel>();
                using (var repository = RepositoryHelper.CreateRepository(dbInfo))
                {
                    var tables = repository.GetAllTableNamesForExclusion();
                    foreach (var table in tables)
                    {
                        predefinedTables.Add(new TableInformationModel(table, true, false));
                    }
                }

                var ptd = _package.GetView<IPickTablesDialog>()
                                  .AddTables(predefinedTables);

                var (closedByOk, selectedTables) = ptd.ShowAndAwaitUserResponse(true);
                if (!closedByOk) return;
                var unselectedTables = predefinedTables.Except(selectedTables).Select(m => m.Name).ToList();

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
                    generator.GenerateSchemaGraph(dbInfo.ConnectionString, unselectedTables);
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