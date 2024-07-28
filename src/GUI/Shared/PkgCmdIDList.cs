// ReSharper disable InconsistentNaming
namespace EFCorePowerTools
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:Const field names should begin with upper-case letter", Justification = "Reviewed")]
    internal static class PkgCmdIDList
    {
        // C# project context menu
        public const uint cmdidReverseEngineerCodeFirst = 0x001;
        public const uint cmdidReverseEngineerCodeFirstRefresh = 0x007;
        public const uint cmdidReverseEngineerDab = 0x012;
        public const uint cmdidAbout = 0x006;
        public const uint cmdidDgmlBuild = 0x0100;
        public const uint cmdidDgmlNuget = 0x0200;
        public const uint cmdidT4Drop = 0x0250;
        public const uint cmdidSqlBuild = 0x0400;
        public const uint cmdidDebugViewBuild = 0x0450;
        public const uint cmdidDbCompare = 0x008;
        public const uint cmdidDbDgml = 0x009;
        public const uint cmdidOptions = 0x010;
        public const uint cmdidDbErDiagram = 0x011;

        // Database project context menu
        public const uint cmdidSqlprojCreate = 0x109;
        public const uint cmdidSqlprojAnalyze = 0x110;

        // Server Explorer context menu
        public const uint cmdidServerExplorerReverseEngineer = 257;
        public const uint cmdidServerExplorerDiagram = 258;
        public const uint cmdidServerExplorerAnalyze = 260;

        // Config file (efpt.config.json) context menu
        public const uint cmdidReverseEngineerEdit = 0x1101;
        public const uint cmdidReverseEngineerRefresh = 0x1102;
    }
}
