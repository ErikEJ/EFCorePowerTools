using System.Collections.Generic;

namespace RevEng.Core.Abstractions
{
    public class ModuleModelFactoryOptions
    {
        public IEnumerable<string> Modules { get; set; }
        public IEnumerable<string> ModulesUsingLegacyDiscovery { get; set; }
#pragma warning disable CA2227 // Collection properties should be read only
        public IDictionary<string, string> MappedModules { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public bool FullModel { get; set; }
        public bool DiscoverMultipleResultSets { get; set; }
        public bool UseLegacyResultSetDiscovery { get; set; }
        public bool UseDateOnlyTimeOnly { get; set; }
    }
}
