using System.Collections.Generic;

namespace RevEng.Core.Abstractions
{
    public class ModuleModelFactoryOptions
    {
        public IEnumerable<string> Modules { get; set; }
        public bool FullModel { get; set; }
        public bool DiscoverMultipleResultSets { get; set; }
    }
}
