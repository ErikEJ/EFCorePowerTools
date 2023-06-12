using System.Collections.Generic;

namespace RevEng.Common
{
    public class NuGetPackage
    {
        public string PackageId { get; set; }
        public string Version { get; set; }
        public string UseMethodName { get; set; }
        public bool Installed { get; set; }
        public bool IsMainProviderPackage { get; set; }
#pragma warning disable CA2227 // Collection properties should be read only
        public IList<DatabaseType> DatabaseTypes { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
