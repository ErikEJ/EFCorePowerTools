using EFCorePowerTools.Handlers.ReverseEngineer;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EFCorePowerTools.Extensions
{
    internal static class ReverseEngineerOptionsExtensions
    {
        public static ReverseEngineerOptions TryRead(string optionsPath, string projectDirectory)
        {
            if (!File.Exists(optionsPath)) return null;
            if (File.Exists(optionsPath + ".ignore")) return null;

            var couldRead = TryRead(optionsPath, out ReverseEngineerOptions deserialized);
            if (couldRead)
            {
                if (!string.IsNullOrEmpty(deserialized.UiHint))
                {
                    deserialized.UiHint = GetFullPathForSqlProj(deserialized.UiHint, projectDirectory);
                }

                return deserialized;
            }
            return null;
        }

        public static string Write(this ReverseEngineerOptions options, string projectDirectory)
        {
            if (!string.IsNullOrEmpty(options.UiHint))
            {
                options.UiHint = SetRelativePathForSqlProj(options.UiHint, projectDirectory);
            }

            using (var ms = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, true, "   "))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ReverseEngineerOptions));
                    serializer.WriteObject(writer, options);
                    writer.Flush();
                }

                var json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }

        private static string SetRelativePathForSqlProj(string uiHint, string projectDirectory)
        {
            if (string.IsNullOrEmpty(projectDirectory))
            {
                return uiHint;
            }

            if (!uiHint.EndsWith(".sqlproj", System.StringComparison.OrdinalIgnoreCase))
            {
                return uiHint;
            }

            if (Path.IsPathRooted(uiHint))
            {
                return PathExtensions.GetRelativePath(projectDirectory, uiHint);
            }

            return uiHint;
        }

        private static string GetFullPathForSqlProj(string uiHint, string projectDirectory)
        {
            if (string.IsNullOrEmpty(projectDirectory))
            {
                return uiHint;
            }

            if (!uiHint.EndsWith(".sqlproj", System.StringComparison.OrdinalIgnoreCase))
            {
                return uiHint;
            }

            if (!Path.IsPathRooted(uiHint))
            {
                string sqlProjPath = Path.Combine(projectDirectory, uiHint);
                return Path.GetFullPath(sqlProjPath);
            }

            return uiHint;
        }

        private static bool TryRead<T>(string optionsPath, out T deserialized) where T : class, new()
        {
            try
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(optionsPath, Encoding.UTF8)));
                var ser = new DataContractJsonSerializer(typeof(T));
                deserialized = ser.ReadObject(ms) as T;
                ms.Close();
                return true;
            }
            catch
            {
                deserialized = null;
                return false;
            }
        }
    }
}