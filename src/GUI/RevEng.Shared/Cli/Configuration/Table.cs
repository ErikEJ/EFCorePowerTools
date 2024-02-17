using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class Table : IEntity
    {
        [JsonPropertyOrder(10)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("exclude")]
        public bool? Exclude { get; set; }

        [JsonPropertyOrder(20)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("excludedColumns")]
        public List<string> ExcludedColumns { get; set; } = null;

        [JsonPropertyOrder(30)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("exclusionWildcard")]
        public string ExclusionWildcard { get; set; }

        [JsonPropertyOrder(40)]
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }
    }
}
