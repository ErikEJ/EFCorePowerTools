using System;
using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Routines.Extensions;

public class SqlServerClrTypeMapper : IClrTypeMapper
{
    public Type GetClrType(ModuleParameter parameter) => SqlServerSqlTypeExtensions.ClrTypeFromSqlParameter(parameter);

    public Type GetClrType(ModuleResultElement resultElement) => SqlServerSqlTypeExtensions.GetClrType(resultElement);
}