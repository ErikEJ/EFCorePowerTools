using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
#pragma warning disable CA2227
    public class CliConfig
    {
        [JsonPropertyOrder(-1)]
        [JsonPropertyName("$schema")]
#pragma warning disable CA1822
        public string JsonSchema =>
            "https://raw.githubusercontent.com/ErikEJ/EFCorePowerTools/master/samples/efcpt-schema.json";
#pragma warning restore CA1822

        [JsonPropertyOrder(10)]
        [JsonPropertyName("code-generation")]
        public CodeGeneration CodeGeneration { get; set; } = new CodeGeneration();

        [JsonPropertyOrder(20)]
        [JsonPropertyName("file-layout")]
        public FileLayout FileLayout { get; set; } = new FileLayout();

        [JsonPropertyOrder(30)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("functions")]
        public List<Function> Functions { get; set; }

        [JsonPropertyOrder(40)]
        [JsonPropertyName("names")]
        public Names Names { get; set; } = new Names();

        [JsonPropertyOrder(50)]
        [JsonPropertyName("replacements")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Replacements Replacements { get; set; }

        [JsonPropertyOrder(60)]
        [JsonPropertyName("stored-procedures")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<StoredProcedure> StoredProcedures { get; set; }

        [JsonPropertyOrder(70)]
        [JsonPropertyName("tables")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Table> Tables { get; set; }

        [JsonPropertyOrder(80)]
        [JsonPropertyName("type-mappings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TypeMappings TypeMappings { get; set; }

        [JsonPropertyOrder(90)]
        [JsonPropertyName("views")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<View> Views { get; set; }
    }
#pragma warning restore CA2227
}
