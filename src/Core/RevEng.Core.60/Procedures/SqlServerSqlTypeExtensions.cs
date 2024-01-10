using NetTopologySuite.Geometries;
using RevEng.Core.Abstractions.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace RevEng.Core
{
    public static class SqlServerSqlTypeExtensions
    {
        private static readonly HashSet<SqlDbType> ScaleTypes = new HashSet<SqlDbType>
        {
            SqlDbType.Decimal,
            SqlDbType.Money,
            SqlDbType.SmallMoney,
        };

        private static readonly HashSet<SqlDbType> VarTimeTypes = new HashSet<SqlDbType>
        {
            SqlDbType.DateTimeOffset,
            SqlDbType.DateTime2,
            SqlDbType.Time,
        };

        private static readonly HashSet<SqlDbType> LengthRequiredTypes = new HashSet<SqlDbType>
        {
            SqlDbType.Binary,
            SqlDbType.VarBinary,
            SqlDbType.Char,
            SqlDbType.VarChar,
            SqlDbType.NChar,
            SqlDbType.NVarChar,
        };

        private static readonly ReadOnlyDictionary<string, string> SqlTypeAliases
        = new ReadOnlyDictionary<string, string>(
            new Dictionary<string, string>()
            {
                { "numeric", "decimal" },
                { "rowversion", "timestamp" },
                { "table type", "structured" },
                { "sql_variant", "variant" },
                { "geography", "udt" },
                { "geometry", "udt" },
                { "hierarchyid", "udt" },
                { "sysname", "nvarchar" },
            });

        public static bool UseDateOnlyTimeOnly { get; set; }

        public static bool IsScaleType(this SqlDbType sqlDbType)
        {
            return ScaleTypes.Contains(sqlDbType);
        }

        public static bool IsVarTimeType(this SqlDbType sqlDbType)
        {
            return VarTimeTypes.Contains(sqlDbType);
        }

        public static bool IsLengthRequiredType(this SqlDbType sqlDbType)
        {
            return LengthRequiredTypes.Contains(sqlDbType);
        }

        public static Type ClrType(this ModuleParameter storedProcedureParameter, bool asMethodParameter = false)
        {
            ArgumentNullException.ThrowIfNull(storedProcedureParameter);

            return GetClrType(storedProcedureParameter.StoreType, storedProcedureParameter.Nullable, asMethodParameter);
        }

        public static Type ClrType(this ModuleResultElement moduleResultElement)
        {
            ArgumentNullException.ThrowIfNull(moduleResultElement);

            return GetClrType(moduleResultElement.StoreType, moduleResultElement.Nullable);
        }

        public static SqlDbType DbType(this ModuleParameter storedProcedureParameter)
        {
            ArgumentNullException.ThrowIfNull(storedProcedureParameter);

            return GetSqlDbType(storedProcedureParameter.StoreType);
        }

        public static Type GetClrType(string storeType, bool isNullable, bool asParameter = false)
        {
            var sqlType = GetSqlDbType(storeType);

            var useDateOnlyTimeOnly = UseDateOnlyTimeOnly;

            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return isNullable ? typeof(long?) : typeof(long);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return isNullable ? typeof(bool?) : typeof(bool);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.DateTime2:
                    return isNullable ? typeof(DateTime?) : typeof(DateTime);

                case SqlDbType.Date:
                    if (useDateOnlyTimeOnly)
                    {
                        return isNullable ? typeof(DateOnly?) : typeof(DateOnly);
                    }

                    return isNullable ? typeof(DateTime?) : typeof(DateTime);

                case SqlDbType.Time:
                    if (useDateOnlyTimeOnly)
                    {
                        return isNullable ? typeof(TimeOnly?) : typeof(TimeOnly);
                    }

                    return isNullable ? typeof(TimeSpan?) : typeof(TimeSpan);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return isNullable ? typeof(decimal?) : typeof(decimal);

                case SqlDbType.Float:
                    return isNullable ? typeof(double?) : typeof(double);

                case SqlDbType.Int:
                    return isNullable ? typeof(int?) : typeof(int);

                case SqlDbType.Real:
                    return isNullable ? typeof(float?) : typeof(float);

                case SqlDbType.UniqueIdentifier:
                    return isNullable ? typeof(Guid?) : typeof(Guid);

                case SqlDbType.SmallInt:
                    return isNullable ? typeof(short?) : typeof(short);

                case SqlDbType.TinyInt:
                    return isNullable ? typeof(byte?) : typeof(byte);

                case SqlDbType.Variant:
                    return typeof(object);

                case SqlDbType.Udt:
                    switch (storeType)
                    {
                        case "geometry":
                        case "geography":
                            if (asParameter)
                            {
                                return typeof(byte[]);
                            }

                            return typeof(Geometry);

                        default:
                            return typeof(byte[]);
                    }

                case SqlDbType.Structured:
                    return typeof(DataTable);

                case SqlDbType.DateTimeOffset:
                    return isNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);

                default:
                    throw new ArgumentOutOfRangeException(nameof(storeType), $"storetype: {storeType}");
            }
        }

        private static SqlDbType GetSqlDbType(string storeType)
        {
            if (string.IsNullOrEmpty(storeType))
            {
                throw new ArgumentException("storeType not specified");
            }

            var cleanedTypeName = RemoveMatchingBraces(storeType);

            if (cleanedTypeName == null)
            {
                throw new ArgumentOutOfRangeException(nameof(storeType), $"Unable to remove braces: {storeType}");
            }

#pragma warning disable CA1308 // Normalize strings to uppercase
            if (SqlTypeAliases.TryGetValue(cleanedTypeName.ToLowerInvariant(), out string alias))
            {
                cleanedTypeName = alias;
            }
#pragma warning restore CA1308 // Normalize strings to uppercase

            if (!Enum.TryParse(cleanedTypeName, true, out SqlDbType result))
            {
                throw new ArgumentOutOfRangeException(nameof(storeType), $"cleanedTypeName: {cleanedTypeName}");
            }

            return result;
        }

        private static string RemoveMatchingBraces(string s)
        {
            var stack = new Stack<char>();
            int count = 0;
            foreach (char ch in s)
            {
                switch (ch)
                {
                    case '(':
                        count += 1;
                        stack.Push(ch);
                        break;
                    case ')':
                        if (count == 0)
                        {
                            stack.Push(ch);
                        }
                        else
                        {
                            char popped;
                            do
                            {
                                popped = stack.Pop();
                            }
                            while (popped != '(');
                            count -= 1;
                        }

                        break;
                    default:
                        stack.Push(ch);
                        break;
                }
            }

            return string.Join(string.Empty, stack.Reverse());
        }
    }
}
