﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public (CodeGenerationMode UsedMode, IEnumerable<KeyValuePair<int, string>> AllowedVersions) CalculateAllowedVersions(CodeGenerationMode codeGenerationMode, Version minimumVersion)
        {
            var list = new List<KeyValuePair<int, string>>();

            if (minimumVersion.Major == 6 || minimumVersion.Major == 2)
            {
                list.Add(new KeyValuePair<int, string>(2, "EF Core 6"));
                list.Add(new KeyValuePair<int, string>(3, "EF Core 7 (preview)"));
            }

            if (minimumVersion.Major == 5 || minimumVersion.Major == 2)
            {
                list.Add(new KeyValuePair<int, string>(0, "EF Core 5"));
            }

            if (minimumVersion.Major == 3 || (minimumVersion.Major == 2 && minimumVersion.Minor == 0) || minimumVersion.Major == 5)
            {
                list.Add(new KeyValuePair<int, string>(1, "EF Core 3"));
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
