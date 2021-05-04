using System;

namespace EFCorePowerTools
{
    internal static class GuidList
    {
        public const string guidDbContextPackagePkgString = "f4c4712c-ceae-4803-8e52-0e2049d5de9f";
        public const string guidDbContextPackageCmdSetString = "c769a05d-8d51-4919-bfe6-5f35a0eaf2ba";
        public const string guidOptionsPageGeneral = "DC8ED597-235B-4CBB-A10E-F3F8C82D89C0";

        public static readonly Guid guidDbContextPackageCmdSet = new Guid(guidDbContextPackageCmdSetString);

        public const string guidReverseEngineerMenuString = "74bcf1bb-979c-408d-adcf-718c16e8f09e";
        public static readonly Guid guidReverseEngineerMenu = new Guid(guidReverseEngineerMenuString);

    }
}