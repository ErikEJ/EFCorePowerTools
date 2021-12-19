using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EFCorePowerTools.Extensions
{
    public static class CustomNameOptionsExtensions
    {
        public static Tuple<List<Schema>, string> TryRead(string optionsCustomNamePath, string optionsPath)
        {
            if (!optionsPath.EndsWith("efpt.config.json", System.StringComparison.OrdinalIgnoreCase))
            {
                var specificName = Path.GetFileName(optionsPath).Replace(".config.", ".renaming.");
                var specificPath = Path.Combine(Path.GetDirectoryName(optionsPath), specificName);

                if (File.Exists(specificPath))
                {
                    optionsCustomNamePath = specificPath;
                }
            }

            if (!File.Exists(optionsCustomNamePath)) return new Tuple<List<Schema>, string>(null, optionsCustomNamePath);
            if (File.Exists(optionsCustomNamePath + ".ignore")) return new Tuple<List<Schema>, string>(null, optionsCustomNamePath);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(optionsCustomNamePath, Encoding.UTF8))))
            {
                var ser = new DataContractJsonSerializer(typeof(List<Schema>));
                var customNamingOptions = ser.ReadObject(ms) as List<Schema>;
                return new Tuple<List<Schema>, string>(customNamingOptions, optionsCustomNamePath);
            }
        }

        public static string Write(List<Schema> namingOptions)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, true, "   "))
                {
                    var serializer = new DataContractJsonSerializer(typeof(List<Schema>));
                    serializer.WriteObject(writer, namingOptions);
                    writer.Flush();
                }

                var json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }
    }
}
