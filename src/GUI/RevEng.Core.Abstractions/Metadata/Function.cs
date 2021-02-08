using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class Function
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public bool IsScalar { get; set; }

        public List<ModuleParameter> Parameters = new List<ModuleParameter>();
    }
}
