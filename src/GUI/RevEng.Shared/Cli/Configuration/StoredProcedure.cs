using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class StoredProcedure : IEntity
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
        [JsonPropertyName("mapped-type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string MappedType { get; set; }

        [JsonPropertyOrder(40)]
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        [JsonPropertyOrder(50)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("use-legacy-resultset-discovery")]
        public bool UseLegacyResultsetDiscovery { get; set; }

        [JsonIgnore]
        public List<string> ExcludedColumns { get; set; }
    }
}
