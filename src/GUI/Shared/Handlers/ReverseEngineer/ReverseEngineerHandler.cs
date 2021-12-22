using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Shared.Models;
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
        private readonly VsDataHelper vsDataHelper;

        public ReverseEngineerHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
            reverseEngineerHelper = new ReverseEngineerHelper();
            vsDataHelper = new VsDataHelper();

        }

        public async System.Threading.Tasks.Task ReverseEngineerCodeFirstAsync(Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (await VSHelper.IsDebugModeAsync())
                {
                    VSHelper.ShowError(ReverseEngineerLocale.CannotGenerateCodeWhileDebugging);
                    return;
                }

                var projectPath = project.FullPath;
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

        public async System.Threading.Tasks.Task ReverseEngineerCodeFirstAsync(Project project, string optionsPath, bool onlyGenerate)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (await VSHelper.IsDebugModeAsync())
                {
                    VSHelper.ShowError(ReverseEngineerLocale.CannotGenerateCodeWhileDebugging);
                    return;
                }

                var renamingPath = project.GetRenamingPath(optionsPath);
                var namingOptionsAndPath = CustomNameOptionsExtensions.TryRead(renamingPath, optionsPath);

                Tuple<bool, string> containsEfCoreReference = null;

                var options = ReverseEngineerOptionsExtensions.TryRead(optionsPath, Path.GetDirectoryName(project.FullPath));

                if (options == null)
                {
                    options = new ReverseEngineerOptions
                    {
                        ProjectRootNamespace = await project.GetAttributeAsync("RootNamespace"),
                    };
                }

                options.ProjectPath = Path.GetDirectoryName(project.FullPath);
                options.OptionsPath = Path.GetDirectoryName(optionsPath);

                bool forceEdit = false;

                if (onlyGenerate)
                {
                    forceEdit = !await ChooseDataBaseConnectionByUiHintAsync(options);

                    if (forceEdit)
                    {
                        await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.DatabaseConnectionNotFoundCannotRefresh);
                    }
                    else
                    {
                        var dbInfo = await GetDatabaseInfoAsync(options);

                        if (dbInfo == null)
                            return;

                        containsEfCoreReference = new Tuple<bool, string>(true, null);
                        options.CustomReplacers = namingOptionsAndPath.Item1;
                        options.InstallNuGetPackage = false;
                    }
                }

                if (!onlyGenerate || forceEdit)
                {
                    if (!await ChooseDataBaseConnectionAsync(options))
                        return;

                    await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.GettingReadyToConnect);

                    var dbInfo = await GetDatabaseInfoAsync(options);

                    if (dbInfo == null)
                        return;

                    VerifySQLServerRightsAndVersion(options);

                    await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.LoadingDatabaseObjects);

                    if (!await LoadDataBaseObjectsAsync(options, dbInfo, namingOptionsAndPath))
                        return;

                    await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.LoadingOptions);

                    containsEfCoreReference = await project.ContainsEfCoreReferenceAsync(options.DatabaseType);
                    options.InstallNuGetPackage = !containsEfCoreReference.Item1;

                    if (!await GetModelOptionsAsync(options, project.Name))
                        return;

                    await SaveOptionsAsync(project, optionsPath, options, new Tuple<List<Schema>, string>(options.CustomReplacers, namingOptionsAndPath.Item2));
                }

                await GenerateFilesAsync(project, options, containsEfCoreReference);

                var nuGetHelper = new NuGetHelper();

                if (options.InstallNuGetPackage && (!onlyGenerate || forceEdit) 
                    && await project.IsNetCore31OrHigherAsync()
                    && containsEfCoreReference != null)
                {
                    await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.InstallingEFCoreProviderPackage);
                       
                    await nuGetHelper.InstallPackageAsync(containsEfCoreReference.Item2, project);
                }

                if (options.Tables.Any(t => t.ObjectType == ObjectType.Procedure)
                    && Properties.Settings.Default.DiscoverMultipleResultSets)
                {
                    await nuGetHelper.InstallPackageAsync("Dapper", project);
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

        private async Task<bool> ChooseDataBaseConnectionByUiHintAsync(ReverseEngineerOptions options)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var databaseList = vsDataHelper.GetDataConnections(_package);
            if (databaseList != null && databaseList.Any())
            {
                var dataBaseInfo = databaseList.Values.FirstOrDefault(m => m.ConnectionName == options.UiHint);
                if (dataBaseInfo != null)
                {
                    options.ConnectionString = dataBaseInfo.ConnectionString;
                    options.DatabaseType = dataBaseInfo.DatabaseType;
                    return true;
                }
            }

            var dacpacList = await EnvDteExtensions.GetDacpacFilesInActiveSolutionAsync();
            if (dacpacList != null && dacpacList.Any())
            { 
                if (!string.IsNullOrEmpty(options.UiHint) 
                    && options.UiHint.EndsWith(".sqlproj", StringComparison.OrdinalIgnoreCase))
                {
                    var candidate = dacpacList
                        .Where(m => !string.IsNullOrWhiteSpace(m) && m.EndsWith(".sqlproj"))
                        .FirstOrDefault(m => m.Equals(options.UiHint, StringComparison.OrdinalIgnoreCase));

                    if (candidate != null)
                    {
                        options.Dacpac = candidate;
                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<bool> ChooseDataBaseConnectionAsync(ReverseEngineerOptions options)
        {
            var databaseList = vsDataHelper.GetDataConnections(_package);
            var dacpacList = await EnvDteExtensions.GetDacpacFilesInActiveSolutionAsync();

            var psd = _package.GetView<IPickServerDatabaseDialog>();

            if (databaseList != null && databaseList.Any())
            {
                psd.PublishConnections(databaseList.Select(m => new DatabaseConnectionModel
                {
                    ConnectionName = m.Value.ConnectionName,
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

            options.CodeGenerationMode = pickDataSourceResult.Payload.CodeGenerationMode;
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

        private async Task<DatabaseConnectionModel> GetDatabaseInfoAsync(ReverseEngineerOptions options)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dbInfo = new DatabaseConnectionModel();

            if (!string.IsNullOrEmpty(options.ConnectionString))
            {
                dbInfo.ConnectionString = options.ConnectionString;
                dbInfo.DatabaseType = options.DatabaseType;
            }

            if (!string.IsNullOrEmpty(options.Dacpac))
            {
                dbInfo.DatabaseType = DatabaseType.SQLServerDacpac;
                dbInfo.ConnectionString = $"Data Source=(local);Initial Catalog={Path.GetFileNameWithoutExtension(options.Dacpac)};Integrated Security=true;";
                options.ConnectionString = dbInfo.ConnectionString;
                options.DatabaseType = dbInfo.DatabaseType;

                options.Dacpac = await EnvDteExtensions.BuildSqlProjAsync(options.Dacpac);
                if (string.IsNullOrEmpty(options.Dacpac))
                {
                    VSHelper.ShowMessage(ReverseEngineerLocale.UnableToBuildSelectedDatabaseProject);
                    return null;
                }
            }

            if (dbInfo.DatabaseType == DatabaseType.Undefined)
            {
                VSHelper.ShowError($"{ReverseEngineerLocale.UnsupportedProvider}");
                return null;
            }

            return dbInfo;
        }

        private async Task<bool> LoadDataBaseObjectsAsync(ReverseEngineerOptions options, DatabaseConnectionModel dbInfo, Tuple<List<Schema>, string> namingOptionsAndPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            await VS.StatusBar.StartAnimationAsync(StatusAnimation.Build);
            var predefinedTables = !string.IsNullOrEmpty(options.Dacpac)
                                       ? await GetDacpacTablesAsync(options.Dacpac, options.CodeGenerationMode)
                                       : await GetTablesAsync(dbInfo, options.CodeGenerationMode, options.Schemas?.ToArray());
            await VS.StatusBar.EndAnimationAsync(StatusAnimation.Build);

            var preselectedTables = new List<SerializationTableModel>();

            if (options.Tables?.Count > 0)
            {
                var normalizedTables = reverseEngineerHelper.NormalizeTables(options.Tables, dbInfo.DatabaseType == DatabaseType.SQLServer);
                preselectedTables.AddRange(normalizedTables);
            }

            await VS.StatusBar.ClearAsync();

            var ptd = _package.GetView<IPickTablesDialog>()
                              .AddTables(predefinedTables, namingOptionsAndPath.Item1)
                              .PreselectTables(preselectedTables);

            var pickTablesResult = ptd.ShowAndAwaitUserResponse(true);

            options.Tables = pickTablesResult.Payload.Objects.ToList();
            options.CustomReplacers = pickTablesResult.Payload.CustomReplacers.ToList();
            return (pickTablesResult.ClosedByOK);
        }

        private async Task<bool> GetModelOptionsAsync(ReverseEngineerOptions options, string projectName)
        {
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
                UseSchemaFolders = options.UseSchemaFolders,
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
                UseNoObjectFilter = options.UseNoObjectFilter,
                UseNoDefaultConstructor = options.UseNoDefaultConstructor,
                UseManyToManyEntity = options.UseManyToManyEntity,
            };

            var modelDialog = _package.GetView<IModelingOptionsDialog>()
                                          .ApplyPresets(presets);
            
            await VS.StatusBar.ClearAsync();

            var modelingOptionsResult = modelDialog.ShowAndAwaitUserResponse(true);

            if (!modelingOptionsResult.ClosedByOK)
                return false;

            options.InstallNuGetPackage = modelingOptionsResult.Payload.InstallNuGetPackage;
            options.UseFluentApiOnly = !modelingOptionsResult.Payload.UseDataAnnotations;
            options.ContextClassName = modelingOptionsResult.Payload.ModelName;
            options.OutputPath = modelingOptionsResult.Payload.OutputPath;
            options.OutputContextPath = modelingOptionsResult.Payload.OutputContextPath;
            options.ContextNamespace = modelingOptionsResult.Payload.ContextNamespace;
            options.UseSchemaFolders = modelingOptionsResult.Payload.UseSchemaFolders;
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
            options.UseNoObjectFilter = modelingOptionsResult.Payload.UseNoObjectFilter;
            options.UseNoDefaultConstructor = modelingOptionsResult.Payload.UseNoDefaultConstructor;
            options.UseManyToManyEntity = modelingOptionsResult.Payload.UseManyToManyEntity;

            return true;
        }

        private void VerifySQLServerRightsAndVersion(ReverseEngineerOptions options)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (options.DatabaseType == DatabaseType.SQLServer && string.IsNullOrEmpty(options.Dacpac))
            {
                if (options.ConnectionString.ToLowerInvariant().Contains(".database.windows.net")
                    && options.ConnectionString.ToLowerInvariant().Contains("active directory interactive"))
                {
                    return;
                }

                var rightsAndVersion = reverseEngineerHelper.HasSqlServerViewDefinitionRightsAndVersion(options.ConnectionString);

                if (!rightsAndVersion.Item1)
                {
                    VSHelper.ShowMessage(ReverseEngineerLocale.SqlServerNoViewDefinitionRights);
                }

                if (rightsAndVersion.Item2.Major < 11)
                {
                    VSHelper.ShowMessage(string.Format(ReverseEngineerLocale.SQLServerVersionNotSupported, rightsAndVersion.Item2));
                }
            }
        }

        private async System.Threading.Tasks.Task GenerateFilesAsync(Project project, ReverseEngineerOptions options, Tuple<bool, string> containsEfCoreReference)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var startTime = DateTime.Now;

            if (options.UseHandleBars)
            {
                DropTemplates(options.OptionsPath, options.CodeGenerationMode);
            }

            options.UseNullableReferences = !await project.IsLegacyAsync() && options.UseNullableReferences;

            await VS.StatusBar.StartAnimationAsync(StatusAnimation.Build);
            await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.GeneratingCode);
            
            var revEngResult = await EfRevEngLauncher.LaunchExternalRunnerAsync(options, options.CodeGenerationMode, project);

            await VS.StatusBar.EndAnimationAsync(StatusAnimation.Build);

            var tfm = await project.GetAttributeAsync("TargetFrameworkMoniker");
            bool isNetStandard = tfm?.Contains(".NETStandard,Version=v2.") ?? false;

            if ((options.SelectedToBeGenerated == 0 || options.SelectedToBeGenerated == 2)
                && !await project.IsNetCore31OrHigherAsync() && !isNetStandard)
            {
                foreach (var filePath in revEngResult.EntityTypeFilePaths)
                {
                    await project.AddExistingFilesAsync(new List<string> { filePath }.ToArray());
                }
            }

            if (options.SelectedToBeGenerated == 0 || options.SelectedToBeGenerated == 1)
            {
                if (!await project.IsNetCore31OrHigherAsync() && !isNetStandard)
                {
                    foreach (var filePath in revEngResult.ContextConfigurationFilePaths)
                    {
                        await project.AddExistingFilesAsync(new List<string> { filePath }.ToArray());
                    }
                    await project.AddExistingFilesAsync(new List<string> { revEngResult.ContextFilePath }.ToArray());
                }

                if (Properties.Settings.Default.OpenGeneratedDbContext)
                {
                    await VS.Documents.OpenAsync(revEngResult.ContextFilePath);
                }
            }

            var duration = DateTime.Now - startTime;

            var missingProviderPackage = containsEfCoreReference.Item1 ? null : containsEfCoreReference.Item2;
            if (options.InstallNuGetPackage || options.SelectedToBeGenerated == 2)
            {
                missingProviderPackage = null;
            }

            await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.ReportingResult);
            var errors = reverseEngineerHelper.ReportRevEngErrors(revEngResult, missingProviderPackage);

            await VS.StatusBar.ShowMessageAsync(string.Format(ReverseEngineerLocale.ReverseEngineerCompleted, duration.ToString("h\\:mm\\:ss")));

            if (errors != ReverseEngineerLocale.ModelGeneratedSuccesfully + Environment.NewLine)
            {
                VSHelper.ShowMessage(errors);
            }

            if (revEngResult.EntityErrors.Count > 0)
            {
                _package.LogError(revEngResult.EntityErrors, null);
            }
            if (revEngResult.EntityWarnings.Count > 0)
            {
                _package.LogError(revEngResult.EntityWarnings, null);
            }
        }

        private async System.Threading.Tasks.Task SaveOptionsAsync(Project project, string optionsPath, ReverseEngineerOptions options, Tuple<List<Schema>, string> renamingOptions)
        {
            if (File.Exists(optionsPath) && File.GetAttributes(optionsPath).HasFlag(FileAttributes.ReadOnly))
            {
                VSHelper.ShowError($"Unable to save options, the file is readonly: {optionsPath}");
                return;
            }

            if (!File.Exists(optionsPath + ".ignore"))
            {
                if (!Properties.Settings.Default.IncludeUiHintInConfig)
                {
                    options.UiHint = null;
                }

                File.WriteAllText(optionsPath, options.Write(Path.GetDirectoryName(project.FullPath)), Encoding.UTF8);

                await project.AddExistingFilesAsync(new List<string> { optionsPath }.ToArray());
            }

            if (renamingOptions.Item1 != null && !File.Exists(renamingOptions.Item2 + ".ignore") && renamingOptions.Item1.Count > 0)
            {
                if (File.Exists(renamingOptions.Item2) && File.GetAttributes(renamingOptions.Item2).HasFlag(FileAttributes.ReadOnly))
                {
                    VSHelper.ShowError($"Unable to save renaming options, the file is readonly: {renamingOptions.Item2}");
                    return;
                }

                File.WriteAllText(renamingOptions.Item2, CustomNameOptionsExtensions.Write(renamingOptions.Item1), Encoding.UTF8);
                await project.AddExistingFilesAsync(new List<string> { renamingOptions.Item2 }.ToArray());
            }
        }

        private void DropTemplates(string path, CodeGenerationMode codeGenerationMode)
        {
            string zipName;
            switch (codeGenerationMode)
            {
                case CodeGenerationMode.EFCore5:
                    zipName = "CodeTemplates502.zip";
                    break;
                case CodeGenerationMode.EFCore3:
                    zipName = "CodeTemplates.zip";
                    break;
                case CodeGenerationMode.EFCore6:
                    zipName = "CodeTemplates600.zip";
                    break;
                default:
                    throw new ArgumentException($"Unsupported code generation mode: {codeGenerationMode}");
            }

            var defaultZip = "CodeTemplates.zip";

            var toDir = Path.Combine(path, "CodeTemplates");
            
            var userTemplateZip = Path.Combine(path, defaultZip);
            var templateZip = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), zipName);

            if (File.Exists(userTemplateZip))
            {
                templateZip = userTemplateZip;
            }

            if (!Directory.Exists(toDir) || IsDirectoryEmpty(toDir))
            {
                Directory.CreateDirectory(toDir);
                ZipFile.ExtractToDirectory(templateZip, toDir);
            }
        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private async Task<List<TableModel>> GetDacpacTablesAsync(string dacpacPath, CodeGenerationMode codeGenerationMode)
        {
            var builder = new TableListBuilder(dacpacPath, DatabaseType.SQLServerDacpac, null);

            return await builder.GetTableDefinitionsAsync(codeGenerationMode);
        }

        private async Task<List<TableModel>> GetTablesAsync(DatabaseConnectionModel dbInfo, CodeGenerationMode codeGenerationMode, SchemaInfo[] schemas)
        {
            if (dbInfo.DataConnection != null)
            {
                dbInfo.DataConnection.Open();
                dbInfo.ConnectionString = DataProtection.DecryptString(dbInfo.DataConnection.EncryptedConnectionString);
            }

            var builder = new TableListBuilder(dbInfo.ConnectionString, dbInfo.DatabaseType, schemas);
            return await builder.GetTableDefinitionsAsync(codeGenerationMode);
        }
    }
}