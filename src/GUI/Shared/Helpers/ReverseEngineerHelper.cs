﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.Locales;
using RevEng.Common;

namespace EFCorePowerTools.Helpers
{
    public class ReverseEngineerHelper
    {
        public List<SerializationTableModel> NormalizeTables(List<SerializationTableModel> tables, bool shouldFix)
        {
            var result = new List<SerializationTableModel>();
            foreach (var table in tables)
            {
                if (shouldFix && !table.Name.StartsWith("[", StringComparison.OrdinalIgnoreCase))
                {
                    table.Name = ReplaceFirst(table.Name, ".", "].[");
                    table.Name = "[" + table.Name + "]";
                }

                result.Add(table);
            }

            return result;
        }

        public List<string> AddSuggestedMappings(ReverseEngineerOptions options, List<TableModel> tables)
        {
            var result = new List<string>();

            if (options.CodeGenerationMode == CodeGenerationMode.EFCore6
                || options.CodeGenerationMode == CodeGenerationMode.EFCore7)
            {
                if (!options.UseHierarchyId && tables.Exists(t => t.Columns != null && t.Columns.Any(c => c.StoreType == "hierarchyid"))
                    && (options.DatabaseType == DatabaseType.SQLServerDacpac || options.DatabaseType == DatabaseType.SQLServer))
                {
                    result.Add("Your database schema contains one or more 'hierarchyid' columns, but you have not enabled them to be mapped.");
                }

                if (!options.UseSpatial && tables.Exists(t => t.Columns != null && t.Columns.Any(c => c.StoreType == "geometry" || c.StoreType == "geography")))
                {
                    result.Add("Your database schema contains one or more 'geometry' or 'geography' columns, but you have not enabled them to be mapped.");
                }

                if (!options.UseDateOnlyTimeOnly && tables.Exists(t => t.Columns != null && t.Columns.Any(c => c.StoreType == "date" || c.StoreType == "time"))
                    && (options.DatabaseType == DatabaseType.SQLServerDacpac || options.DatabaseType == DatabaseType.SQLServer))
                {
                    result.Add("Your database schema contains one or more 'date' or 'time' columns, but you have not enabled them to be mapped to TimeOnly/DateOnly.");
                }
            }

            return result;
        }

        public void DropT4Templates(string projectPath)
        {
            DropTemplates(projectPath, projectPath, CodeGenerationMode.EFCore7, false);
        }

        public string DropTemplates(string optionsPath, string projectPath, CodeGenerationMode codeGenerationMode, bool useHandlebars, int selectedOption = 0)
        {
            string zipName;

            const string T4Version = "703";

            if (useHandlebars)
            {
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore6:
                        zipName = "CodeTemplates600.zip";
                        break;
                    case CodeGenerationMode.EFCore7:
                        zipName = "CodeTemplates700.zip";
                        break;
                    default:
                        throw new ArgumentException($"Unsupported code generation mode for templates: {codeGenerationMode}");
                }
            }
            else
            {
                if (codeGenerationMode == CodeGenerationMode.EFCore7)
                {
                    zipName = $"T4_{T4Version}.zip";
                }
                else
                {
                    throw new ArgumentException($"Unsupported code generation mode for T4 templates: {codeGenerationMode}");
                }
            }

            var defaultZip = "CodeTemplates.zip";
            var userTemplateZip = Path.Combine(optionsPath, defaultZip);

            var toDir = useHandlebars ? Path.Combine(optionsPath, "CodeTemplates") : Path.Combine(projectPath, "CodeTemplates");
            var templateZip = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), zipName);

            if (File.Exists(userTemplateZip) && useHandlebars)
            {
                templateZip = userTemplateZip;
            }

            if (!Directory.Exists(toDir) || IsDirectoryEmpty(toDir))
            {
                Directory.CreateDirectory(toDir);
                ZipFile.ExtractToDirectory(templateZip, toDir);

                // Using POCO Templates
                if (selectedOption == 3)
                {
                    var pocoT4 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "POCOEntityType.t4");
                    var target = Path.Combine(toDir, "EFCore", "EntityType.t4");
                    File.Copy(pocoT4, target, true);
                }
            }

            if (!useHandlebars && Directory.Exists(toDir))
            {
                var error = $"The latest T4 template version could not be found, looking for 'Template version: {T4Version}' in the T4 file - please update your T4 templates, for example by renaming the CodeTemplates folder.";
                var check = $"Template version: {T4Version}";

                var target = Path.Combine(toDir, "EFCore", "EntityType.t4");
                if (File.Exists(target))
                {
                    var content = File.ReadAllText(target, Encoding.UTF8);
                    if (content.IndexOf(check, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        return error;
                    }
                }

                target = Path.Combine(toDir, "EFCore", "DbContext.t4");
                if (File.Exists(target))
                {
                    var content = File.ReadAllText(target, Encoding.UTF8);
                    if (content.IndexOf(check, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        return error;
                    }
                }
            }

            return string.Empty;
        }

        public (CodeGenerationMode UsedMode, IList<CodeGenerationItem> AllowedVersions) CalculateAllowedVersions(CodeGenerationMode codeGenerationMode, Version minimumVersion)
        {
            var list = new List<CodeGenerationItem>();

            if (minimumVersion.Major == 6 || minimumVersion.Major == 2)
            {
                list.Add(new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore7, Value = "EF Core 7" });
                list.Add(new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore6, Value = "EF Core 6" });
                list.Add(new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore8, Value = "EF Core 8 (preview)" });
            }

            if (!list.Any())
            {
                throw new InvalidOperationException(".NET 5 and earlier projects are no longer supported");
            }

            var firstMode = list.Select(i => i.Key).First();

            if (!list.Exists(i => i.Key == (int)codeGenerationMode))
            {
                codeGenerationMode = (CodeGenerationMode)firstMode;
            }

            return (codeGenerationMode, list);
        }

        public string ReportRevEngErrors(ReverseEngineerResult revEngResult, string missingProviderPackage)
        {
            var errors = new StringBuilder();
            if (revEngResult.EntityErrors.Count == 0)
            {
                errors.Append(ReverseEngineerLocale.ModelGeneratedSuccesfully + Environment.NewLine);
            }
            else
            {
                errors.Append(ReverseEngineerLocale.CheckOutputWindowForErrors + Environment.NewLine);
            }

            if (revEngResult.EntityWarnings.Count > 0)
            {
                errors.Append(ReverseEngineerLocale.CheckOutputWindowForWarnings + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(missingProviderPackage))
            {
                errors.AppendLine();
                errors.AppendFormat(string.Format(ReverseEngineerLocale.PackageNotFoundInProject, missingProviderPackage));
            }

            return errors.ToString();
        }

        public string GetReadMeText(ReverseEngineerOptions options, string content)
        {
            return content.Replace("[ProviderName]", GetProviderName(options.DatabaseType))
                .Replace("[ConnectionString]", options.ConnectionString.Replace(@"\", @"\\"))
                .Replace("[ContextName]", options.ContextClassName);
        }

        public string GetReadMeText(ReverseEngineerOptions options, string content, List<NuGetPackage> packages)
        {
            var extraPackages = packages.Where(p => !p.IsMainProviderPackage && p.UseMethodName != null)
                .Select(p => $"Use{p.UseMethodName}()").ToList();

            var useText = string.Empty;

            if (extraPackages.Count > 0)
            {
                useText = "," + Environment.NewLine + "           x => x." + string.Join(".", extraPackages);
            }

            return content.Replace("[ProviderName]", GetProviderName(options.DatabaseType))
                .Replace("[ConnectionString]", options.ConnectionString.Replace(@"\", @"\\"))
                .Replace("[UseList]", useText)
                .Replace("[ContextName]", options.ContextClassName);
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        private string GetProviderName(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.Undefined:
                    return "[ProviderName]";
                case DatabaseType.SQLServer:
                    return "SqlServer";
                case DatabaseType.SQLite:
                    return "Sqlite";
                case DatabaseType.Npgsql:
                    return "Npgsql";
                case DatabaseType.Mysql:
                    return "Mysql";
                case DatabaseType.Oracle:
                    return "Oracle";
                case DatabaseType.SQLServerDacpac:
                    return "SqlServer";
                case DatabaseType.Firebird:
                    return "Firebird";
                default:
                    return "[ProviderName]";
            }
        }
    }
}
