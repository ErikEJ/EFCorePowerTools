using System;
using System.Collections.Generic;
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

        public void DropT4Templates(string projectPath)
        {
            DropTemplates(projectPath, projectPath, CodeGenerationMode.EFCore8, false);
        }

        public string DropTemplates(string optionsPath, string projectPath, CodeGenerationMode codeGenerationMode, bool useHandlebars, int selectedOption = 0)
        {
            string zipName;
            string t4Version = "703";

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
                    case CodeGenerationMode.EFCore8:
                        zipName = "CodeTemplates800.zip";
                        break;
                    default:
                        throw new ArgumentException($"Unsupported code generation mode for templates: {codeGenerationMode}");
                }
            }
            else
            {
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore7:
                        t4Version = "703";
                        break;
                    case CodeGenerationMode.EFCore8:
                        t4Version = "800";
                        break;
                    case CodeGenerationMode.EFCore9:
                        t4Version = "900";
                        break;
                    default:
                        throw new ArgumentException($"Unsupported code generation mode for T4 templates: {codeGenerationMode}");
                }

                zipName = $"T4_{t4Version}.zip";
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

            if (!useHandlebars && Directory.Exists(toDir) && selectedOption != 3)
            {
                var error = $"The latest T4 template version could not be found, looking for 'Template version: {t4Version}' in the T4 file - please update your T4 templates, for example by renaming the CodeTemplates folder.";
                var check = $"Template version: {t4Version}";

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
            }

            if (minimumVersion.Major >= 8)
            {
                list.Add(new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore8, Value = "EF Core 8" });
                list.Add(new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore7, Value = "EF Core 7" });
                list.Add(new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore9, Value = "EF Core 9 (preview)" });
            }

            if (!list.Any())
            {
                return (codeGenerationMode, list);
            }

            var firstMode = list.Select(i => i.Key).First();

            if (!list.Exists(i => i.Key == (int)codeGenerationMode))
            {
                codeGenerationMode = (CodeGenerationMode)firstMode;
            }

            return (codeGenerationMode, list);
        }

        public IList<TemplateTypeItem> CalculateAllowedTemplates(CodeGenerationMode codeGenerationMode)
        {
            var list = new List<TemplateTypeItem>();

            if (codeGenerationMode == CodeGenerationMode.EFCore7
                || codeGenerationMode == CodeGenerationMode.EFCore8)
            {
                list.Add(new TemplateTypeItem { Key = 0, Value = "C# - Handlebars" });
                list.Add(new TemplateTypeItem { Key = 1, Value = "TypeScript - Handlebars" });
                list.Add(new TemplateTypeItem { Key = 2, Value = "C# - T4" });
                list.Add(new TemplateTypeItem { Key = 3, Value = "C# - T4 (POCO)" });
            }
            else if (codeGenerationMode == CodeGenerationMode.EFCore6)
            {
                list.Add(new TemplateTypeItem { Key = 0, Value = "C# - Handlebars" });
                list.Add(new TemplateTypeItem { Key = 1, Value = "TypeScript - Handlebars" });
            }
            else if (codeGenerationMode == CodeGenerationMode.EFCore9)
            {
                list.Add(new TemplateTypeItem { Key = 2, Value = "C# - T4" });
                list.Add(new TemplateTypeItem { Key = 3, Value = "C# - T4 (POCO)" });
            }

            return list;
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
                if (revEngResult.EntityWarnings.Exists(w => w.IndexOf("Could not find type mapping", StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    revEngResult.EntityWarnings.Add("Consider enabling more type mappings via 'Advanced' options.");
                }

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

        public string AddResultToFinalText(string finalText, ReverseEngineerResult revEngResult)
        {
            if (revEngResult.HasIssues)
            {
                var warningText = new StringBuilder();

                warningText.AppendLine("Some issues were discovered during reverse engineering, consider addressing them:");
                warningText.AppendLine();

                foreach (var errorItem in revEngResult.EntityErrors)
                {
                    warningText.AppendLine(errorItem);
                    warningText.AppendLine();
                }

                foreach (var warningItem in revEngResult.EntityWarnings)
                {
                    warningText.AppendLine(warningItem);
                    warningText.AppendLine();
                }

                finalText = finalText + Environment.NewLine + warningText.ToString();
            }

            return finalText;
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
