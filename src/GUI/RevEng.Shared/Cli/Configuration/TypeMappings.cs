using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class TypeMappings
    {
        [JsonPropertyOrder(10)]
        [JsonPropertyName("use-DateOnly-TimeOnly")]
        public bool UseDateOnlyTimeOnly { get; set; }

        [JsonPropertyOrder(20)]
        [JsonPropertyName("use-HierarchyId")]
        public bool UseHierarchyId { get; set; }

        [JsonPropertyOrder(30)]
        [JsonPropertyName("use-NodaTime")]
        public bool UseNodaTime { get; set; }

        [JsonPropertyOrder(40)]
        [JsonPropertyName("use-spatial")]
        public bool UseSpatial { get; set; }
    }
}
