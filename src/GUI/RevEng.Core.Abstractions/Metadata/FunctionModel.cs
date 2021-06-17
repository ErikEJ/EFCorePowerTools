using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class FunctionModel : ModuleModel<Function>
    {
        public List<Function> Functions
        {
            get => Routines;
            set => Routines = value;
        }
    }
}
