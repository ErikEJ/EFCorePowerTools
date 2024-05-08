using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using RevEng.Common;

namespace EFCorePowerTools.Extensions
{
    internal static class ReverseEngineerOptionsExtensions
    {
        public static ReverseEngineerOptions TryRead(string optionsPath)
        {
            if (!File.Exists(optionsPath))
            {
                return null;
            }

            if (optionsPath.EndsWith(Constants.ConfigFileName, System.StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (File.Exists(optionsPath + ".ignore"))
            {
                return null;
            }

            var couldRead = TryRead(optionsPath, out ReverseEngineerOptions deserialized);
            if (couldRead)
            {
                return deserialized;
            }

            return null;
        }

        public static string Write(this ReverseEngineerOptions options)
        {
            options.UiHint = null;

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

        private static bool TryRead<T>(string optionsPath, out T deserialized)
            where T : class, new()
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
