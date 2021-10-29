using EFCorePowerTools.Locales;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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
            var version = new Version(12, 0);
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
                errors.AppendFormat(String.Format(ReverseEngineerLocale.PackageNotFoundInProject, missingProviderPackage));
            }

            return errors.ToString();
        }

        public string GenerateClassName(string classBasis)
        {
            if (string.IsNullOrWhiteSpace(classBasis))
            {
                throw new ArgumentNullException(nameof(classBasis));
            }
            var className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(classBasis);
            var isValid = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#").IsValidIdentifier(className);

            if (!isValid)
            {
                // File name contains invalid chars, remove them
                var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.None, TimeSpan.FromSeconds(5));
                className = regex.Replace(className, "");

                // Class name doesn't begin with a letter, insert an underscore
                if (!char.IsLetter(className, 0))
                {
                    className = className.Insert(0, "_");
                }
            }

            return className.Replace(" ", string.Empty);
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

    }
}
