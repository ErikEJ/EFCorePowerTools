using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ReverseEngineer20.ReverseEngineer
{
    public static class ReverseEngineerCommandOptionsExtensions
    {
        public static string Write(this ReverseEngineerCommandOptions result)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, true, "   "))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ReverseEngineerCommandOptions));
                    serializer.WriteObject(writer, result);
                    writer.Flush();
                }

                var json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }

        public static ReverseEngineerCommandOptions TryRead(string optionsPath)
        {
            if (!File.Exists(optionsPath)) return null;

            var couldRead = TryRead(optionsPath, out ReverseEngineerCommandOptions deserialized);
            if (couldRead)
                return deserialized;

            return null;
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