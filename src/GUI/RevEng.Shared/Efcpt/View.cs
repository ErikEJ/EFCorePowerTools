using System.Text.Json.Serialization;

namespace RevEng.Common.Efcpt
{
    public class View : IEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("exclude")]
        public bool Exclude { get; set; }
    }
}
