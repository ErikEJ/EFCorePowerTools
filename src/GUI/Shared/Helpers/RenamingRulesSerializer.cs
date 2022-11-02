using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using RevEng.Common;

namespace EFCorePowerTools.Helpers
{
    internal static class RenamingRulesSerializer
    {
        public static IEnumerable<Schema> TryRead(string optionsPath)
        {
            if (!File.Exists(optionsPath))
            {
                return Enumerable.Empty<Schema>();
            }

            var couldRead = TryRead(optionsPath, out List<Schema> deserialized);
            if (couldRead)
            {
                return deserialized;
            }

            return Enumerable.Empty<Schema>();
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
