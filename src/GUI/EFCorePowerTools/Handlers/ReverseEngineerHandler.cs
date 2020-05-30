﻿using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Shared.Models;
using EnvDTE;
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
    internal class ReverseEngineerHandler
    {
        private readonly EFCorePowerToolsPackage _package;
        private readonly ReverseEngineerHelper reverseEngineerHelper;

        public ReverseEngineerHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
            reverseEngineerHelper = new ReverseEngineerHelper();
        }

        public async void ReverseEngineerCodeFirst(Project project)
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

                var startTime = DateTime.Now;
                var projectPath = project.Properties.Item("FullPath").Value.ToString();
                var optionsPath = Path.Combine(projectPath, "efpt.config.json");
                var renamingPath = Path.Combine(projectPath, "efpt.renaming.json");

                var databaseList = EnvDteHelper.GetDataConnections(_package);
                var dacpacList = _package.Dte2.DTE.GetDacpacFilesInActiveSolution(EnvDteHelper.GetProjectFilesInSolution(_package));

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

                var includeViews = pickDataSourceResult.Payload.IncludeViews;

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

                if (includeViews && (dbInfo.DatabaseType == DatabaseType.SQLCE40))
                {
                    EnvDteHelper.ShowError($"Unsupported provider with EF Core 3.0: {dbInfo.DatabaseType}");
                    return;
                }

                var options = ReverseEngineerOptionsExtensions.TryRead(optionsPath);

                List<TableInformationModel> predefinedTables = !string.IsNullOrEmpty(dacpacPath)
                                           ? GetDacpacTables(dacpacPath, includeViews)
                                           : RepositoryHelper.GetTablesFromRepository(dbInfo, includeViews);

                var preselectedTables = new List<TableInformationModel>();
                if (options != null)
                {
                    dacpacSchema = options.DefaultDacpacSchema;
                    if (options.Tables.Count > 0)
                    {
                        var normalizedTables = reverseEngineerHelper.NormalizeTables(options.Tables, dbInfo.DatabaseType == DatabaseType.SQLServer);
                        preselectedTables.AddRange(normalizedTables);
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
                else if (dbInfo.DatabaseType == DatabaseType.Mysql)
                {
                    classBasis = EnvDteHelper.GetMysqlDatabaseName(dbInfo.ConnectionString);
                }
                else if (dbInfo.DatabaseType == DatabaseType.Oracle)
                {
                    classBasis = EnvDteHelper.GetOracleDatabaseName(dbInfo.ConnectionString);
                }
                else
                {
                    classBasis = RepositoryHelper.GetClassBasis(dbInfo.ConnectionString, dbInfo.DatabaseType);
                }
                var model = reverseEngineerHelper.GenerateClassName(classBasis) + "Context";
                var packageResult = project.ContainsEfCoreReference(dbInfo.DatabaseType);

                var presets = new ModelingOptionsModel
                {
                    InstallNuGetPackage = !packageResult.Item1,
                    ModelName = options != null ? options.ContextClassName : model,
                    ProjectName = project.Name,
                    Namespace = options != null ? options.ProjectRootNamespace : project.Properties.Item("DefaultNamespace").Value.ToString(),
                    DacpacPath = dacpacPath,
                };
                if (options != null)
                {
                    presets.UseDataAnnotations = !options.UseFluentApiOnly;
                    presets.UseDatabaseNames = options.UseDatabaseNames;
                    presets.UsePluralizer = options.UseInflector;
                    presets.UseDbContextSplitting = options.UseDbContextSplitting;
                    presets.UseHandelbars = options.UseHandleBars;
                    presets.SelectedHandlebarsLanguage = options.SelectedHandlebarsLanguage;
                    presets.ReplaceId = options.IdReplace;
                    presets.DoNotCombineNamespace = options.DoNotCombineNamespace;
                    presets.IncludeConnectionString = options.IncludeConnectionString;
                    presets.ModelName = options.ContextClassName;
                    presets.Namespace = options.ProjectRootNamespace;
                    presets.OutputPath = options.OutputPath;
                    presets.OutputContextPath = options.OutputContextPath;
                    presets.ModelNamespace = options.ModelNamespace;
                    presets.ContextNamespace = options.ContextNamespace;
                    presets.SelectedToBeGenerated = options.SelectedToBeGenerated;
                    presets.DacpacPath = options.Dacpac;
                }

                var modelDialog = _package.GetView<IModelingOptionsDialog>()
                                          .ApplyPresets(presets);

                _package.Dte2.StatusBar.Text = "Getting options...";

                var modelingOptionsResult = modelDialog.ShowAndAwaitUserResponse(true);
                if (!modelingOptionsResult.ClosedByOK) return;

                options = new ReverseEngineerOptions
                {
                    UseFluentApiOnly = !modelingOptionsResult.Payload.UseDataAnnotations,
                    ConnectionString = dbInfo.ConnectionString,
                    ContextClassName = modelingOptionsResult.Payload.ModelName,
                    DatabaseType = (ReverseEngineer20.DatabaseType)dbInfo.DatabaseType,
                    ProjectPath = projectPath,
                    OutputPath = modelingOptionsResult.Payload.OutputPath,
                    OutputContextPath = modelingOptionsResult.Payload.OutputContextPath,
                    ContextNamespace = modelingOptionsResult.Payload.ContextNamespace,
                    ModelNamespace = modelingOptionsResult.Payload.ModelNamespace,
                    ProjectRootNamespace = modelingOptionsResult.Payload.Namespace,
                    UseDatabaseNames = modelingOptionsResult.Payload.UseDatabaseNames,
                    UseInflector = modelingOptionsResult.Payload.UsePluralizer,
                    UseLegacyPluralizer = options?.UseLegacyPluralizer ?? false,
                    UseSpatial = options?.UseSpatial ?? false,
                    UseNodaTime = options?.UseNodaTime ?? false,
                    UseDbContextSplitting = modelingOptionsResult.Payload.UseDbContextSplitting,
                    IdReplace = modelingOptionsResult.Payload.ReplaceId,
                    DoNotCombineNamespace = modelingOptionsResult.Payload.DoNotCombineNamespace,
                    UseHandleBars = modelingOptionsResult.Payload.UseHandelbars,
                    SelectedHandlebarsLanguage = modelingOptionsResult.Payload.SelectedHandlebarsLanguage,
                    IncludeConnectionString = modelingOptionsResult.Payload.IncludeConnectionString,
                    SelectedToBeGenerated = modelingOptionsResult.Payload.SelectedToBeGenerated,
                    Dacpac = dacpacPath,
                    DefaultDacpacSchema = dacpacSchema,
                    Tables = pickTablesResult.Payload.ToList(),
                    CustomReplacers = customNameOptions
                };

                _package.Dte2.StatusBar.Text = "Generating code...";

                var tfm = project.Properties.Item("TargetFrameworkMoniker").Value.ToString();
                bool isNetStandard = tfm.Contains(".NETStandard,Version=v2.");

                if (modelingOptionsResult.Payload.UseHandelbars)
                {
                    var dropped = (DropTemplates(projectPath));
                    if (dropped && !project.IsNetCore() && !isNetStandard)
                    {
                        project.ProjectItems.AddFromDirectory(Path.Combine(projectPath, "CodeTemplates"));
                    }
                }

                var revEngResult = revEng.GenerateFiles(options, includeViews);

                if (modelingOptionsResult.Payload.SelectedToBeGenerated == 0 || modelingOptionsResult.Payload.SelectedToBeGenerated == 2)
                {
                    foreach (var filePath in revEngResult.EntityTypeFilePaths)
                    {
                        if (!project.IsNetCore() && !isNetStandard)
                        {
                            project.ProjectItems.AddFromFile(filePath);
                        }
                    }
                    if (modelingOptionsResult.Payload.SelectedToBeGenerated == 2)
                    {
                        if (File.Exists(revEngResult.ContextFilePath)) File.Delete(revEngResult.ContextFilePath);
                    }
                }
                if (modelingOptionsResult.Payload.SelectedToBeGenerated == 0 || modelingOptionsResult.Payload.SelectedToBeGenerated == 1)
                {
                    if (!project.IsNetCore() && !isNetStandard)
                    {
                        project.ProjectItems.AddFromFile(revEngResult.ContextFilePath);
                        _package.Dte2.ItemOperations.OpenFile(revEngResult.ContextFilePath);
                    }
                    if (modelingOptionsResult.Payload.SelectedToBeGenerated == 1)
                    {
                        foreach (var filePath in revEngResult.EntityTypeFilePaths)
                        {
                            if (File.Exists(filePath)) File.Delete(filePath);
                        }
                    }
                }

                var missingProviderPackage = packageResult.Item1 ? null : packageResult.Item2;
                if (modelingOptionsResult.Payload.InstallNuGetPackage || modelingOptionsResult.Payload.SelectedToBeGenerated == 2)
                {
                    missingProviderPackage = null;
                }

                _package.Dte2.StatusBar.Text = "Reporting result...";
                var errors = reverseEngineerHelper.ReportRevEngErrors(revEngResult, missingProviderPackage);

                SaveOptions(project, optionsPath, options);

                if (modelingOptionsResult.Payload.InstallNuGetPackage)
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

        private void SaveOptions(Project project, string optionsPath, ReverseEngineerOptions options)
        {
            if (!File.Exists(optionsPath + ".ignore"))
            {
                File.WriteAllText(optionsPath, options.Write(), Encoding.UTF8);
                project.ProjectItems.AddFromFile(optionsPath);
            }
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

        public List<TableInformationModel> GetDacpacTables(string dacpacPath, bool includeViews)
        {
            var builder = new DacpacTableListBuilder(dacpacPath);
            return builder.GetTableDefinitions(includeViews);
        }
    }
}