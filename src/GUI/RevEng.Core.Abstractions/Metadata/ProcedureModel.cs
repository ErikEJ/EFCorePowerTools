using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class ProcedureModel
    {
        public List<Procedure> Procedures { get; set; }
        public List<string> Errors { get; set; }
    }
}
