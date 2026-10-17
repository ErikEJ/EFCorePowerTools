namespace EFCorePowerTools;

internal static class PkgCmdIDList
{
    // C# project context menu
    public const uint CmdIdReverseEngineerCodeFirst = 0x001;
    public const uint CmdIdWizardPoc = 0x002;
    public const uint CmdIdReverseEngineerCodeFirstRefresh = 0x007;
    public const uint CmdIdReverseEngineerDab = 0x012;
    public const uint CmdIdAbout = 0x006;
    public const uint CmdIdDgmlBuild = 0x0100;
    public const uint CmdIdDgmlNuget = 0x0200;
    public const uint CmdIdSqlBuild = 0x0400;
    public const uint CmdIdDebugViewBuild = 0x0450;
    public const uint CmdIdDbCompare = 0x008;
    public const uint CmdIdDbDgml = 0x009;
    public const uint CmdIdOptions = 0x010;
    public const uint CmdIdDbErDiagram = 0x011;

    // Database project context menu
    public const uint CmdIdSqlprojCreate = 0x109;
    public const uint CmdIdSqlprojAnalyze = 0x110;

    // Server Explorer context menu
    public const uint CmdIdServerExplorerReverseEngineer = 257;
    public const uint CmdIdServerExplorerDiagram = 258;
    public const uint CmdIdServerExplorerAnalyze = 260;

    // Config file (efpt.config.json/dab-config.json) context menu
    public const uint CmdIdReverseEngineerEdit = 0x1101;
    public const uint CmdIdReverseEngineerRefresh = 0x1102;
    public const uint CmdIdDabStart = 0x1103;
}