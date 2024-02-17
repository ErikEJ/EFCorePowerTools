using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class FileLayout
    {
        [JsonPropertyOrder(10)]
        [JsonPropertyName("output-dbcontext-path")]
        public string OutputDbContextPath { get; set; }

        [JsonPropertyOrder(20)]
        [JsonPropertyName("output-path")]
        public string OutputPath { get; set; } = "Models";

        [JsonPropertyOrder(30)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("split-dbcontext-preview")]
        public bool SplitDbContextPreview { get; set; }

        [JsonPropertyOrder(40)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("use-schema-folders-preview")]
        public bool UseSchemaFoldersPreview { get; set; }

        [JsonPropertyOrder(50)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("use-schema-namespaces-preview")]
        public bool UseSchemaNamespacesPreview { get; set; }
    }
}
