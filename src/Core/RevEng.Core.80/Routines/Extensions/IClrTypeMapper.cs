using System;
using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Routines.Extensions
{
    public interface IClrTypeMapper
    {
        Type GetClrType(ModuleParameter parameter);
        Type GetClrType(ModuleResultElement resultElement);
    }
}
