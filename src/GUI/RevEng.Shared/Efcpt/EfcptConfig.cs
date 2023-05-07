using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RevEng.Common.Efcpt
{
#pragma warning disable CA2227
    public class EfcptConfig
    {
        [JsonPropertyName("$schema")]
#pragma warning disable CA1822
        public string JsonSchema =>
            "https://raw.githubusercontent.com/ErikEJ/EFCorePowerTools/master/samples/efcpt-schema.json";
#pragma warning restore CA1822
        [JsonPropertyName("code-generation")]
        public CodeGeneration CodeGeneration { get; set; } = new CodeGeneration();

        [JsonPropertyName("names")]
        public Names Names { get; set; } = new Names();

        [JsonPropertyName("file-layout")]
        public FileLayout FileLayout { get; set; } = new FileLayout();

        [JsonPropertyName("type-mappings")]
        public TypeMappings TypeMappings { get; set; }

        [JsonPropertyName("replacements")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Replacements Replacements { get; set; }

        [JsonPropertyName("tables")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Table> Tables { get; set; }

        [JsonPropertyName("views")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<View> Views { get; set; }

        [JsonPropertyName("stored-procedures")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<StoredProcedure> StoredProcedures { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("functions")]
        public List<Function> Functions { get; set; }
    }
#pragma warning restore CA2227
}
