using System;
using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Routines.Extensions;

public interface IClrTypeMapper
{
    public Type GetClrType(ModuleParameter parameter);

    public Type GetClrType(ModuleResultElement resultElement);
}