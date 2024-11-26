using System;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;

namespace RevEng.Core.Routines.Functions
{
#pragma warning disable CA1812 // Internal class that is apparently never instantiated; this class is instantiated generically
    internal sealed class NotImplementedFunctionModelFactory : IFunctionModelFactory
#pragma warning restore CA1812 // Internal class that is apparently never instantiated
    {
        public RoutineModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            throw new NotSupportedException();
        }
    }
}
