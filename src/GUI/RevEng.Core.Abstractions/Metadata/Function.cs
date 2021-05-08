using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class Function : SqlObjectBase
    {
        public bool IsScalar { get; set; }

        public List<ModuleParameter> Parameters = new List<ModuleParameter>();
    }
}
