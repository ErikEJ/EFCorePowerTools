using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class Irregular
    {
        [JsonPropertyOrder(10)]
        [JsonPropertyName("singular")]
        public string Singular { get; set; }

        [JsonPropertyOrder(20)]
        [JsonPropertyName("plural")]
        public string Plural { get; set; }

        [JsonPropertyOrder(30)]
        [JsonPropertyName("match-ending")]
        public bool MatchEnding { get; set; } = true;
    }
}