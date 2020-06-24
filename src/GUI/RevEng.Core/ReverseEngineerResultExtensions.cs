using ReverseEngineer20;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ReverseEngineer20.ReverseEngineer
{
    public static class ReverseEngineerResultExtensions
    {
        public static string Write(this ReverseEngineerResult result)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, true, "   "))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ReverseEngineerResult));
                    serializer.WriteObject(writer, result);
                    writer.Flush();
                }

                var json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }
    }
}
