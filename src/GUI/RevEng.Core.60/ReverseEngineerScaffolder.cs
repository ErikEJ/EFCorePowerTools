using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using RevEng.Common;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Functions;
using RevEng.Core.Procedures;

namespace RevEng.Core
{
    public class ReverseEngineerScaffolder : IReverseEngineerScaffolder
    {
        private readonly IDatabaseModelFactory databaseModelFactory;
        private readonly IScaffoldingModelFactory factory;
        private readonly ICSharpHelper code;
        private readonly IFunctionScaffolder functionScaffolder;
        private readonly IFunctionModelFactory functionModelFactory;
        private readonly IProcedureScaffolder procedureScaffolder;
        private readonly IProcedureModelFactory procedureModelFactory;

        public ReverseEngineerScaffolder(
            IDatabaseModelFactory databaseModelFactory,
            IScaffoldingModelFactory scaffoldingModelFactory,
            IFunctionScaffolder functionScaffolder,
            IFunctionModelFactory functionModelFactory,
            IProcedureScaffolder procedureScaffolder,
            IProcedureModelFactory procedureModelFactory,
            IModelCodeGeneratorSelector modelCodeGeneratorSelector,
            ICSharpHelper cSharpHelper)
        {
            this.databaseModelFactory = databaseModelFactory;
            factory = scaffoldingModelFactory;
            code = cSharpHelper;
            this.functionScaffolder = functionScaffolder;
            this.functionModelFactory = functionModelFactory;
            this.procedureScaffolder = procedureScaffolder;
            this.procedureModelFactory = procedureModelFactory;
            ModelCodeGeneratorSelector = modelCodeGeneratorSelector;
        }

        private IModelCodeGeneratorSelector ModelCodeGeneratorSelector { get; }

        public SavedModelFiles GenerateDbContext(
            ReverseEngineerCommandOptions options,
            List<string> schemas,
            string outputContextDir,
            string modelNamespace,
            string contextNamespace,
            string projectPath,
            string outputPath)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            SavedModelFiles filePaths;
            var modelOptions = new ModelReverseEngineerOptions
            {
                UseDatabaseNames = options.UseDatabaseNames,
#if CORE60
                NoPluralize = !options.UseInflector,
#endif
            };

            var codeOptions = new ModelCodeGenerationOptions
            {
                UseDataAnnotations = !options.UseFluentApiOnly,
                Language = "C#",

                ContextName = code.Identifier(options.ContextClassName),
                ContextDir = outputContextDir,
                RootNamespace = null,
                ContextNamespace = contextNamespace,
                ModelNamespace = modelNamespace,
                SuppressConnectionStringWarning = false,
                ConnectionString = options.ConnectionString,
#if CORE60
                SuppressOnConfiguring = !options.IncludeConnectionString,
#endif
#if CORE60
                UseNullableReferenceTypes = options.UseNullableReferences,
#endif
#if CORE70
                ProjectDir = options.UseT4 ? projectPath : null,
#endif
            };

            var dbOptions = new DatabaseModelFactoryOptions(options.Tables.Where(t => t.ObjectType.HasColumns()).Select(m => m.Name), schemas);

            var scaffoldedModel = ScaffoldModel(
                    options.Dacpac ?? options.ConnectionString,
                    dbOptions,
                    modelOptions,
                    codeOptions,
                    options.UseBoolPropertiesWithoutDefaultSql,
                    options.SelectedToBeGenerated == 1, // DbContext only
                    options.SelectedToBeGenerated == 2, // Entities only
                    options.UseSchemaFolders,
                    options.UseSchemaNamespaces);

            filePaths = Save(
                scaffoldedModel,
                Path.GetFullPath(Path.Combine(options.ProjectPath, outputPath ?? string.Empty)));
            return filePaths;
        }

        public SavedModelFiles GenerateFunctions(
            ReverseEngineerCommandOptions options,
            ref List<string> errors,
            string outputContextDir,
            string modelNamespace,
            string contextNamespace,
            bool supportsFunctions)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var functionModelScaffolder = functionScaffolder;
            if (functionModelScaffolder != null
                && supportsFunctions
                && (options.Tables.Any(t => t.ObjectType == ObjectType.ScalarFunction)
                    || !options.Tables.Any()))
            {
                var modelFactoryOptions = new ModuleModelFactoryOptions
                {
                    FullModel = true,
                    Modules = options.Tables.Where(t => t.ObjectType == ObjectType.ScalarFunction).Select(m => m.Name),
                };

                var functionModel = functionModelFactory.Create(options.Dacpac ?? options.ConnectionString, modelFactoryOptions);

                ApplyRenamers(functionModel.Routines, options.CustomReplacers);

                var functionOptions = new ModuleScaffolderOptions
                {
                    ContextDir = outputContextDir,
                    ContextName = options.ContextClassName,
                    ContextNamespace = contextNamespace,
                    ModelNamespace = modelNamespace,
                    NullableReferences = options.UseNullableReferences,
                    UseAsyncCalls = options.UseAsyncCalls,
                };

                var functionScaffoldedModel = functionModelScaffolder.ScaffoldModel(functionModel, functionOptions, ref errors);

                if (functionScaffoldedModel != null)
                {
                    return functionModelScaffolder.Save(
                        functionScaffoldedModel,
                        Path.GetFullPath(Path.Combine(options.ProjectPath, options.OutputPath ?? string.Empty)),
                        contextNamespace,
                        options.UseAsyncCalls);
                }
            }

            return null;
        }

        public SavedModelFiles GenerateStoredProcedures(
            ReverseEngineerCommandOptions options,
            ref List<string> errors,
            string outputContextDir,
            string modelNamespace,
            string contextNamespace,
            bool supportsProcedures)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var procedureModelScaffolder = procedureModelFactory;
            if (procedureModelScaffolder != null
                && supportsProcedures
                && (options.Tables.Any(t => t.ObjectType == ObjectType.Procedure)
                    || !options.Tables.Any()))
            {
                var procedureModelFactoryOptions = new ModuleModelFactoryOptions
                {
                    DiscoverMultipleResultSets = options.UseMultipleSprocResultSets,
                    UseLegacyResultSetDiscovery = options.UseLegacyResultSetDiscovery && !options.UseMultipleSprocResultSets,
                    UseDateOnlyTimeOnly = options.UseDateOnlyTimeOnly,
                    FullModel = true,
                    Modules = options.Tables.Where(t => t.ObjectType == ObjectType.Procedure).Select(m => m.Name),
                    ModulesUsingLegacyDiscovery = options.Tables
                        .Where(t => t.ObjectType == ObjectType.Procedure && t.UseLegacyResultSetDiscovery)
                        .Select(m => m.Name),
                    MappedModules = options.Tables
                        .Where(t => t.ObjectType == ObjectType.Procedure && !string.IsNullOrEmpty(t.MappedType))
                        .Select(m => new { m.Name, m.MappedType })
                        .ToDictionary(m => m.Name, m => m.MappedType),
                };

                var procedureModel = procedureModelFactory.Create(options.Dacpac ?? options.ConnectionString, procedureModelFactoryOptions);

                ApplyRenamers(procedureModel.Routines, options.CustomReplacers);

                var procedureOptions = new ModuleScaffolderOptions
                {
                    ContextDir = outputContextDir,
                    ContextName = options.ContextClassName,
                    ContextNamespace = contextNamespace,
                    ModelNamespace = modelNamespace,
                    NullableReferences = options.UseNullableReferences,
                    UseSchemaFolders = options.UseSchemaFolders,
                    UseAsyncCalls = options.UseAsyncCalls,
                    UseSchemaNamespaces = options.UseSchemaNamespaces,
                };

                var procedureScaffoldedModel = procedureScaffolder.ScaffoldModel(procedureModel, procedureOptions, ref errors);

                if (procedureScaffoldedModel != null)
                {
                    return procedureScaffolder.Save(
                        procedureScaffoldedModel,
                        Path.GetFullPath(Path.Combine(options.ProjectPath, options.OutputPath ?? string.Empty)),
                        contextNamespace,
                        options.UseAsyncCalls);
                }
            }

            return null;
        }

        private static SavedModelFiles Save(
           ScaffoldedModel scaffoldedModel,
           string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            var contextPath = string.Empty;

            if (scaffoldedModel.ContextFile != null)
            {
                contextPath = Path.GetFullPath(Path.Combine(outputDir, scaffoldedModel.ContextFile!.Path));
                Directory.CreateDirectory(Path.GetDirectoryName(contextPath)!);
                File.WriteAllText(contextPath, scaffoldedModel.ContextFile.Code, Encoding.UTF8);
            }

            var additionalFiles = new List<string>();
            foreach (var entityTypeFile in scaffoldedModel.AdditionalFiles)
            {
                var additionalFilePath = Path.Combine(outputDir, entityTypeFile.Path);
                if (additionalFilePath != null)
                {
                    var path = Path.GetDirectoryName(additionalFilePath);
                    if (path != null)
                    {
                        Directory.CreateDirectory(path);
                        File.WriteAllText(additionalFilePath, entityTypeFile.Code, Encoding.UTF8);
                        additionalFiles.Add(additionalFilePath);
                    }
                }
            }

            return new SavedModelFiles(contextPath, additionalFiles);
        }

        private static void ApplyRenamers(IEnumerable<SqlObjectBase> sqlObjects, List<Schema> renamers)
        {
            if (renamers == null || !renamers.Any())
            {
                return;
            }

            foreach (var sqlObject in sqlObjects)
            {
                var schema = renamers
                    .FirstOrDefault(x => x.SchemaName == sqlObject.Schema);

                if (schema != null)
                {
                    if (schema.Tables != null && schema.Tables.Any(t => t.Name == sqlObject.Name))
                    {
                        sqlObject.NewName = schema.Tables.SingleOrDefault(t => t.Name == sqlObject.Name)?.NewName;
                    }
                    else if (!string.IsNullOrEmpty(schema.TableRegexPattern) && schema.TablePatternReplaceWith != null)
                    {
                        sqlObject.NewName = RegexNameReplace(schema.TableRegexPattern, sqlObject.Name, schema.TablePatternReplaceWith);
                    }
                }
            }
        }

        private static string RegexNameReplace(string pattern, string originalName, string replacement, int timeout = 100)
        {
            string newName = string.Empty;

            try
            {
                newName = Regex.Replace(originalName, pattern, replacement, RegexOptions.None, TimeSpan.FromMilliseconds(timeout));
            }
            catch (RegexMatchTimeoutException)
            {
                Console.WriteLine($"Regex pattern {pattern} time out when trying to match {originalName}, name won't be replaced");
            }

            return newName;
        }

        private static void AppendSchemaFoldersAndNamespace(IModel databaseModel, ScaffoldedModel scaffoldedModel, bool useSchemaFolders, bool useSchemaNamespaces, IEnumerable<string> schemas)
        {
            // Tables and views only
            if (!useSchemaFolders && !useSchemaNamespaces)
            {
                return;
            }

            if (useSchemaNamespaces)
            {
                scaffoldedModel.ContextFile.Code = AppendSchemaNamespace(string.Empty, scaffoldedModel.ContextFile.Code, schemas);
            }

            foreach (var entityType in scaffoldedModel.AdditionalFiles)
            {
                var entityTypeName = Path.GetFileNameWithoutExtension(entityType.Path);
                var entityTypeExtension = Path.GetExtension(entityType.Path);
                var entityMatch = databaseModel.GetEntityTypes().FirstOrDefault(x => x.Name == entityTypeName);
                var entityTypeSchema = entityMatch?.GetSchema();
#if CORE60
                if (entityMatch?.GetViewName() != null)
                {
                    entityTypeSchema = entityMatch?.GetViewSchema();
                }
#endif
                if (!string.IsNullOrEmpty(entityTypeSchema))
                {
                    if (useSchemaFolders)
                    {
                        entityType.Path = Path.Combine(entityTypeSchema, entityTypeName + entityTypeExtension);
                    }

                    if (useSchemaNamespaces)
                    {
                        entityType.Code = AppendSchemaNamespace(entityTypeSchema, entityType.Code, schemas);
                    }
                }
            }
        }

        private static string AppendSchemaNamespace(string entityTypeSchema, string code, IEnumerable<string> schemas)
        {
            var nameSpaceSuffix = "Schema";
            var entityTypeSchemaWithSuffix = entityTypeSchema + nameSpaceSuffix;
            var namespaceKeyWord = "namespace ";
            var usingKeyWord = "using ";
            var codeLines = code.Split(new string[] { "\r\n", "\r" }, StringSplitOptions.None);
            var originalNameSpaceLine = codeLines.Single(l => l.StartsWith(namespaceKeyWord, StringComparison.Ordinal));
            var newNameSpaceLine = originalNameSpaceLine;
            var cSharp10NameSpaceStyle = newNameSpaceLine.EndsWith(";", StringComparison.Ordinal);
            if (cSharp10NameSpaceStyle)
            {
                newNameSpaceLine = newNameSpaceLine.Substring(0, newNameSpaceLine.Length - 1);
            }

            var originalLastUsing = codeLines.Last(l => l.StartsWith(usingKeyWord, StringComparison.Ordinal));
            var regexStartsWithNameSpace = new Regex(Regex.Escape(namespaceKeyWord));
            var newUsings = new StringBuilder(originalLastUsing);
            newUsings.AppendLine();
            foreach (var schema in schemas.Where(s => s != entityTypeSchemaWithSuffix).OrderBy(s => s))
            {
                var newUsing = regexStartsWithNameSpace.Replace(newNameSpaceLine, usingKeyWord, 1);
                newUsings.Append(newUsing);
                newUsings.Append('.');
                newUsings.Append(schema);
                newUsings.Append(nameSpaceSuffix);
                newUsings.AppendLine(";");
            }

            newNameSpaceLine = newNameSpaceLine + $".{entityTypeSchemaWithSuffix}";
            if (cSharp10NameSpaceStyle)
            {
                newNameSpaceLine += ";";
            }

            var sb = new StringBuilder();
            foreach (var codeLine in codeLines)
            {
                if (codeLine.Equals(originalNameSpaceLine, StringComparison.Ordinal) && !string.IsNullOrEmpty(entityTypeSchema))
                {
                    sb.AppendLine(newNameSpaceLine);
                }
                else if (codeLine.Equals(originalLastUsing, StringComparison.Ordinal))
                {
                    sb.AppendLine(newUsings.ToString());
                }
                else
                {
                    sb.AppendLine(codeLine);
                }
            }

            return sb.ToString();
        }

        private ScaffoldedModel ScaffoldModel(
            string connectionString,
            DatabaseModelFactoryOptions databaseOptions,
            ModelReverseEngineerOptions modelOptions,
            ModelCodeGenerationOptions codeOptions,
            bool removeNullableBoolDefaults,
            bool dbContextOnly,
            bool entitiesOnly,
            bool useSchemaFolders,
            bool useSchemaNamespaces)
        {
            var databaseModel = databaseModelFactory.Create(connectionString, databaseOptions);

            if (removeNullableBoolDefaults)
            {
                foreach (var column in databaseModel.Tables
                    .SelectMany(table => table.Columns
                        .Where(column => ((column.StoreType == "bit" || column.StoreType == "boolean")
                            && !column.IsNullable
                            && !string.IsNullOrEmpty(column.DefaultValueSql))
                            ||

                            // Oracle
                            (column.StoreType == "NUMBER(1)" && !column.IsNullable
                                && !string.IsNullOrEmpty(column.DefaultValueSql)
                                && (column.DefaultValueSql?.Trim() == "1" || column.DefaultValueSql?.Trim() == "0")))))
                {
                    column.DefaultValueSql = null;
                }
            }

#if CORE60
            var model = factory.Create(databaseModel, modelOptions);
#else
            var model = factory.Create(databaseModel, modelOptions.UseDatabaseNames);
#endif
            if (model == null)
            {
                throw new InvalidOperationException($"No model from provider {factory.GetType().ShortDisplayName()}");
            }
#if CORE70
            var codeGenerator = ModelCodeGeneratorSelector.Select(codeOptions);
#else
            var codeGenerator = ModelCodeGeneratorSelector.Select(codeOptions.Language);
#endif
            var codeModel = codeGenerator.GenerateModel(model, codeOptions);
            if (entitiesOnly)
            {
                codeModel.ContextFile = null!;
            }

            if (dbContextOnly)
            {
                codeModel.AdditionalFiles.Clear();
            }

            AppendSchemaFoldersAndNamespace(model, codeModel, useSchemaFolders, useSchemaNamespaces, databaseModel.Tables.Select(t => t.Schema).Distinct());

            return codeModel;
        }
    }
}
