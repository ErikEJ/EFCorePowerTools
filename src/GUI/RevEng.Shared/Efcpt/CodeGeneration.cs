using System.Text.Json.Serialization;

namespace RevEng.Common.Efcpt
{
    public class CodeGeneration
    {
        [JsonPropertyName("enable-on-configuring")]
        public bool EnableOnConfiguring { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "all";

        [JsonPropertyName("use-database-names")]
        public bool UseDatabaseNames { get; set; }

        [JsonPropertyName("use-data-annotations")]
        public bool UseDataAnnotations { get; set; }

        [JsonPropertyName("use-nullable-reference-types")]
        public bool UseNullableReferenceTypes { get; set; } = true;

        [JsonPropertyName("use-inflector")]
        public bool UseInflector { get; set; }

        [JsonPropertyName("use-legacy-inflector")]
        public bool UseLegacyInflector { get; set; }

        [JsonPropertyName("use-many-to-many-entity")]
        public bool UseManyToManyEntity { get; set; }

        [JsonPropertyName("use-t4")]
        public bool UseT4 { get; set; }

        [JsonPropertyName("remove-defaultsql-from-bool-properties")]
        public bool RemoveDefaultSqlFromBoolProperties { get; set; }

        [JsonPropertyName("soft-delete-obsolete-files")]
        public bool SoftDeleteObsoleteFiles { get; set; }

        [JsonPropertyName("discover-multiple-stored-procedure-resultsets-preview")]
        public bool DiscoverMultipleStoredProcedureResultsetsPreview { get; set; }

        [JsonPropertyName("use-alternate-stored-procedure-resultset-discovery")]
        public bool UseAlternateStoredProcedureResultsetDiscovery { get; set; }
    }
}
