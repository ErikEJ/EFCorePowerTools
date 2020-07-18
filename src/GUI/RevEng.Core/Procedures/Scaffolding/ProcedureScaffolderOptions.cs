using System.Collections.Generic;

namespace RevEng.Core.Procedures.Scaffolding
{
    public class ProcedureScaffolderOptions
    {
        public virtual IEnumerable<string> Procedures { get; }
        public virtual string ContextName { get; set; }
        public virtual string ContextNamespace { get; set; }
        public virtual string ModelNamespace { get; set; }
    }
}
