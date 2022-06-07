using System.Collections.Generic;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions;

namespace RevEng.Core.Procedures
{
    public class NotImplementedProcedureScaffolder : IProcedureScaffolder
    {
        public SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpaceValue, bool useAsyncCalls)
        {
            throw new NotSupportedException();
        }

        public ScaffoldedModel ScaffoldModel(RoutineModel model, ModuleScaffolderOptions scaffolderOptions, ref List<string> errors)
        {
            throw new NotSupportedException();
        }
    }
}
