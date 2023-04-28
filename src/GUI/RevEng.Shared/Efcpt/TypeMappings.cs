using System.Text.Json.Serialization;

namespace RevEng.Common.Efcpt
{
    public class TypeMappings
    {
        [JsonPropertyName("use-DateOnly-TimeOnly")]
        public bool UseDateOnlyTimeOnly { get; set; }
        [JsonPropertyName("use-HierarchyId")]
        public bool UseHierarchyId { get; set; }
        [JsonPropertyName("use-spatial")]
        public bool UseSpatial { get; set; }
        [JsonPropertyName("use-NodaTime")]
        public bool UseNodaTime { get; set; }
    }
}
