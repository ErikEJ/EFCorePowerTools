using EnvDTE;
using ReverseEngineer20;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EFCorePowerTools.Extensions
{
    internal static class ReverseEngineerOptionsExtensions
    {
        public static ReverseEngineerOptions TryRead(string optionsPath)
        {
            if (!File.Exists(optionsPath)) return null;
            if (File.Exists(optionsPath + ".ignore")) return null;

            ReverseEngineerOptions deserializedOptions = null;
            try
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(optionsPath, Encoding.UTF8)));
                var ser = new DataContractJsonSerializer(typeof(ReverseEngineerOptions));
                deserializedOptions = ser.ReadObject(ms) as ReverseEngineerOptions;
                ms.Close();
            }
            catch { }
            return deserializedOptions;
        }

        public static string Write(this ReverseEngineerOptions options)
        {
            var ms = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(ReverseEngineerOptions));
            ser.WriteObject(ms, options);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
    }
}