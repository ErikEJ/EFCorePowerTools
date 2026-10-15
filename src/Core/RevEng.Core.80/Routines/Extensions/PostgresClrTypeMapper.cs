using System;
using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Routines.Extensions;

public class PostgresClrTypeMapper : IClrTypeMapper
{
    public Type GetClrType(ModuleParameter parameter) => PostgresNpgsqlTypeExtensions.ClrTypeFromNpgsqlParameter(parameter);

    public Type GetClrType(ModuleResultElement resultElement) => PostgresNpgsqlTypeExtensions.GetClrType(resultElement);
}