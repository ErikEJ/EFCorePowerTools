using System;

namespace EFCorePowerTools
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:Const field names should begin with upper-case letter", Justification = "Reviewed")]
    internal static class GuidList
    {
        public const string guidDbContextPackagePkgString = "f4c4712c-ceae-4803-8e52-0e2049d5de9f";
        public const string guidDbContextPackageCmdSetString = "c769a05d-8d51-4919-bfe6-5f35a0eaf2ba";
        public const string guidOptionsPageGeneral = "DC8ED597-235B-4CBB-A10E-F3F8C82D89C0";

        public const string guidReverseEngineerMenuString = "74bcf1bb-979c-408d-adcf-718c16e8f09e";
        public static readonly Guid GuidReverseEngineerMenu = new Guid(guidReverseEngineerMenuString);
        public static readonly Guid GuidDbContextPackageCmdSet = new Guid(guidDbContextPackageCmdSetString);
        public static readonly Guid GuidDbContextPackage = new Guid(guidDbContextPackagePkgString);
        public static readonly Guid GuidSqlprojCreate = new Guid("9a55a2b4-3e29-4359-882b-fa5f51c09301");
        public static readonly Guid GuidServerExplorerCreate = new Guid("946311de-35f2-4379-84e2-91867976faf9");
    }
}
