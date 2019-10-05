using ReverseEngineer20;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ReverseEngineer20.ReverseEngineer
{
    internal static class ReverseEngineerOptionsExtensions
    {
        public static ReverseEngineerCommandOptions TryDeserialize(string options)
        {
            var couldRead = TryRead(options, out ReverseEngineerCommandOptions deserialized);
            if (couldRead)
                return deserialized;

            // Fallback
            return null;
        }

        private static bool TryRead<T>(string options, out T deserialized) where T : class, new()
        {
            try
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(options));
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