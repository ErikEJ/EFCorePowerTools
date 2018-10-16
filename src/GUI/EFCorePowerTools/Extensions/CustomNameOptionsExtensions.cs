using ReverseEngineer20;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace EFCorePowerTools.Extensions
{
    public class CustomNameOptionsExtensions
    {
        public static List<Schema> TryRead(string optionsCustomNamePath)
        {
            if (!File.Exists(optionsCustomNamePath)) return null;
            if (File.Exists(optionsCustomNamePath + ".ignore")) return null;

            List<Schema> customNamingOptions = null;
            try
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(optionsCustomNamePath, Encoding.UTF8)));
                var ser = new DataContractJsonSerializer(typeof(List<Schema>));
                customNamingOptions = ser.ReadObject(ms) as List<Schema>;
                ms.Close();
            }
            catch{}
            return customNamingOptions;
        }
    }
}
