using System.Collections.Generic;

namespace RevEng.Core.Abstractions
{
    public class FunctionModelFactoryOptions
    {
        public IEnumerable<string> Functions { get; set; }
        public bool FullModel { get; set; }
    }
}
