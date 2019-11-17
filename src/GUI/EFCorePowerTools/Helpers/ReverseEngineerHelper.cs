using EFCorePowerTools.Shared.Models;
using ReverseEngineer20;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EFCorePowerTools.Helpers
{
    public class ReverseEngineerHelper
    {
        public List<TableInformationModel> NormalizeTables(List<TableInformationModel> tables, bool shouldFix)
        {
            var result = new List<TableInformationModel>();
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

        public string ReportRevEngErrors(ReverseEngineerResult revEngResult, string missingProviderPackage)
        {
            var errors = new StringBuilder();
            if (revEngResult.EntityErrors.Count == 0)
            {
                errors.Append("Model generated successfully." + Environment.NewLine);
            }
            else
            {
                errors.Append("Please check the output window for errors" + Environment.NewLine);
            }

            if (revEngResult.EntityWarnings.Count > 0)
            {
                errors.Append("Please check the output window for warnings" + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(missingProviderPackage))
            {
                errors.AppendLine();
                errors.AppendFormat("The \"{0}\" NuGet package was not found in the project - it must be installed in order to build.", missingProviderPackage);
            }

            return errors.ToString();
        }

        public string GenerateClassName(string value)
        {
            var className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
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
