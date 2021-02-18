using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Shared.Models;
using EnvDTE;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    internal class ReverseEngineerHandler
    {
        private readonly EFCorePowerToolsPackage _package;
        private readonly ReverseEngineerHelper reverseEngineerHelper;
        private readonly object _icon;

        public ReverseEngineerHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
            _icon = (short)Microsoft.VisualStudio.Shell.Interop.Constants.SBAI_Build;
            reverseEngineerHelper = new ReverseEngineerHelper();
        }

        public async System.Threading.Tasks.Task ReverseEngineerCodeFirstAsync(Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (_package.Dte2.Mode == vsIDEMode.vsIDEModeDebug)
            {
                EnvDteHelper.ShowError("Cannot generate code while debugging");
                return;
            }
            var projectPath = project.Properties.Item("FullPath")?.Value.ToString();
            var optionsPaths = project.GetConfigFiles();
            var optionsPath = optionsPaths.First();

            if (optionsPaths.Count > 1)
            {
                var pcd = _package.GetView<IPickConfigDialog>();
                pcd.PublishConfigurations(optionsPaths.Select(m => new ConfigModel
                {
                    ConfigPath = m,
                    ProjectPath = projectPath
                }));

                var pickConfigResult = pcd.ShowAndAwaitUserResponse(true);
                if (!pickConfigResult.ClosedByOK)
                    return;

                optionsPath = pickConfigResult.Payload.ConfigPath;
            }
            await ReverseEngineerCodeFirstAsync(project, optionsPath, false);

        }

        public async System.Threading.Tasks.Task ReverseEngineerCodeFirstAsync(Project project, string optionsPath, bool onlyGenerate)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                var dteH = new EnvDteHelper();

                if (_package.Dte2.Mode == vsIDEMode.vsIDEModeDebug)
                {
                    EnvDteHelper.ShowError("Cannot generate code while debugging");
                    return;
                }

                var renamingPath = project.GetRenamingPath();
                var namingOptionsAndPath = CustomNameOptionsExtensions.TryRead(renamingPath, optionsPath);

                Tuple<bool, string> containsEfCoreReference = null;

                var options = ReverseEngineerOptionsExtensions.TryRead(optionsPath) ?? new ReverseEngineerOptions();

                options.ProjectPath = project.Properties.Item("FullPath")?.Value.ToString();

                if (string.IsNullOrWhiteSpace(options.ProjectRootNamespace))
                    options.ProjectRootNamespace = project.Properties.Item("DefaultNamespace").Value.ToString();

                bool forceEdit = false;

                if (onlyGenerate)
                {
                    forceEdit = !ChooseDataBaseConnectionByUiHint(options);

                    if (forceEdit)
                    {
                        _package.Dte2.StatusBar.Text = "Database connection not found, cannot refresh.";
                    }
                    else
                    {
                        containsEfCoreReference = project.ContainsEfCoreReference(options.DatabaseType);
                        options.InstallNuGetPackage = false;
                    }

                }

                if (!onlyGenerate || forceEdit)
                {
                    if (!ChooseDataBaseConnection(options))
                        return;

                    _package.Dte2.StatusBar.Text = "Getting ready to connect...";

                    DatabaseInfo dbInfo = GetDatabaseInfo(options);

                    if (dbInfo == null)
                        return;

                    _package.Dte2.StatusBar.Text = "Loading database objects...";

                    if (!await LoadDataBaseObjectsAsync(options, dbInfo, namingOptionsAndPath))
                        return;

                    _package.Dte2.StatusBar.Text = "Loading options...";

                    containsEfCoreReference = project.ContainsEfCoreReference(options.DatabaseType);
                    options.InstallNuGetPackage = !containsEfCoreReference.Item1;

                    if (!GetModelOptions(options, project.Name))
                        return;

                    SaveOptions(project, optionsPath, options, new Tuple<List<Schema>, string>(options.CustomReplacers, namingOptionsAndPath.Item2));
                }

                VerifySQLServerRightsAndVersion(options);

                GenerateEfRevEng(project, options, containsEfCoreReference);

                if (options.InstallNuGetPackage && (!onlyGenerate || forceEdit))
                {
                    _package.Dte2.StatusBar.Text = "Installing EF Core provider package";
                    var nuGetHelper = new NuGetHelper();
                    await nuGetHelper.InstallPackageAsync(containsEfCoreReference.Item2, project);
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


        private bool ChooseDataBaseConnectionByUiHint(ReverseEngineerOptions options)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var databaseList = VsDataHelper.GetDataConnections(_package);
            if (databaseList != null && databaseList.Any())
            {
                var dataBaseInfo = databaseList.Values.FirstOrDefault(m => m.Caption == options.UiHint);
                if (dataBaseInfo != null)
                {
                    options.ConnectionString = dataBaseInfo.ConnectionString;
                    options.DatabaseType = dataBaseInfo.DatabaseType;
                    return true;
                }
            }
            return false;
        }

        private bool ChooseDataBaseConnection(ReverseEngineerOptions options)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var databaseList = VsDataHelper.GetDataConnections(_package);
            var dacpacList = _package.Dte2.DTE.GetDacpacFilesInActiveSolution(EnvDteHelper.GetProjectFilesInSolution(_package));

            var psd = _package.GetView<IPickServerDatabaseDialog>();

            if (databaseList != null && databaseList.Any())
            {
                psd.PublishConnections(databaseList.Select(m => new DatabaseConnectionModel
                {
                    ConnectionName = m.Value.Caption,
                    ConnectionString = m.Value.ConnectionString,
                    DatabaseType = m.Value.DatabaseType,
                    DataConnection = m.Value.DataConnection,
                }));
            }

            if (dacpacList != null && dacpacList.Any())
            {
                psd.PublishDefinitions(dacpacList.Select(m => new DatabaseDefinitionModel
                {
                    FilePath = m
                }));
            }


            if (options.FilterSchemas && options.Schemas != null && options.Schemas.Any())
            {
                psd.PublishSchemas(options.Schemas);
            }

            psd.PublishCodeGenerationMode(options.CodeGenerationMode);

            if (!string.IsNullOrEmpty(options.UiHint))
            {
                psd.PublishUiHint(options.UiHint);
            }

            var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
            if (!pickDataSourceResult.ClosedByOK)
                return false;

            options.CodeGenerationMode = pickDataSourceResult.Payload.IncludeViews ? CodeGenerationMode.EFCore5 : CodeGenerationMode.EFCore3;
            options.FilterSchemas = pickDataSourceResult.Payload.FilterSchemas;
            options.Schemas = options.FilterSchemas ? pickDataSourceResult.Payload.Schemas?.ToList() : null;
            options.UiHint = pickDataSourceResult.Payload.UiHint;
            options.Dacpac = pickDataSourceResult.Payload.Definition?.FilePath;


            if (pickDataSourceResult.Payload.Connection != null)
            {
                options.ConnectionString = pickDataSourceResult.Payload.Connection.ConnectionString;
                options.DatabaseType = pickDataSourceResult.Payload.Connection.DatabaseType;
            }

            return true;
        }

        private DatabaseInfo GetDatabaseInfo(ReverseEngineerOptions options)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            DatabaseInfo dbInfo = new DatabaseInfo();

            if (!string.IsNullOrEmpty(options.ConnectionString))
            {
                dbInfo.ConnectionString = options.ConnectionString;
                dbInfo.DatabaseType = options.DatabaseType;
            }

            if (!string.IsNullOrEmpty(options.Dacpac))
            {
                dbInfo.DatabaseType = DatabaseType.SQLServerDacpac;
                dbInfo.ConnectionString = $"Data Source=(local);Initial Catalog={Path.GetFileNameWithoutExtension(options.Dacpac)};Integrated Security=true;";
                options.Dacpac = _package.Dte2.DTE.BuildSqlProj(options.Dacpac);
                if (string.IsNullOrEmpty(options.Dacpac))
                {
                    EnvDteHelper.ShowMessage("Unable to build selected Database Project");
                    return null;
                }
            }

            if (dbInfo.DatabaseType == DatabaseType.SQLCE35
                || dbInfo.DatabaseType == DatabaseType.SQLCE40
                || dbInfo.DatabaseType == DatabaseType.Undefined)
            {
                EnvDteHelper.ShowError($"Unsupported provider: {dbInfo.ServerVersion}");
                return null;
            }

            return dbInfo;
        }



        private async Task<bool> LoadDataBaseObjectsAsync(ReverseEngineerOptions options, DatabaseInfo dbInfo, Tuple<List<Schema>, string> namingOptionsAndPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            _package.Dte2.StatusBar.Animate(true, _icon);
            var predefinedTables = !string.IsNullOrEmpty(options.Dacpac)
                                       ? await GetDacpacTablesAsync(options.Dacpac, options.CodeGenerationMode == CodeGenerationMode.EFCore5)
                                       : await GetTablesAsync(dbInfo, options.CodeGenerationMode == CodeGenerationMode.EFCore5, options.Schemas?.ToArray());
            _package.Dte2.StatusBar.Animate(false, _icon);

            var preselectedTables = new List<SerializationTableModel>();

            if (options.Tables?.Count > 0)
            {
                var normalizedTables = reverseEngineerHelper.NormalizeTables(options.Tables, dbInfo.DatabaseType == DatabaseType.SQLServer);
                preselectedTables.AddRange(normalizedTables);
            }

            _package.Dte2.StatusBar.Clear();

            var ptd = _package.GetView<IPickTablesDialog>()
                              .AddTables(predefinedTables, namingOptionsAndPath.Item1)
                              .PreselectTables(preselectedTables);

            var pickTablesResult = ptd.ShowAndAwaitUserResponse(true);

            options.Tables = pickTablesResult.Payload.Objects.ToList();
            options.CustomReplacers = pickTablesResult.Payload.CustomReplacers.ToList();
            return (pickTablesResult.ClosedByOK);
        }


        private bool GetModelOptions(ReverseEngineerOptions options, string projectName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var classBasis = VsDataHelper.GetDatabaseName(options.ConnectionString, options.DatabaseType);
            var model = reverseEngineerHelper.GenerateClassName(classBasis) + "Context";

            if (string.IsNullOrEmpty(options.ContextClassName))
                options.ContextClassName = model;

            var presets = new ModelingOptionsModel
            {
                InstallNuGetPackage = options.InstallNuGetPackage,
                ModelName = options.ContextClassName,
                ProjectName = projectName,
                Namespace = options.ProjectRootNamespace,
                DacpacPath = options.Dacpac,
                UseDataAnnotations = !options.UseFluentApiOnly,
                UseDatabaseNames = options.UseDatabaseNames,
                UsePluralizer = options.UseInflector,
                UseDbContextSplitting = options.UseDbContextSplitting,
                UseHandlebars = options.UseHandleBars,
                SelectedHandlebarsLanguage = options.SelectedHandlebarsLanguage,
                IncludeConnectionString = options.IncludeConnectionString,
                OutputPath = options.OutputPath,
                OutputContextPath = options.OutputContextPath,
                ModelNamespace = options.ModelNamespace,
                ContextNamespace = options.ContextNamespace,
                SelectedToBeGenerated = options.SelectedToBeGenerated,
                UseEf6Pluralizer = options.UseLegacyPluralizer,
                MapSpatialTypes = options.UseSpatial,
                MapNodaTimeTypes = options.UseNodaTime,
                UseBoolPropertiesWithoutDefaultSql = options.UseBoolPropertiesWithoutDefaultSql,
                UseNoConstructor = options.UseNoConstructor,
                UseNoNavigations = options.UseNoNavigations,
                UseNullableReferences = options.UseNullableReferences,
            };

            var modelDialog = _package.GetView<IModelingOptionsDialog>()
                                          .ApplyPresets(presets);

            _package.Dte2.StatusBar.Clear();

            var modelingOptionsResult = modelDialog.ShowAndAwaitUserResponse(true);

            if (!modelingOptionsResult.ClosedByOK)
                return false;

            options.InstallNuGetPackage = modelingOptionsResult.Payload.InstallNuGetPackage;
            options.UseFluentApiOnly = !modelingOptionsResult.Payload.UseDataAnnotations;
            options.ContextClassName = modelingOptionsResult.Payload.ModelName;
            options.OutputPath = modelingOptionsResult.Payload.OutputPath;
            options.OutputContextPath = modelingOptionsResult.Payload.OutputContextPath;
            options.ContextNamespace = modelingOptionsResult.Payload.ContextNamespace;
            options.ModelNamespace = modelingOptionsResult.Payload.ModelNamespace;
            options.ProjectRootNamespace = modelingOptionsResult.Payload.Namespace;
            options.UseDatabaseNames = modelingOptionsResult.Payload.UseDatabaseNames;
            options.UseInflector = modelingOptionsResult.Payload.UsePluralizer;
            options.UseLegacyPluralizer = modelingOptionsResult.Payload.UseEf6Pluralizer;
            options.UseSpatial = modelingOptionsResult.Payload.MapSpatialTypes;
            options.UseNodaTime = modelingOptionsResult.Payload.MapNodaTimeTypes;
            options.UseDbContextSplitting = modelingOptionsResult.Payload.UseDbContextSplitting;
            options.UseHandleBars = modelingOptionsResult.Payload.UseHandlebars;
            options.SelectedHandlebarsLanguage = modelingOptionsResult.Payload.SelectedHandlebarsLanguage;
            options.IncludeConnectionString = modelingOptionsResult.Payload.IncludeConnectionString;
            options.SelectedToBeGenerated = modelingOptionsResult.Payload.SelectedToBeGenerated;
            options.UseBoolPropertiesWithoutDefaultSql = modelingOptionsResult.Payload.UseBoolPropertiesWithoutDefaultSql;
            options.UseNullableReferences = modelingOptionsResult.Payload.UseNullableReferences;
            options.UseNoConstructor = modelingOptionsResult.Payload.UseNoConstructor;
            options.UseNoNavigations = modelingOptionsResult.Payload.UseNoNavigations;

            return true;
        }

        private void VerifySQLServerRightsAndVersion(ReverseEngineerOptions options)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (options.DatabaseType == DatabaseType.SQLServer && string.IsNullOrEmpty(options.Dacpac))
            {
                var rightsAndVersion = reverseEngineerHelper.HasSqlServerViewDefinitionRightsAndVersion(options.ConnectionString);

                if (rightsAndVersion.Item1 == false)
                {
                    EnvDteHelper.ShowMessage("The SQL Server user does not have 'VIEW DEFINITION' rights, default constraints may not be available.");
                }

                if (rightsAndVersion.Item2.Major < 11)
                {
                    EnvDteHelper.ShowMessage($"SQL Server version {rightsAndVersion.Item2} may not be supported.");
                }
            }
        }

        private void GenerateEfRevEng(Project project, ReverseEngineerOptions options, Tuple<bool, string> containsEfCoreReference)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var startTime = DateTime.Now;

            if (options.UseHandleBars)
            {
                var dropped = (DropTemplates(options.ProjectPath, options.CodeGenerationMode == CodeGenerationMode.EFCore5));
                if (dropped)
                {
                    project.ProjectItems.AddFromDirectory(Path.Combine(options.ProjectPath, "CodeTemplates"));
                }
            }

            _package.Dte2.StatusBar.Animate(true, _icon);
            _package.Dte2.StatusBar.Text = "Generating code...";
            var revEngResult = EfRevEngLauncher.LaunchExternalRunner(options, options.CodeGenerationMode == CodeGenerationMode.EFCore5);
            _package.Dte2.StatusBar.Animate(false, _icon);

            if (options.SelectedToBeGenerated == 0 || options.SelectedToBeGenerated == 2)
            {
                foreach (var filePath in revEngResult.EntityTypeFilePaths)
                {
                    project.ProjectItems.AddFromFile(filePath);
                }
                if (options.SelectedToBeGenerated == 2)
                {
                    if (File.Exists(revEngResult.ContextFilePath)) File.Delete(revEngResult.ContextFilePath);
                    foreach (var filePath in revEngResult.ContextConfigurationFilePaths)
                    {
                        if (File.Exists(filePath)) File.Delete(filePath);
                    }
                }
            }

            if (options.SelectedToBeGenerated == 0 || options.SelectedToBeGenerated == 1)
            {
                foreach (var filePath in revEngResult.ContextConfigurationFilePaths)
                {
                    project.ProjectItems.AddFromFile(filePath);
                }
                project.ProjectItems.AddFromFile(revEngResult.ContextFilePath);

                _package.Dte2.ItemOperations.OpenFile(revEngResult.ContextFilePath);

                if (options.SelectedToBeGenerated == 1)
                {
                    foreach (var filePath in revEngResult.EntityTypeFilePaths)
                    {
                        if (File.Exists(filePath)) File.Delete(filePath);
                    }
                }
            }

            var duration = DateTime.Now - startTime;

            var missingProviderPackage = containsEfCoreReference.Item1 ? null : containsEfCoreReference.Item2;
            if (options.InstallNuGetPackage || options.SelectedToBeGenerated == 2)
            {
                missingProviderPackage = null;
            }

            _package.Dte2.StatusBar.Text = "Reporting result...";
            var errors = reverseEngineerHelper.ReportRevEngErrors(revEngResult, missingProviderPackage);

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


        }

        private void SaveOptions(Project project, string optionsPath, ReverseEngineerOptions options, Tuple<List<Schema>, string> renamingOptions)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!File.Exists(optionsPath + ".ignore"))
            {
                File.WriteAllText(optionsPath, options.Write(), Encoding.UTF8);
                project.ProjectItems.AddFromFile(optionsPath);
            }

            if (renamingOptions.Item1 != null && !File.Exists(renamingOptions.Item2 + ".ignore") && renamingOptions.Item1.Count() > 0)
            {
                File.WriteAllText(renamingOptions.Item2, CustomNameOptionsExtensions.Write(renamingOptions.Item1), Encoding.UTF8);
                project.ProjectItems.AddFromFile(renamingOptions.Item2);
            }
        }



        private bool DropTemplates(string projectPath, bool useEFCore5)
        {
            var zipName = useEFCore5 ? "CodeTemplates501.zip" : "CodeTemplates.zip";

            var toDir = Path.Combine(projectPath, "CodeTemplates");
            var fromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!Directory.Exists(toDir))
            {
                Directory.CreateDirectory(toDir);
                ZipFile.ExtractToDirectory(Path.Combine(fromDir, zipName), toDir);
                return true;
            }

            return false;
        }

        private async Task<List<TableModel>> GetDacpacTablesAsync(string dacpacPath, bool useEFCore5)
        {
            var builder = new TableListBuilder(dacpacPath, DatabaseType.SQLServerDacpac, null);
            return await System.Threading.Tasks.Task.Run(() => builder.GetTableDefinitions(useEFCore5));
        }

        private async Task<List<TableModel>> GetTablesAsync(DatabaseInfo dbInfo, bool useEFCore5, SchemaInfo[] schemas)
        {
            if (dbInfo.DataConnection != null)
            {
                dbInfo.DataConnection.Open();
                dbInfo.ConnectionString = DataProtection.DecryptString(dbInfo.DataConnection.EncryptedConnectionString);
            }

            var builder = new TableListBuilder(dbInfo.ConnectionString, dbInfo.DatabaseType, schemas);
            return await System.Threading.Tasks.Task.Run(() => builder.GetTableDefinitions(useEFCore5));
        }
    }
}