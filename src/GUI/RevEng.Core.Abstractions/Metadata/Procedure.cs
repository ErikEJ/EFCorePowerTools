using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class Procedure : SqlObjectBase
    {
        public bool HasValidResultSet { get; set; }

        public List<ModuleParameter> Parameters = new List<ModuleParameter>();
        public List<ProcedureResultElement> ResultElements = new List<ProcedureResultElement>();
    }
}
