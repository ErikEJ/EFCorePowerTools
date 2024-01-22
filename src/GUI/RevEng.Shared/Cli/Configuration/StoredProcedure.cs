using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class StoredProcedure : IEntity
    {
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("exclude")]
        public bool? Exclude { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("use-legacy-resultset-discovery")]
        public bool UseLegacyResultsetDiscovery { get; set; }

        [JsonPropertyName("mapped-type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string MappedType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("exclusionWildcard")]
        public string ExclusionWildcard { get; set; }

        [JsonIgnore]
        public List<string> ExcludedColumns { get; set; }
    }
}
