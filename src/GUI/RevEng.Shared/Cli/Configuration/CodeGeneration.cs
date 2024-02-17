using System.Text.Json.Serialization;

namespace RevEng.Common.Cli.Configuration
{
    public class CodeGeneration
    {
        [JsonPropertyOrder(10)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("discover-multiple-stored-procedure-resultsets-preview")]
        public bool DiscoverMultipleStoredProcedureResultsetsPreview { get; set; }

        [JsonPropertyOrder(20)]
        [JsonPropertyName("enable-on-configuring")]
        public bool EnableOnConfiguring { get; set; }

        [JsonPropertyOrder(30)]
        [JsonPropertyName("generate-mermaid-diagram")]
        public bool GenerateMermaidDiagram { get; set; }

        [JsonPropertyOrder(40)]
        [JsonPropertyName("merge-dacpacs")]
        public bool MergeDacpacs { get; set; }

        [JsonPropertyOrder(50)]
        [JsonPropertyName("refresh-object-lists")]
        public bool RefreshObjectLists { get; set; } = true;

        [JsonPropertyOrder(60)]
        [JsonPropertyName("remove-defaultsql-from-bool-properties")]
        public bool RemoveDefaultSqlFromBoolProperties { get; set; }

        [JsonPropertyOrder(70)]
        [JsonPropertyName("soft-delete-obsolete-files")]
        public bool SoftDeleteObsoleteFiles { get; set; } = true;

        [JsonPropertyOrder(80)]
        [JsonPropertyName("t4-template-path")]
        public string T4TemplatePath { get; set; }

        [JsonPropertyOrder(90)]
        [JsonPropertyName("type")]
        public string Type { get; set; } = "all";

        [JsonPropertyOrder(100)]
        [JsonPropertyName("use-alternate-stored-procedure-resultset-discovery")]
        public bool UseAlternateStoredProcedureResultsetDiscovery { get; set; }

        [JsonPropertyOrder(110)]
        [JsonPropertyName("use-data-annotations")]
        public bool UseDataAnnotations { get; set; }

        [JsonPropertyOrder(120)]
        [JsonPropertyName("use-database-names")]
        public bool UseDatabaseNames { get; set; }

        [JsonPropertyOrder(130)]
        [JsonPropertyName("use-decimal-data-annotation-for-sproc-results")]
        public bool UseDecimalDataAnnotation { get; set; } = true;

        [JsonPropertyOrder(140)]
        [JsonPropertyName("use-inflector")]
        public bool UseInflector { get; set; } = true;

        [JsonPropertyOrder(150)]
        [JsonPropertyName("use-legacy-inflector")]
        public bool UseLegacyInflector { get; set; }

        [JsonPropertyOrder(160)]
        [JsonPropertyName("use-many-to-many-entity")]
        public bool UseManyToManyEntity { get; set; }

        [JsonPropertyOrder(170)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("use-no-navigations-preview")]
        public bool UseNoNavigationsPreview { get; set; }

        [JsonPropertyOrder(180)]
        [JsonPropertyName("use-nullable-reference-types")]
        public bool UseNullableReferenceTypes { get; set; } = true;

        [JsonPropertyOrder(190)]
        [JsonPropertyName("use-prefix-navigation-naming")]
        public bool UsePrefixNavigationNaming { get; set; }

        [JsonPropertyOrder(200)]
        [JsonPropertyName("use-t4")]
        public bool UseT4 { get; set; }
    }
}
