using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RevEng.Common.Efcpt
{
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable SA1300 // Element should begin with upper-case letter
    public class EfcptConfig
    {
        [JsonPropertyName("$schema")]
        public string JsonSchema { get; set; }

        [JsonPropertyName("code-generation")]
        public CodeGeneration codegeneration { get; set; } = new CodeGeneration();

        [JsonPropertyName("names")]
        public Names names { get; set; } = new Names();

        [JsonPropertyName("file-layout")]
        public FileLayout filelayout { get; set; } = new FileLayout();

        [JsonPropertyName("type-mappings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TypeMappings typemappings { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Replacements replacements { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Table> tables { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<View> views { get; set; }

        [JsonPropertyName("stored-procedures")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<StoredProcedure> storedprocedures { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Function> functions { get; set; }
    }

    public class CodeGeneration
    {
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
        [JsonPropertyName("root-namespace")]
        public string rootnamespace { get; set; }

        [JsonPropertyName("dbcontext-name")]
        public string dbcontextname { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("dbcontext-namespace")]
        public string dbcontextnamespace { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
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

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool exclude { get; set; }
    }

    public class View
    {
        public string name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool exclude { get; set; }
    }

    public class StoredProcedure
    {
        public string name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool exclude { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("use-legacy-resultset-discovery")]
        public bool uselegacyresultsetdiscovery { get; set; }

        [JsonPropertyName("mapped-type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string mappedtype { get; set; }
    }

    public class Function
    {
        public string name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool exclude { get; set; }
    }
#pragma warning restore SA1402 // File may only contain a single type
#pragma warning restore SA1300 // Element should begin with upper-case letter
#pragma warning restore CA1819 // Properties should not return arrays
}
