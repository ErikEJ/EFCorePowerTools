using System.Text.Json.Serialization;

namespace RevEng.Common.Efcpt
{
    public class Names
    {
        [JsonPropertyName("root-namespace")]
        public string RootNamespace { get; set; }

        [JsonPropertyName("dbcontext-name")]
        public string DbContextName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("dbcontext-namespace")]
        public string DbContextNamespace { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("model-namespace")]
        public string ModelNamespace { get; set; }
    }
}
