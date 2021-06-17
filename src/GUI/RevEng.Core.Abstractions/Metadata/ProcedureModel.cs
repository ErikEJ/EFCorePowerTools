using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class ProcedureModel : ModuleModel<Procedure>
    {
        public List<Procedure> Procedures
        {
            get => Routines;
            set => Routines = value;
        }
    }
}
