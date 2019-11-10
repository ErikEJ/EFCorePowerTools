using ReverseEngineer20.ReverseEngineer;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EFCorePowerTools.Extensions
{
    public class CustomNameOptionsExtensions
    {
        public static List<Schema> TryRead(string optionsCustomNamePath)
        {
            if (!File.Exists(optionsCustomNamePath)) return null;
            if (File.Exists(optionsCustomNamePath + ".ignore")) return null;

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(optionsCustomNamePath, Encoding.UTF8))))
            {
                var ser = new DataContractJsonSerializer(typeof(List<Schema>));
                var customNamingOptions = ser.ReadObject(ms) as List<Schema>;
                return customNamingOptions;
            }
        }
    }
}
