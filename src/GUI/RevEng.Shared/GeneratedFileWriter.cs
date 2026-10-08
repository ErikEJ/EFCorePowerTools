using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace RevEng.Common
{
    public static class GeneratedFileWriter
    {
        public static void WriteAllText(string path, string text, string lineEndingStyle = null, string fileEncoding = null)
        {
            var normalized = NormalizeLineEndings(text, lineEndingStyle);
            var encoding = GetEncoding(fileEncoding);
            Retry(() => File.WriteAllText(path, normalized, encoding));
        }

        public static void WriteAllLines(string path, IEnumerable<string> lines, string lineEndingStyle = null, string fileEncoding = null)
        {
            if (lines == null)
            {
                throw new ArgumentNullException(nameof(lines));
            }

            var newLine = GetLineEnding(lineEndingStyle);
            var encoding = GetEncoding(fileEncoding);
            Retry(() =>
            {
                using var writer = new StreamWriter(path, false, encoding) { NewLine = newLine };
                foreach (var line in lines)
                {
                    writer.WriteLine(line == null ? null : NormalizeLineEndings(line, lineEndingStyle));
                }
            });
        }

        public static string NormalizeLineEndings(string text, string lineEndingStyle)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", GetLineEnding(lineEndingStyle));
        }

        public static bool IsSupportedLineEndingStyle(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                || value.Equals("native", StringComparison.OrdinalIgnoreCase)
                || value.Equals("lf", StringComparison.OrdinalIgnoreCase)
                || value.Equals("crlf", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetLineEnding(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Equals("native", StringComparison.OrdinalIgnoreCase))
            {
                return Environment.NewLine;
            }

            if (value.Equals("lf", StringComparison.OrdinalIgnoreCase))
            {
                return "\n";
            }

            if (value.Equals("crlf", StringComparison.OrdinalIgnoreCase))
            {
                return "\r\n";
            }

            throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported line ending style.");
        }

        public static bool IsSupportedEncoding(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                || value.Equals("utf-8-bom", StringComparison.OrdinalIgnoreCase)
                || value.Equals("utf-8", StringComparison.OrdinalIgnoreCase);
        }

        public static Encoding GetEncoding(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Equals("utf-8-bom", StringComparison.OrdinalIgnoreCase))
            {
                return Encoding.UTF8;
            }

            if (value.Equals("utf-8", StringComparison.OrdinalIgnoreCase))
            {
                return new UTF8Encoding(false);
            }

            throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported file encoding.");
        }

        public static void ValidateOptions(ReverseEngineerCommandOptions options, List<string> warnings)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (warnings == null)
            {
                throw new ArgumentNullException(nameof(warnings));
            }

            if (!IsSupportedLineEndingStyle(options.FileLineEndingStyle))
            {
                warnings.Add($"FileLineEndingStyle '{options.FileLineEndingStyle}' is invalid. Supported values are 'native', 'lf', or 'crlf'. The native platform line ending will be used.");
                options.FileLineEndingStyle = "native";
            }

            if (!IsSupportedEncoding(options.FileEncoding))
            {
                warnings.Add($"FileEncoding '{options.FileEncoding}' is invalid. Supported values are 'utf-8' or 'utf-8-bom'. UTF-8 with BOM will be used.");
                options.FileEncoding = "utf-8-bom";
            }
        }

        private static void Retry(Action write)
        {
            for (int i = 1; i <= 4; ++i)
            {
                try
                {
                    write();
                    return;
                }
                catch (IOException) when (i <= 3)
                {
                    Thread.Sleep(500);
                }
            }
        }
    }
}
