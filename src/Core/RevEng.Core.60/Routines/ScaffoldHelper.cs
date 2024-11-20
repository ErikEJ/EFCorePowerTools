using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Design;
using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Routines
{
    internal static class ScaffoldHelper
    {
        private static readonly HashSet<string> KeyWords = new HashSet<string>
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "void",
            "volatile",
            "while",
        };

        public static string GetDbContextExtensionsText(bool useAsyncCalls)
        {
#if CORE80
            var dbContextExtensionTemplateName = useAsyncCalls ? "RevEng.Core.DbContextExtensionsSqlQuery" : "RevEng.Core.DbContextExtensionsSqlQuery.Sync";
#else
            var dbContextExtensionTemplateName = useAsyncCalls ? "RevEng.Core.DbContextExtensions" : "RevEng.Core.DbContextExtensions.Sync";
#endif
            var assembly = typeof(ScaffoldHelper).GetTypeInfo().Assembly;
            using var stream = assembly.GetManifestResourceStream(dbContextExtensionTemplateName);
            if (stream == null)
            {
                return string.Empty;
            }

            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static string GenerateIdentifierName(Routine routine, RoutineModel model, ICSharpHelper code, bool usePascalCase)
        {
            ArgumentNullException.ThrowIfNull(routine);

            ArgumentNullException.ThrowIfNull(model);

            if (usePascalCase)
            {
                var name = GenerateUniqueName(routine, model);

                return CreateIdentifier(name, name, true).Item1;
            }
            else
            {
                var identifier = code.Identifier(GenerateUniqueName(routine, model));

                return GenerateIdentifier(identifier);
            }
        }

        public static Tuple<string, string> GeneratePropertyName(string propertyName, ICSharpHelper code, bool usePascalCase)
        {
            ArgumentNullException.ThrowIfNull(propertyName);

            if (!usePascalCase)
            {
                return CreateIdentifier(propertyName, propertyName, false);
            }
            else
            {
                var identifier = code.Identifier(propertyName, capitalize: true);

                return CreateIdentifier(GenerateIdentifier(identifier), propertyName, true);
            }
        }

        public static Tuple<string, string> CreateIdentifier(string name, string propertyName, bool usePascalCase)
        {
            if (!usePascalCase)
            {
                var original = name;

                var isValid = System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(name);

                string columAttribute = null;

                if (!isValid)
                {
                    columAttribute = $"[Column(\"{name}\")]";

                    // File name contains invalid chars, remove them
                    var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.None, TimeSpan.FromSeconds(5));
                    name = regex.Replace(name, string.Empty);

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        // we cannot fix it
                        name = original;
                    }
                    else if (!char.IsLetter(name, 0))
                    {
                        name = name.Insert(0, "_");
                    }
                }

                if (KeyWords.Contains(name))
                {
                    name = "@" + name;
                }

                return new Tuple<string, string>(name.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase), columAttribute);
            }
            else
            {
                string columAttribute = null;
                if (!name.Equals(propertyName, StringComparison.Ordinal))
                {
                    columAttribute = $"[Column(\"{propertyName}\")]";
                }

                return new Tuple<string, string>(name, columAttribute);
            }
        }

        private static string GenerateUniqueName(Routine routine, RoutineModel model)
        {
            if (!string.IsNullOrEmpty(routine.NewName))
            {
                return routine.NewName;
            }

            var numberOfNames = model.Routines.Count(p => p.Name == routine.Name);

            if (numberOfNames > 1)
            {
                return routine.Name + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(routine.Schema);
            }

            return routine.Name;
        }

        private static string GenerateIdentifier(string value)
        {
            var candidateStringBuilder = new StringBuilder();
            var previousLetterCharInWordIsLowerCase = false;
            var isFirstCharacterInWord = true;

            foreach (var c in value)
            {
                var isNotLetterOrDigit = !char.IsLetterOrDigit(c);
                if (isNotLetterOrDigit
                    || (previousLetterCharInWordIsLowerCase && char.IsUpper(c)))
                {
                    isFirstCharacterInWord = true;
                    previousLetterCharInWordIsLowerCase = false;
                    if (isNotLetterOrDigit)
                    {
                        continue;
                    }
                }

                candidateStringBuilder.Append(
                    isFirstCharacterInWord ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
                isFirstCharacterInWord = false;
                if (char.IsLower(c))
                {
                    previousLetterCharInWordIsLowerCase = true;
                }
            }

            return candidateStringBuilder.ToString();
        }
    }
}
