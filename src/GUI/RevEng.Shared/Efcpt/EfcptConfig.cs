using System.Text.Json.Serialization;

namespace RevEng.Common.Efcpt
{
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable SA1300 // Element should begin with upper-case letter
    public class EfcptConfig
    {
        [JsonPropertyName("$schema")]
        public string JsonShema { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Table[] tables { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public View[] views { get; set; }

        [JsonPropertyName("stored-procedures")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public StoredProcedures[] storedprocedures { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Function[] functions { get; set; }

        [JsonPropertyName("code-generation")]
        [JsonInclude]
        public CodeGeneration codegeneration { get; set; } = new CodeGeneration();

        [JsonPropertyName("names")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Names names { get; set; }

        [JsonPropertyName("file-layout")]
        [JsonInclude]
        public FileLayout filelayout { get; set; } = new FileLayout();

        [JsonPropertyName("type-mappings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TypeMappings typemappings { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Replacements replacements { get; set; }
    }

    public class CodeGeneration
    {
        [JsonPropertyName("target-version")]
        public int targetversion { get; set; }

        [JsonPropertyName("enable-on-configuring")]
        public bool enableonconfiguring { get; set; }

        public string type { get; set; } = "all";

        [JsonPropertyName("use-database-names")]
        public bool usedatabasenames { get; set; }

        [JsonPropertyName("use-data-annotations")]
        public bool usedataannotations { get; set; }

        [JsonPropertyName("use-nullable-reference-types")]
        public bool usenullablereferencetypes { get; set; } = true;

        [JsonPropertyName("use-inflector")]
        public bool useinflector { get; set; }

        [JsonPropertyName("use-legacy-inflector")]
        public bool uselegacyinflector { get; set; }

        [JsonPropertyName("use-many-to-many-entity")]
        public bool usemanytomanyentity { get; set; }

        [JsonPropertyName("use-t4")]
        public bool uset4 { get; set; }

        [JsonPropertyName("remove-defaultsql-from-bool-properties")]
        public bool removedefaultsqlfromboolproperties { get; set; }

        [JsonPropertyName("soft-delete-obsolete-files")]
        public bool softdeleteobsoletefiles { get; set; }

        [JsonPropertyName("discover-multiple-stored-procedure-resultsets-preview")]
        public bool discovermultiplestoredprocedureresultsetspreview { get; set; }

        [JsonPropertyName("use-alternate-stored-procedure-resultset-discovery")]
        public bool usealternatestoredprocedureresultsetdiscovery { get; set; }
    }

    public class Names
    {
        [JsonPropertyName("dbcontext-name")]
        public string dbcontextname { get; set; }
        [JsonPropertyName("dbcontext-namespace")]
        public string dbcontextnamespace { get; set; }
        [JsonPropertyName("model-namespace")]
        public string modelnamespace { get; set; }
    }

    public class FileLayout
    {
        [JsonPropertyName("output-path")]
        public string outputpath { get; set; } = "Models";
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("output-dbcontext-path")]
        public string outputdbcontextpath { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("split-dbcontext-preview")]
        public bool splitdbcontextpreview { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("use-schema-folders-preview")]
        public bool useschemafolderspreview { get; set; }
    }

    public class TypeMappings
    {
        [JsonPropertyName("use-DateOnly-TimeOnly")]
        public bool useDateOnlyTimeOnly { get; set; }
        [JsonPropertyName("use-HierarchyId")]
        public bool useHierarchyId { get; set; }
        [JsonPropertyName("use-spatial")]
        public bool usespatial { get; set; }
        [JsonPropertyName("use-NodaTime")]
        public bool useNodaTime { get; set; }
    }

    public class Replacements
    {
        [JsonPropertyName("preserve-casing-with-regex")]
        public bool preservecasingwithregex { get; set; }
        [JsonPropertyName("uncountable-words")]
        public string[] uncountablewords { get; set; }
    }

    public class Table
    {
        public string name { get; set; }
        public bool exclude { get; set; }
    }

    public class View
    {
        public string name { get; set; }
        public bool exclude { get; set; }
    }

    public class StoredProcedures
    {
        public string name { get; set; }
        public bool exclude { get; set; }
        [JsonPropertyName("use-legacy-resultset-discovery")]
        public bool uselegacyresultsetdiscovery { get; set; }
        [JsonPropertyName("mapped-type")]
        public string mappedtype { get; set; }
    }

    public class Function
    {
        public string name { get; set; }
        public bool exclude { get; set; }
    }
#pragma warning restore SA1402 // File may only contain a single type
#pragma warning restore SA1300 // Element should begin with upper-case letter
#pragma warning restore CA1819 // Properties should not return arrays
}
