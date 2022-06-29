using RevEng.Common;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RevEng.Core
{
    public static class ReverseEngineerOptionsExtensions
    {
        public static ReverseEngineerCommandOptions TryDeserialize(string options)
        {
            var couldRead = TryRead(options, out ReverseEngineerCommandOptions deserialized);
            if (couldRead)
            {
                return deserialized;
            }

            // Fallback
            return null;
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
