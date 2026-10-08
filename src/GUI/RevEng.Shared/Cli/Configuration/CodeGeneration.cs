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
        [JsonPropertyName("file-line-endings")]
        public string FileLineEndings { get; set; } = "native";

        [JsonPropertyOrder(50)]
        [JsonPropertyName("merge-dacpacs")]
        public bool MergeDacpacs { get; set; }

        [JsonPropertyOrder(60)]
        [JsonPropertyName("refresh-object-lists")]
        public bool RefreshObjectLists { get; set; } = true;

        [JsonPropertyOrder(70)]
        [JsonPropertyName("remove-defaultsql-from-bool-properties")]
        public bool RemoveDefaultSqlFromBoolProperties { get; set; }

        [JsonPropertyOrder(80)]
        [JsonPropertyName("soft-delete-obsolete-files")]
        public bool SoftDeleteObsoleteFiles { get; set; } = true;

        [JsonPropertyOrder(90)]
        [JsonPropertyName("t4-template-path")]
        public string T4TemplatePath { get; set; }

        [JsonPropertyOrder(100)]
        [JsonPropertyName("type")]
        public string Type { get; set; } = "all";

        [JsonPropertyOrder(110)]
        [JsonPropertyName("use-alternate-stored-procedure-resultset-discovery")]
        public bool UseAlternateStoredProcedureResultsetDiscovery { get; set; }

        [JsonPropertyOrder(120)]
        [JsonPropertyName("use-stored-procedure-resultset-fallback")]
        public bool UseStoredProcedureResultSetFallback { get; set; } = true;

        [JsonPropertyOrder(130)]
        [JsonPropertyName("use-data-annotations")]
        public bool UseDataAnnotations { get; set; }

        [JsonPropertyOrder(140)]
        [JsonPropertyName("use-database-names")]
        public bool UseDatabaseNames { get; set; }

        [JsonPropertyOrder(145)]
        [JsonPropertyName("use-database-names-for-routines")]
        public bool UseDatabaseNamesForRoutines { get; set; } = true;

        [JsonPropertyOrder(150)]
        [JsonPropertyName("use-decimal-data-annotation-for-sproc-results")]
        public bool UseDecimalDataAnnotation { get; set; } = true;

        [JsonPropertyOrder(160)]
        [JsonPropertyName("use-inflector")]
        public bool UseInflector { get; set; } = true;

        [JsonPropertyOrder(170)]
        [JsonPropertyName("use-internal-access-modifiers-for-sprocs-and-functions")]
        public bool UseInternalAccessModifiersForSprocsAndFunctions { get; set; }

        [JsonPropertyOrder(180)]
        [JsonPropertyName("use-legacy-inflector")]
        public bool UseLegacyInflector { get; set; }

        [JsonPropertyOrder(190)]
        [JsonPropertyName("use-many-to-many-entity")]
        public bool UseManyToManyEntity { get; set; }

        [JsonPropertyOrder(200)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("use-no-navigations-preview")]
        public bool UseNoNavigationsPreview { get; set; }

        [JsonPropertyOrder(210)]
        [JsonPropertyName("use-nullable-reference-types")]
        public bool UseNullableReferenceTypes { get; set; } = true;

        [JsonPropertyOrder(220)]
        [JsonPropertyName("use-prefix-navigation-naming")]
        public bool UsePrefixNavigationNaming { get; set; }

        [JsonPropertyOrder(230)]
        [JsonPropertyName("use-typed-tvp-parameters")]
        public bool UseTypedTvpParameters { get; set; } = true;

        [JsonPropertyOrder(240)]
        [JsonPropertyName("use-t4")]
        public bool UseT4 { get; set; }

        [JsonPropertyOrder(250)]
        [JsonPropertyName("use-t4-split")]
        public bool UseT4Split { get; set; }
    }
}
