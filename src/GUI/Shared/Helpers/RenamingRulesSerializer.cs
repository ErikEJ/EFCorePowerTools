using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using RevEng.Common;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EFCorePowerTools.Helpers
{
    internal static class RenamingRulesSerializer
    {
        public static readonly JsonSerializerOptions Settings = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        public static Schema[] FromJson(string json) =>
            JsonSerializer.Deserialize<Schema[]>(json, Settings);

        public static string ToJson(this IEnumerable<Schema> rules) => JsonSerializer.Serialize(rules, Settings);
    }
}
