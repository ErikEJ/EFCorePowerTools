using System.Text.Json.Serialization;

namespace RevEng.Common.Efcpt
{
    public class Replacements
    {
        [JsonPropertyName("preserve-casing-with-regex")]
        public bool PreserveCasingWithRegex { get; set; }
        [JsonPropertyName("uncountable-words")]
#pragma warning disable CA1819
        public string[] UncountableWords { get; set; }
#pragma warning restore CA1819
    }
}
