using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class RuleReplacement
    {
        [JsonPropertyOrder(10)]
        [JsonPropertyName("rule")]
        public string Rule { get; set; }

        [JsonPropertyOrder(20)]
        [JsonPropertyName("replacement")]
        public string Replacement { get; set; }
    }
}