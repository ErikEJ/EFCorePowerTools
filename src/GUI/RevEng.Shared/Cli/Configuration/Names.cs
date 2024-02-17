using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class Names
    {
        [JsonPropertyOrder(10)]
        [JsonPropertyName("dbcontext-name")]
        public string DbContextName { get; set; }

        [JsonPropertyOrder(20)]
        [JsonPropertyName("dbcontext-namespace")]
        public string DbContextNamespace { get; set; }

        [JsonPropertyOrder(30)]
        [JsonPropertyName("model-namespace")]
        public string ModelNamespace { get; set; }

        [JsonPropertyOrder(40)]
        [JsonPropertyName("root-namespace")]
        public string RootNamespace { get; set; }
    }
}
