using System.Text.Json.Serialization;

namespace RevEng.Common.Efcpt
{
    public class FileLayout
    {
        [JsonPropertyName("output-path")]
        public string OutputPath { get; set; } = "Models";

        [JsonPropertyName("output-dbcontext-path")]
        public string OutputDbContextPath { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("split-dbcontext-preview")]
        public bool SplitDbContextPreview { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("use-schema-folders-preview")]
        public bool UseSchemaFoldersPreview { get; set; }
    }
}
