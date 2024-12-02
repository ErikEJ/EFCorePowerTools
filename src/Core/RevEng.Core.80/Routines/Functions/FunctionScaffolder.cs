using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Extensions;

namespace RevEng.Core.Routines.Functions
{
    public abstract class FunctionScaffolder : RoutineScaffolder
    {
        protected FunctionScaffolder([System.Diagnostics.CodeAnalysis.NotNull] ICSharpHelper code, IClrTypeMapper typeMapper)
            : base(code, typeMapper)
        {
        }

        public new string FileNameSuffix { get; set; }

        public new SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpaceValue, bool useAsyncCalls)
        {
            return base.Save(scaffoldedModel, outputDir, nameSpaceValue, useAsyncCalls);
        }

        public new ScaffoldedModel ScaffoldModel(RoutineModel model, ModuleScaffolderOptions scaffolderOptions, List<string> schemas, ref List<string> errors)
        {
            return base.ScaffoldModel(model, scaffolderOptions, schemas, ref errors);
        }
    }
}
