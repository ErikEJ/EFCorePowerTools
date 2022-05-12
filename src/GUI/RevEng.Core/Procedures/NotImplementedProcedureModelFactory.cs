using System;
using System.Collections.Generic;
using System.Text;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;

namespace RevEng.Core.Procedures
{
    internal class NotImplementedProcedureModelFactory : IProcedureModelFactory
    {
        public RoutineModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            throw new NotSupportedException();
        }
    }
}
