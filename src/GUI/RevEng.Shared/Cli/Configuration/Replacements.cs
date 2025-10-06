using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class Replacements
    {
        [JsonPropertyOrder(10)]
        [JsonPropertyName("preserve-casing-with-regex")]
        public bool PreserveCasingWithRegex { get; set; }

        [JsonPropertyOrder(30)]
        [JsonPropertyName("irregular-words")]
#pragma warning disable CA1819
        public Irregular[] IrregularWords { get; set; }
#pragma warning restore CA1819

        [JsonPropertyOrder(20)]
        [JsonPropertyName("uncountable-words")]
#pragma warning disable CA1819
        public string[] UncountableWords { get; set; }
#pragma warning restore CA1819

        [JsonPropertyOrder(40)]
        [JsonPropertyName("plural-rules")]
#pragma warning disable CA1819
        public RuleReplacement[] PluralWords { get; set; }
#pragma warning restore CA1819

        [JsonPropertyOrder(50)]
        [JsonPropertyName("singular-rules")]
#pragma warning disable CA1819
        public RuleReplacement[] SingularWords { get; set; }
#pragma warning restore CA1819
    }
}