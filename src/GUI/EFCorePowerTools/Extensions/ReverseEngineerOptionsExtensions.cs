using EnvDTE;
using ReverseEngineer20;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EFCorePowerTools.Extensions
{
    using System.Runtime.Serialization;

    internal static class ReverseEngineerOptionsExtensions
    {
        public static ReverseEngineerOptions TryRead(string optionsPath)
        {
            if (!File.Exists(optionsPath)) return null;
            if (File.Exists(optionsPath + ".ignore")) return null;

            // Top-down deserialization:
            // Try to deserialize the current head version of the class.
            // If this fails, go top down with older versions and migrate them to the head version.

            var couldRead = TryRead(optionsPath, out ReverseEngineerOptions deserialized);
            if (couldRead)
                return deserialized;

            couldRead = TryRead(optionsPath, out ReverseEngineerOptionsV1 deserializedV1);
            if (couldRead)
                return ReverseEngineerOptions.FromV1(deserializedV1);

            // Fallback
            return null;
        }

        public static string Write(this ReverseEngineerOptions options)
        {
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