using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RevEng.Common.Extensions
{
    public static class JsonSerializerExtensions
    {
        public static T TryReadJsonFile<T>(this string filePath)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return new T();
            }

            var couldRead = TryReadJsonFile(filePath, out T deserialized);
            if (couldRead)
            {
                return deserialized;
            }

            return new T();
        }

        public static bool TryReadJsonFile<T>(this string filePath, out T deserialized)
            where T : class, new()
        {
            try
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(filePath, Encoding.UTF8)));
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

        public static string ToJson<T>(this T jsonModelRoot)
            where T : class
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, true, "   "))
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    serializer.WriteObject(writer, jsonModelRoot);
                    writer.Flush();
                }

                var json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }

        public static void ToJson<T>(this T jsonModel, string filePath)
            where T : class
        {
            var json = ToJson(jsonModel);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }
    }
}
