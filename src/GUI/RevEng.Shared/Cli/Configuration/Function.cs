using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    public class Function : IEntity
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        [JsonPropertyOrder(10)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("exclude")]
        public bool? Exclude { get; set; }

        [JsonPropertyOrder(20)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("exclusionWildcard")]
        public string ExclusionWildcard { get; set; }

        [JsonPropertyOrder(30)]
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        [JsonIgnore]
        public List<string> ExcludedColumns { get; set; } = null;
    }
}
