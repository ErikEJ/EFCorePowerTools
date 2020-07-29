using System.Collections.Generic;

namespace RevEng.Core.Procedures.Model.Metadata
{
    public class Procedure
    {
        public string Name { get; set; }
        public string Schema { get; set; }

        public List<ProcedureParameter> Parameters = new List<ProcedureParameter>();
        public List<ProcedureResultElement> ResultElements = new List<ProcedureResultElement>();
    }
}
