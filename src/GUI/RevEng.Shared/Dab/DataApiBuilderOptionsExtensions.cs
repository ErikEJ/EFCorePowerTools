using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RevEng.Common.Dab
{
    public static class DataApiBuilderOptionsExtensions
    {
        public static DataApiBuilderOptions TryRead(string optionsPath)
        {
            if (string.IsNullOrEmpty(optionsPath))
            {
                return null;
            }

            if (!File.Exists(optionsPath))
            {
                return null;
            }

            if (File.Exists(optionsPath + ".ignore"))
            {
                return null;
            }

            var options = File.ReadAllText(optionsPath, Encoding.UTF8);

            var couldRead = TryRead(options, out DataApiBuilderOptions deserialized);
            if (couldRead)
            {
                return deserialized;
            }

            return null;
        }

        public static string Write(this DataApiBuilderOptions result)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, true, "   "))
                {
                    var serializer = new DataContractJsonSerializer(typeof(DataApiBuilderOptions));
                    serializer.WriteObject(writer, result);
                    writer.Flush();
                }

                var json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }

        private static bool TryRead<T>(string options, out T deserialized)
            where T : class, new()
        {
            try
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(options));
                var ser = new DataContractJsonSerializer(typeof(T));
                deserialized = ser.ReadObject(ms) as T;
                ms.Close();
                return true;
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is EncoderFallbackException)
            {
                deserialized = null;
                return false;
            }
        }
    }
}
