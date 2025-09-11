using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace RevEng.Common.Cli
{
    public static class T4Helper
    {
        public static string DropT4Templates(string projectPath, CodeGenerationMode codeGenerationMode, bool useEntityTypeSplitting = false)
        {
            string t4Version = "900";

            if (codeGenerationMode == CodeGenerationMode.EFCore8)
            {
                t4Version = "800";
            }

            if (codeGenerationMode == CodeGenerationMode.EFCore10)
            {
                t4Version = "1000";
            }

            if (useEntityTypeSplitting)
            {
                t4Version += "_Split";
            }

            var zipName = $"T4_{t4Version}.zip";

            var toDir = Path.Combine(projectPath, "CodeTemplates");
            var templateZip = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), zipName);

            if (!Directory.Exists(toDir))
            {
                Directory.CreateDirectory(toDir);
                ZipFile.ExtractToDirectory(templateZip, toDir);
            }

            if (Directory.Exists(toDir))
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

                target = Path.Combine(toDir, "EFCore", "EntityTypeConfiguration.t4");
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
    }
}