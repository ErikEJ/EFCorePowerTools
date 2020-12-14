using System.Collections.Generic;

namespace RevEng.Core.Abstractions
{
    public class ProcedureModelFactoryOptions
    {
        public IEnumerable<string> Procedures { get; set; }
        public bool FullModel { get; set; }
    }
}
