using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    public class Function : IEntity
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("exclude")]
        public bool? Exclude { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("exclusionWildcard")]
        public string ExclusionWildcard { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("excludedColumns")]
        public string[] ExcludedColumns { get; set; }
    }
}
