using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.Models;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    internal class ReverseEngineerHandler
    {
        private readonly EFCorePowerToolsPackage package;
        private readonly ReverseEngineerHelper reverseEngineerHelper;
        private readonly VsDataHelper vsDataHelper;
        private List<string> legacyDiscoveryObjects = new List<string>();
        private Dictionary<string, string> mappedTypes = new Dictionary<string, string>();

        public ReverseEngineerHandler(EFCorePowerToolsPackage package)
        {
            this.package = package;
            reverseEngineerHelper = new ReverseEngineerHelper();
            vsDataHelper = new VsDataHelper();
        }

        public async System.Threading.Tasks.Task<(string OptionsPath, Project Project)> DropSqlprojOptionsAsync(List<Project> candidateProjects, string sqlProjectPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var project = candidateProjects.First();

            if (candidateProjects.Count > 1)
            {
                var pcd = package.GetView<IPickProjectDialog>();
                pcd.PublishProjects(candidateProjects.Select(m => new ProjectModel
                {
                    Project = m,
                }));

                var pickProjectResult = pcd.ShowAndAwaitUserResponse(true);
                if (!pickProjectResult.ClosedByOK)
                {
                    return (null, null);
                }

                project = pickProjectResult.Payload.Project;
            }

            var optionsPaths = project.GetConfigFiles();
            var optionsPath = optionsPaths.First();

            if (File.Exists(optionsPath))
            {
                return (null, project);
            }

            var options = new ReverseEngineerOptions
            {
                Tables = new List<SerializationTableModel>(),
                CodeGenerationMode = CodeGenerationMode.EFCore6,
                DatabaseType = DatabaseType.SQLServerDacpac,
                UiHint = sqlProjectPath,
            };

            await SaveOptionsAsync(project, optionsPath, options, new Tuple<List<Schema>, string>(null, null));

            return (optionsPath, project);
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
                    var pcd = package.GetView<IPickConfigDialog>();
                    pcd.PublishConfigurations(optionsPaths.Select(m => new ConfigModel
                    {
                        ConfigPath = m,
                        ProjectPath = projectPath,
                    }));

                    var pickConfigResult = pcd.ShowAndAwaitUserResponse(true);
                    if (!pickConfigResult.ClosedByOK)
                    {
                        return;
                    }

                    optionsPath = pickConfigResult.Payload.ConfigPath;
                }

                await ReverseEngineerCodeFirstAsync(project, optionsPath, false);
            }
            catch (AggregateException ae)
            {
                foreach (var innerException in ae.Flatten().InnerExceptions)
                {
                    package.LogError(new List<string>(), innerException);
                }
            }
            catch (Exception exception)
            {
                package.LogError(new List<string>(), exception);
            }
        }

        public async System.Threading.Tasks.Task ReverseEngineerCodeFirstAsync(Project project, string optionsPath, bool onlyGenerate, bool fromSqlProj = false)
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

                legacyDiscoveryObjects = options.Tables?.Where(t => t.UseLegacyResultSetDiscovery).Select(t => t.Name).ToList() ?? new List<string>();
                mappedTypes = options.Tables?
                    .Where(t => !string.IsNullOrEmpty(t.MappedType) && t.ObjectType == ObjectType.Procedure)
                    .Select(m => new { m.Name, m.MappedType }).ToDictionary(m => m.Name, m => m.MappedType) ?? new Dictionary<string, string>();

                options.ProjectPath = Path.GetDirectoryName(project.FullPath);
                options.OptionsPath = Path.GetDirectoryName(optionsPath);

                bool forceEdit = false;

                DatabaseConnectionModel dbInfo = null;

                if (onlyGenerate || fromSqlProj)
                {
                    forceEdit = !await ChooseDataBaseConnectionByUiHintAsync(options);

                    if (forceEdit)
                    {
                        await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.DatabaseConnectionNotFoundCannotRefresh);
                    }
                    else
                    {
                        dbInfo = await GetDatabaseInfoAsync(options);

                        if (dbInfo == null)
                        {
                            return;
                        }

                        containsEfCoreReference = new Tuple<bool, string>(true, null);
                        options.CustomReplacers = namingOptionsAndPath.Item1;
                        options.InstallNuGetPackage = !onlyGenerate;
                    }
                }

                if (!onlyGenerate || forceEdit)
                {
                    if (!fromSqlProj)
                    {
                        if (!await ChooseDataBaseConnectionAsync(options, project))
                        {
                            return;
                        }

                        await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.GettingReadyToConnect);

                        dbInfo = await GetDatabaseInfoAsync(options);

                        if (dbInfo == null)
                        {
                            return;
                        }

                        VerifySQLServerRightsAndVersion(options);
                    }

                    await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.LoadingDatabaseObjects);

                    if (!await LoadDataBaseObjectsAsync(options, dbInfo, namingOptionsAndPath))
                    {
                        return;
                    }

                    await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.LoadingOptions);

                    containsEfCoreReference = await project.ContainsEfCoreReferenceAsync(options.DatabaseType);
                    options.InstallNuGetPackage = !containsEfCoreReference.Item1;

                    if (!await GetModelOptionsAsync(options, project.Name))
                    {
                        return;
                    }

                    await SaveOptionsAsync(project, optionsPath, options, new Tuple<List<Schema>, string>(options.CustomReplacers, namingOptionsAndPath.Item2));
                }

                await GenerateFilesAsync(project, options, containsEfCoreReference);

                await InstallNuGetPackagesAsync(project, onlyGenerate, containsEfCoreReference, options, forceEdit);

                var postRunFile = Path.Combine(Path.GetDirectoryName(optionsPath), "efpt.postrun.cmd");
                if (File.Exists(postRunFile))
                {
                    Process.Start($"\"{postRunFile}\"");
                }

                Telemetry.TrackEvent("PowerTools.ReverseEngineer");
            }
            catch (AggregateException ae)
            {
                foreach (var innerException in ae.Flatten().InnerExceptions)
                {
                    package.LogError(new List<string>(), innerException);
                }
            }
            catch (Exception exception)
            {
                package.LogError(new List<string>(), exception);
            }
        }

        private static async System.Threading.Tasks.Task InstallNuGetPackagesAsync(Project project, bool onlyGenerate, Tuple<bool, string> containsEfCoreReference, ReverseEngineerOptions options, bool forceEdit)
        {
            var nuGetHelper = new NuGetHelper();

            if (options.InstallNuGetPackage && (!onlyGenerate || forceEdit)
                && await project.IsNetCore31OrHigherAsync()
                && containsEfCoreReference != null)
            {
                await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.InstallingEFCoreProviderPackage);

                await nuGetHelper.InstallPackageAsync(containsEfCoreReference.Item2, project);
            }

            if (options.Tables.Any(t => t.ObjectType == ObjectType.Procedure)
                && AdvancedOptions.Instance.DiscoverMultipleResultSets)
            {
                await nuGetHelper.InstallPackageAsync("Dapper", project);
            }
        }

        private async Task<bool> ChooseDataBaseConnectionByUiHintAsync(ReverseEngineerOptions options)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var databaseList = vsDataHelper.GetDataConnections(package);
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

            var dacpacList = await SqlProjHelper.GetDacpacFilesInActiveSolutionAsync();
            if (dacpacList != null && dacpacList.Any() && !string.IsNullOrEmpty(options.UiHint)
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

            return false;
        }

        private async Task<bool> ChooseDataBaseConnectionAsync(ReverseEngineerOptions options, Project project)
        {
            var databaseList = vsDataHelper.GetDataConnections(package);
            var dacpacList = await SqlProjHelper.GetDacpacFilesInActiveSolutionAsync();

            var psd = package.GetView<IPickServerDatabaseDialog>();

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
                    FilePath = m,
                }));
            }

            if (options.FilterSchemas && options.Schemas != null && options.Schemas.Any())
            {
                psd.PublishSchemas(options.Schemas);
            }

            var (usedMode, allowedVersions) = reverseEngineerHelper.CalculateAllowedVersions(options.CodeGenerationMode, await project.GetEFCoreVersionHintAsync());

            psd.PublishCodeGenerationMode(usedMode, allowedVersions);

            if (!string.IsNullOrEmpty(options.UiHint))
            {
                psd.PublishUiHint(options.UiHint);
            }

            var pickDataSourceResult = psd.ShowAndAwaitUserResponse(true);
            if (!pickDataSourceResult.ClosedByOK)
            {
                return false;
            }

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

                options.Dacpac = await SqlProjHelper.BuildSqlProjAsync(options.Dacpac);
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

            var isSqliteToolboxInstalled = options.DatabaseType != DatabaseType.SQLite;

            await VS.StatusBar.EndAnimationAsync(StatusAnimation.Build);

            var preselectedTables = new List<SerializationTableModel>();

            if (options.Tables?.Count > 0)
            {
                var normalizedTables = reverseEngineerHelper.NormalizeTables(options.Tables, dbInfo.DatabaseType == DatabaseType.SQLServer);
                preselectedTables.AddRange(normalizedTables);
            }

            await VS.StatusBar.ClearAsync();

            var ptd = package.GetView<IPickTablesDialog>()
                              .AddTables(predefinedTables, namingOptionsAndPath.Item1)
                              .PreselectTables(preselectedTables)
                              .SqliteToolboxInstall(isSqliteToolboxInstalled);

            var pickTablesResult = ptd.ShowAndAwaitUserResponse(true);

            options.Tables = pickTablesResult.Payload.Objects.ToList();
            options.CustomReplacers = pickTablesResult.Payload.CustomReplacers.ToList();
            return pickTablesResult.ClosedByOK;
        }

        private async Task<bool> GetModelOptionsAsync(ReverseEngineerOptions options, string projectName)
        {
            var classBasis = VsDataHelper.GetDatabaseName(options.ConnectionString, options.DatabaseType);

            if (string.IsNullOrEmpty(options.ContextClassName))
            {
                options.ContextClassName = classBasis + "Context";
            }

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
                UseHandlebars = options.UseHandleBars || options.UseT4,
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
                MapHierarchyId = options.UseHierarchyId,
                MapNodaTimeTypes = options.UseNodaTime,
                UseBoolPropertiesWithoutDefaultSql = options.UseBoolPropertiesWithoutDefaultSql,
                UseNoConstructor = options.UseNoConstructor,
                UseNoNavigations = options.UseNoNavigations,
                UseNullableReferences = options.UseNullableReferences,
                UseNoObjectFilter = options.UseNoObjectFilter,
                UseNoDefaultConstructor = options.UseNoDefaultConstructor,
                UseManyToManyEntity = options.UseManyToManyEntity,
            };

            var modelDialog = package.GetView<IModelingOptionsDialog>()
                                          .ApplyPresets(presets);

            await VS.StatusBar.ClearAsync();

            var modelingOptionsResult = modelDialog.ShowAndAwaitUserResponse(true);

            if (!modelingOptionsResult.ClosedByOK)
            {
                return false;
            }

            var isHandleBarsLanguage = modelingOptionsResult.Payload.SelectedHandlebarsLanguage == 0
                || modelingOptionsResult.Payload.SelectedHandlebarsLanguage == 1;
            options.UseHandleBars = modelingOptionsResult.Payload.UseHandlebars && isHandleBarsLanguage;
            options.SelectedHandlebarsLanguage = modelingOptionsResult.Payload.SelectedHandlebarsLanguage;
            options.UseT4 = modelingOptionsResult.Payload.UseHandlebars && !isHandleBarsLanguage;

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
            options.UseHierarchyId = modelingOptionsResult.Payload.MapHierarchyId;
            options.UseNodaTime = modelingOptionsResult.Payload.MapNodaTimeTypes;
            options.UseDbContextSplitting = modelingOptionsResult.Payload.UseDbContextSplitting;
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
                    && (options.ConnectionString.ToLowerInvariant().Contains("active directory interactive")
                        || options.ConnectionString.ToLowerInvariant().Contains("active directory default")))
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

            await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 1, 4);

            var startTime = DateTime.Now;

            if (options.UseHandleBars || options.UseT4)
            {
                reverseEngineerHelper.DropTemplates(options.OptionsPath, options.ProjectPath, options.CodeGenerationMode, options.UseHandleBars);
            }

            options.UseNullableReferences = !await project.IsLegacyAsync() && options.UseNullableReferences;

            await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 2, 4);

            var revEngResult = await EfRevEngLauncher.LaunchExternalRunnerAsync(options, options.CodeGenerationMode, project);

            await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 3, 4);

            var tfm = await project.GetAttributeAsync("TargetFrameworkMoniker");
            bool isNetStandard = tfm?.Contains(".NETStandard,Version=v2.") ?? false;

            if ((options.SelectedToBeGenerated == 0 || options.SelectedToBeGenerated == 2)
                && !await project.IsNetCore31OrHigherIncluding70Async() && !isNetStandard)
            {
                foreach (var filePath in revEngResult.EntityTypeFilePaths)
                {
                    await project.AddExistingFilesAsync(new List<string> { filePath }.ToArray());
                }
            }

            if (options.SelectedToBeGenerated == 0 || options.SelectedToBeGenerated == 1)
            {
                if (!await project.IsNetCore31OrHigherIncluding70Async() && !isNetStandard)
                {
                    foreach (var filePath in revEngResult.ContextConfigurationFilePaths)
                    {
                        await project.AddExistingFilesAsync(new List<string> { filePath }.ToArray());
                    }

                    await project.AddExistingFilesAsync(new List<string> { revEngResult.ContextFilePath }.ToArray());
                }

                if (AdvancedOptions.Instance.OpenGeneratedDbContext)
                {
                    var readmeName = "PowerToolsReadMe.txt";
                    var finalText = reverseEngineerHelper.GetReadMeText(options, File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), readmeName), Encoding.UTF8));
                    var readmePath = Path.Combine(Path.GetTempPath(), readmeName);

                    File.WriteAllText(readmePath, finalText, Encoding.UTF8);

                    await VS.Documents.OpenAsync(readmePath);

                    await VS.Documents.OpenAsync(revEngResult.ContextFilePath);
                }
            }

            await VS.StatusBar.ShowProgressAsync(ReverseEngineerLocale.GeneratingCode, 4, 4);

            var duration = DateTime.Now - startTime;

            var missingProviderPackage = containsEfCoreReference.Item1 ? null : containsEfCoreReference.Item2;
            if (options.InstallNuGetPackage || options.SelectedToBeGenerated == 2)
            {
                missingProviderPackage = null;
            }

            var errors = reverseEngineerHelper.ReportRevEngErrors(revEngResult, missingProviderPackage);

            await VS.StatusBar.ShowMessageAsync(string.Format(ReverseEngineerLocale.ReverseEngineerCompleted, duration.ToString("h\\:mm\\:ss")));

            if (errors != ReverseEngineerLocale.ModelGeneratedSuccesfully + Environment.NewLine)
            {
                VSHelper.ShowMessage(errors);
            }

            if (revEngResult.EntityErrors.Count > 0)
            {
                package.LogError(revEngResult.EntityErrors, null);
            }

            if (revEngResult.EntityWarnings.Count > 0)
            {
                package.LogError(revEngResult.EntityWarnings, null);
            }

            Telemetry.TrackFrameworkUse(nameof(ReverseEngineerHandler), options.CodeGenerationMode);
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
                if (!AdvancedOptions.Instance.IncludeUiHintInConfig)
                {
                    options.UiHint = null;
                }

                foreach (var table in options.Tables)
                {
                    if (legacyDiscoveryObjects.Contains(table.Name))
                    {
                        table.UseLegacyResultSetDiscovery = true;
                    }

                    if (mappedTypes.ContainsKey(table.Name))
                    {
                        table.MappedType = mappedTypes[table.Name];
                    }
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
