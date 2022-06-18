using System.Collections.Generic;

namespace RevEng.Core.Abstractions
{
    public class ModuleModelFactoryOptions
    {
        public IEnumerable<string> Modules { get; set; }
        public IEnumerable<string> ModulesUsingLegacyDiscovery { get; set; }
        public IDictionary<string, string> MappedModules { get; set; }
        public bool FullModel { get; set; }
        public bool DiscoverMultipleResultSets { get; set; }
        public bool UseLegacyResultSetDiscovery { get; set; }
    }
}
