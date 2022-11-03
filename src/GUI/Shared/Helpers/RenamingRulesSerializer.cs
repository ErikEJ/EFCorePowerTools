using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using RevEng.Common;

namespace EFCorePowerTools.Helpers
{
    public static class RenamingRulesSerializer
    {
        public static Model TryRead(string configFilePath)
        {
            if (string.IsNullOrEmpty(configFilePath) || !File.Exists(configFilePath))
            {
                return new Model();
            }

            var couldRead = TryRead(configFilePath, out Model deserialized);
            if (couldRead)
            {
                return deserialized;
            }

            return new Model();
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
