namespace RevEng.Common
{
    public static class RuleFiles
    {
        /// <summary> table and column renaming file. </summary>
        public const string RenamingFilename = "efpt.renaming.json";

        /// <summary> property renaming rules file (for renaming navigations) </summary>
        public const string PropertyFilename = "efpt.property-renaming.json";

        /// <summary> enum mapping rule file. </summary>
        public const string EnumMappingFilename = "efpt.enum-mapping.json";

        public const string GeneralEfPtJsonFileMask = "efpt.*.json";
    }
}
