using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace RevEng.Common.Cli
{
    public static class T4Helper
    {
        public static string DropT4Templates(string projectPath)
        {
            const string T4Version = "703";

            var zipName = $"T4_{T4Version}.zip";

            var toDir = Path.Combine(projectPath, "CodeTemplates");
            var templateZip = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), zipName);

            if (!Directory.Exists(toDir))
            {
                Directory.CreateDirectory(toDir);
                ZipFile.ExtractToDirectory(templateZip, toDir);
            }

            if (Directory.Exists(toDir))
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
    }
}
