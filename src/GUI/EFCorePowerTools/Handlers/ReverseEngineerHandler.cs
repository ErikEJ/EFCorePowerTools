using EFCorePowerTools.Extensions;
using EnvDTE;
using ErikEJ.SqlCeToolbox.Dialogs;
using ErikEJ.SqlCeToolbox.Helpers;
using ReverseEngineer20;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EFCorePowerTools.Handlers
{
    using Contracts.Views;
    using Dialogs;
    using ReverseEngineer20.ReverseEngineer;
    using Shared.Enums;
    using Shared.Models;

    internal class ReverseEngineerHandler
    {
        private readonly EFCorePowerToolsPackage _package;

        public ReverseEngineerHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        public async void ReverseEngineerCodeFirst(string assemblyPath, Project project)
        {
            try
            {
                var dteH = new EnvDteHelper();
                var revEng = new EfCoreReverseEngineer();
                string dacpacSchema = null;

                if (_package.Dte2.Mode == vsIDEMode.vsIDEModeDebug)
                {
                    EnvDteHelper.ShowError("Cannot generate code while debugging");
                    return;
                }

                if (string.IsNullOrEmpty(assemblyPath))
                {
                    throw new ArgumentException(assemblyPath, nameof(assemblyPath));
                }

                var startTime = DateTime.Now;
                var projectPath = project.Properties.Item("FullPath").Value.ToString();
                var optionsPath = Path.Combine(projectPath, "efpt.config.json");
                var renamingPath = Path.Combine(projectPath, "efpt.renaming.json");

                var databaseList = EnvDteHelper.GetDataConnections(_package);
                var dacpacList = _package.Dte2.DTE.GetDacpacFilesInActiveSolution();

                var psd = _package.GetView<IPickServerDatabaseDialog>();

                if (databaseList != null && databaseList.Any())
                {
                    psd.PublishConnections(databaseList.Select(m => new DatabaseConnectionModel
                    {
                        ConnectionName = m.Value.Caption,
                        ConnectionString = m.Value.ConnectionString,
                        DatabaseType = m.Value.DatabaseType
                    }));
                }

                if (dacpacList != null && dacpacList.Any())
                {
                    psd.PublishDefinitions(dacpacList.Select(m => new DatabaseDefinitionModel
                    {
                        FilePath = m
                    }));
                }

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
                var dacpacPath = pickDataSourceResult.Payload.Definition?.FilePath;

                if (dbInfo == null) dbInfo = new DatabaseInfo();

                if (!string.IsNullOrEmpty(dacpacPath))
                {
                    dbInfo.DatabaseType = DatabaseType.SQLServer;
                    dbInfo.ConnectionString = "Data Source=.;Initial Catalog=" + Path.GetFileNameWithoutExtension(dacpacPath);
                    dacpacPath = _package.Dte2.DTE.BuildSqlProj(dacpacPath);
                    if (string.IsNullOrEmpty(dacpacPath))
                    {
                        EnvDteHelper.ShowMessage("Unable to build selected Database Project");
                        return;
                    }
                }

                if (dbInfo.DatabaseType == DatabaseType.SQLCE35)
                {
                    EnvDteHelper.ShowError($"Unsupported provider: {dbInfo.ServerVersion}");
                    return;
                }

                var options = ReverseEngineerOptionsExtensions.TryRead(optionsPath);

                var predefinedTables = !string.IsNullOrEmpty(dacpacPath)
                                           ? revEng.GetDacpacTables(dacpacPath)
                                           : GetTablesFromRepository(dbInfo);

                var preselectedTables = new List<TableInformationModel>();
                if (options != null)
                {
                    dacpacSchema = options.DefaultDacpacSchema;
                    if (options.Tables.Count > 0)
                    {
                        preselectedTables.AddRange(options.Tables);
                    }
                }

                var ptd = _package.GetView<IPickTablesDialog>()
                                  .AddTables(predefinedTables)
                                  .PreselectTables(preselectedTables);
                
                var customNameOptions = CustomNameOptionsExtensions.TryRead(renamingPath);

                var pickTablesResult = ptd.ShowAndAwaitUserResponse(true);
                if (!pickTablesResult.ClosedByOK) return;

                var classBasis = string.Empty;
                if (dbInfo.DatabaseType == DatabaseType.Npgsql)
                {
                    classBasis = EnvDteHelper.GetNpgsqlDatabaseName(dbInfo.ConnectionString);
                }
                else
                {
                    classBasis = RepositoryHelper.GetClassBasis(dbInfo.ConnectionString, dbInfo.DatabaseType);
                }
                var model = revEng.GenerateClassName(classBasis) + "Context";
                var packageResult = project.ContainsEfCoreReference(dbInfo.DatabaseType);

                var modelDialog = new EfCoreModelDialog(options)
                {
                    InstallNuGetPackage = !packageResult.Item1,
                    ModelName = options != null ? options.ContextClassName : model,
                    ProjectName = project.Name,
                    NameSpace = options != null ? options.ProjectRootNamespace : project.Properties.Item("DefaultNamespace").Value.ToString(),
                    DacpacPath = dacpacPath
                };

                _package.Dte2.StatusBar.Text = "Getting options...";
                if (modelDialog.ShowModal() != true) return;

                options = new ReverseEngineerOptions
                {
                    UseFluentApiOnly = !modelDialog.UseDataAnnotations,
                    ConnectionString = dbInfo.ConnectionString,
                    ContextClassName = modelDialog.ModelName,
                    DatabaseType = (ReverseEngineer20.DatabaseType)dbInfo.DatabaseType,
                    ProjectPath = projectPath,
                    AssemblyPath = assemblyPath,
                    OutputPath = modelDialog.OutputPath,
                    ProjectRootNamespace = modelDialog.NameSpace,
                    UseDatabaseNames = modelDialog.UseDatabaseNames,
                    UseInflector = modelDialog.UsePluralizer,
                    IdReplace = modelDialog.ReplaceId,
                    UseHandleBars = modelDialog.UseHandelbars,
                    IncludeConnectionString = modelDialog.IncludeConnectionString,
                    SelectedToBeGenerated = modelDialog.SelectedTobeGenerated,
                    Dacpac = dacpacPath,
                    DefaultDacpacSchema = dacpacSchema,
                    Tables = pickTablesResult.Payload.ToList(),
                    CustomReplacers = customNameOptions
                };

                _package.Dte2.StatusBar.Text = "Generating code...";

                var tfm = project.Properties.Item("TargetFrameworkMoniker").Value.ToString();
                bool isNetStandard = tfm.Contains(".NETStandard,Version=v2.0");

                if (modelDialog.UseHandelbars)
                {
                    var dropped = (DropTemplates(projectPath));
                    if (dropped && !project.IsNetCore() && !isNetStandard)
                    {
                        project.ProjectItems.AddFromDirectory(Path.Combine(projectPath, "CodeTemplates"));
                    }
                }

                var revEngResult = revEng.GenerateFiles(options);

                if (modelDialog.SelectedTobeGenerated == 0 || modelDialog.SelectedTobeGenerated == 2)
                {
                    foreach (var filePath in revEngResult.EntityTypeFilePaths)
                    {
                        project.ProjectItems.AddFromFile(filePath);
                    }
                    if (modelDialog.SelectedTobeGenerated == 2)
                    {
                        if (File.Exists(revEngResult.ContextFilePath)) File.Delete(revEngResult.ContextFilePath);
                    }
                }
                if (modelDialog.SelectedTobeGenerated == 0 || modelDialog.SelectedTobeGenerated == 1)
                {
                    project.ProjectItems.AddFromFile(revEngResult.ContextFilePath);
                    if (!project.IsNetCore() && !isNetStandard)
                    {
                        _package.Dte2.ItemOperations.OpenFile(revEngResult.ContextFilePath);
                    }
                    if (modelDialog.SelectedTobeGenerated == 1)
                    {
                        foreach (var filePath in revEngResult.EntityTypeFilePaths)
                        {
                            if (File.Exists(filePath)) File.Delete(filePath);
                        }
                    }
                }

                var missingProviderPackage = packageResult.Item1 ? null : packageResult.Item2;
                if (modelDialog.InstallNuGetPackage || modelDialog.SelectedTobeGenerated == 2)
                {
                    missingProviderPackage = null;
                }

                _package.Dte2.StatusBar.Text = "Reporting result...";
                var errors = ReportRevEngErrors(revEngResult, missingProviderPackage);

                SaveOptions(project, optionsPath, options);

                if (modelDialog.InstallNuGetPackage)
                {
                    _package.Dte2.StatusBar.Text = "Installing EF Core provider package";
                    var nuGetHelper = new NuGetHelper();
                    await nuGetHelper.InstallPackageAsync(packageResult.Item2, project);
                }
                var duration = DateTime.Now - startTime;
                _package.Dte2.StatusBar.Text = $"Reverse engineer completed in {duration:h\\:mm\\:ss}";

                EnvDteHelper.ShowMessage(errors);

                if (revEngResult.EntityErrors.Count > 0)
                {
                    _package.LogError(revEngResult.EntityErrors, null);
                }
                if (revEngResult.EntityWarnings.Count > 0)
                {
                    _package.LogError(revEngResult.EntityWarnings, null);
                }
                Telemetry.TrackEvent("PowerTools.ReverseEngineer");
            }
            catch (AggregateException ae)
            {
                foreach (var innerException in ae.Flatten().InnerExceptions)
                {
                    _package.LogError(new List<string>(), innerException);
                }
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }

        private List<TableInformationModel> GetTablesFromRepository(DatabaseInfo dbInfo)
        {
            if (dbInfo.DatabaseType == DatabaseType.Npgsql)
            {
                return EnvDteHelper.GetNpgsqlTableNames(dbInfo.ConnectionString);
            }

            using (var repository = RepositoryHelper.CreateRepository(dbInfo))
            {
                var allPks = repository.GetAllPrimaryKeys();
                var tableList = repository.GetAllTableNamesForExclusion();
                var tables = new List<TableInformationModel>();

                foreach (var table in tableList)
                {
                    var hasPrimaryKey = allPks.Any(m => m.TableName == table);
                    tables.Add(new TableInformationModel(table, hasPrimaryKey));
                }
                return tables;
            }
        }

        private void SaveOptions(Project project, string optionsPath, ReverseEngineerOptions options)
        {
            if (!File.Exists(optionsPath + ".ignore"))
            {
                File.WriteAllText(optionsPath, options.Write(), Encoding.UTF8);
                project.ProjectItems.AddFromFile(optionsPath);
            }
        }

        private string ReportRevEngErrors(EfCoreReverseEngineerResult revEngResult, string missingProviderPackage)
        {
            var errors = new StringBuilder();
            if (revEngResult.EntityErrors.Count == 0)
            {
                errors.Append("Model generated successfully." + Environment.NewLine);
            }
            else
            {
                errors.Append("Please check the output window for errors" + Environment.NewLine);
            }

            if (revEngResult.EntityWarnings.Count > 0)
            {
                errors.Append("Please check the output window for warnings" + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(missingProviderPackage))
            {
                errors.AppendLine();
                errors.AppendFormat("The \"{0}\" NuGet package was not found in the project - it must be installed in order to build.", missingProviderPackage);
            }

            return errors.ToString();
        }

        private bool DropTemplates(string projectPath)
        {
            var toDir = Path.Combine(projectPath, "CodeTemplates");
            var fromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!Directory.Exists(toDir))
            {
                Directory.CreateDirectory(toDir);
                ZipFile.ExtractToDirectory(Path.Combine(fromDir, "CodeTemplates.zip"), toDir);
                return true;
            }

            return false;
        }
    }
}