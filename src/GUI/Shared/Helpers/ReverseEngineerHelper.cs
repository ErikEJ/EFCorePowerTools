using System;
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

        public Tuple<bool, Version> HasSqlServerViewDefinitionRightsAndVersion(string connectionString)
        {
            var hasRights = false;
            Version version;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT HAS_PERMS_BY_NAME(DB_NAME(), 'DATABASE', 'VIEW DEFINITION');", connection))
                {
                    var result = (int)command.ExecuteScalar();
                    hasRights = Convert.ToBoolean(result);
                }

                using (var command = new SqlCommand("SELECT SERVERPROPERTY('ProductVersion');", connection))
                {
                    var result = (string)command.ExecuteScalar();
                    version = new Version(result);
                }
            }

            return new Tuple<bool, Version>(hasRights, version);
        }

        public void DropT4Templates(string projectPath)
        {
            DropTemplates(projectPath, projectPath, CodeGenerationMode.EFCore7, false);
        }

        public void DropTemplates(string optionsPath, string projectPath, CodeGenerationMode codeGenerationMode, bool useHandlebars)
        {
            string zipName;
            if (useHandlebars)
            {
                switch (codeGenerationMode)
                {
                    case CodeGenerationMode.EFCore3:
                        zipName = "CodeTemplates.zip";
                        break;
                    case CodeGenerationMode.EFCore6:
                        zipName = "CodeTemplates600.zip";
                        break;
                    default:
                        throw new ArgumentException($"Unsupported code generation mode for templates: {codeGenerationMode}");
                }
            }
            else
            {
                if (codeGenerationMode == CodeGenerationMode.EFCore7)
                {
                    zipName = "T4_700.zip";
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
            }
        }

        public (CodeGenerationMode UsedMode, IList<CodeGenerationItem> AllowedVersions) CalculateAllowedVersions(CodeGenerationMode codeGenerationMode, Version minimumVersion)
        {
            var list = new List<CodeGenerationItem>();

            if (minimumVersion.Major == 6 || minimumVersion.Major == 2)
            {
                list.Add(new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore6, Value = "EF Core 6" });
                list.Add(new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore7, Value = "EF Core 7 (preview)" });
            }

            if (minimumVersion.Major == 3 || minimumVersion.Major == 2)
            {
                list.Add(new CodeGenerationItem { Key = (int)CodeGenerationMode.EFCore3, Value = "EF Core 3" });
            }

            if (!list.Any())
            {
                throw new InvalidOperationException("no supported target frameworks found in project");
            }

            var firstMode = list.Select(i => i.Key).First();

            if (!list.Any(i => i.Key == (int)codeGenerationMode))
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
                .Replace("[ConnectionString]", options.ConnectionString)
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
