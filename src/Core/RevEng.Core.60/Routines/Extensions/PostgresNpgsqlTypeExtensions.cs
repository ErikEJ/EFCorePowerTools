using NetTopologySuite.Geometries;
using NpgsqlTypes;
using RevEng.Core.Abstractions.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RevEng.Core.Routines.Extensions
{
    public static class PostgresNpgsqlTypeExtensions
    {
        private static readonly HashSet<NpgsqlDbType> ScaleTypes =
        [
            NpgsqlDbType.Numeric,
            NpgsqlDbType.Money,
        ];

        private static readonly HashSet<NpgsqlDbType> VarTimeTypes =
        [
            NpgsqlDbType.TimestampTz,
            NpgsqlDbType.Timestamp,
            NpgsqlDbType.Time,
        ];

        private static readonly HashSet<NpgsqlDbType> LengthRequiredTypes =
        [
            NpgsqlDbType.Varchar,
            NpgsqlDbType.Char,
        ];

        private static readonly ReadOnlyDictionary<string, string> SqlTypeAliases
        = new ReadOnlyDictionary<string, string>(
            new Dictionary<string, string>()
            {
                { "bool", "boolean" },
                { "decimal", "numeric" },
                { "double precision", "double" },
                { "float8", "double" },
                { "character varying", "varchar" },
                { "character", "char" },
                { "float4", "real" },
                { "int", "integer" },
                { "int4", "integer" },
                { "timestamp with time zone", "timestamptz" },
                { "time with time zone", "timetz" },
                { "timestamp without time zone", "timestamp" },
                { "time without time zone", "time" },

                ////{ "character varying[]", "text[]" },
                ////{ "character[]", "char[]" },
                ////{ "timestamp with time zone[]", "timestamp[]" },
                ////{ "timestamp without time zone[]", "timestamp[]" },
                ////{ "time with time zone[]", "time[]" },
                ////{ "time without time zone[]", "time[]" },
                ////{ "double precision[]", "float8[]" },
                ////{ "real[]", "float4[]" },
                ////{ "integer[]", "int[]" },
                ////{ "boolean[]", "bool[]" },
            });

        public static bool UseDateOnlyTimeOnly { get; set; }

        public static bool IsScaleType(this NpgsqlDbType dbType)
        {
            return ScaleTypes.Contains(dbType);
        }

        public static bool IsVarTimeType(this NpgsqlDbType dbType)
        {
            return VarTimeTypes.Contains(dbType);
        }

        public static bool IsLengthRequiredType(this NpgsqlDbType dbType)
        {
            return LengthRequiredTypes.Contains(dbType);
        }

        public static Type ClrTypeFromNpgsqlParameter(this ModuleParameter storedProcedureParameter, bool asMethodParameter = false)
        {
            ArgumentNullException.ThrowIfNull(storedProcedureParameter);

            return GetClrType(storedProcedureParameter.StoreType, storedProcedureParameter.Nullable, asMethodParameter);
        }

        public static NpgsqlDbType GetNpgsqlDbType(this ModuleParameter storedProcedureParameter)
        {
            ArgumentNullException.ThrowIfNull(storedProcedureParameter);

            return GetNpgsqlDbType(storedProcedureParameter.StoreType);
        }

        public static Type GetClrType(this ModuleResultElement moduleResultElement)
        {
            ArgumentNullException.ThrowIfNull(moduleResultElement);

            return GetClrType(moduleResultElement.StoreType, moduleResultElement.Nullable);
        }

        public static Type GetClrType(string storeType, bool isNullable, bool asParameter = false)
        {
            var sqlType = GetNpgsqlDbType(storeType);

            var useDateOnlyTimeOnly = UseDateOnlyTimeOnly;

            switch (sqlType)
            {
                case NpgsqlDbType.Bigint:
                    return isNullable ? typeof(long?) : typeof(long);

                case NpgsqlDbType.Bytea:
                    return typeof(byte[]);

                case NpgsqlDbType.Boolean:
                    return isNullable ? typeof(bool?) : typeof(bool);

                case NpgsqlDbType.Char:
                case NpgsqlDbType.Text:
                case NpgsqlDbType.Varchar:
                case NpgsqlDbType.Json:
                case NpgsqlDbType.Jsonb:
                case NpgsqlDbType.Xml:
                    return typeof(string);

                case NpgsqlDbType.Date:
                    if (useDateOnlyTimeOnly)
                    {
                        return isNullable ? typeof(DateOnly?) : typeof(DateOnly);
                    }

                    return isNullable ? typeof(DateTime?) : typeof(DateTime);

                case NpgsqlDbType.Double:
                    return isNullable ? typeof(double?) : typeof(double);

                case NpgsqlDbType.Integer:
                    return isNullable ? typeof(int?) : typeof(int);

                case NpgsqlDbType.Numeric:
                case NpgsqlDbType.Money:
                    return isNullable ? typeof(decimal?) : typeof(decimal);

                case NpgsqlDbType.Real:
                    return isNullable ? typeof(float?) : typeof(float);

                case NpgsqlDbType.Smallint:
                    return isNullable ? typeof(short?) : typeof(short);

                case NpgsqlDbType.Time:
                    if (useDateOnlyTimeOnly)
                    {
                        return isNullable ? typeof(TimeOnly?) : typeof(TimeOnly);
                    }

                    return isNullable ? typeof(TimeSpan?) : typeof(TimeSpan);

                case NpgsqlDbType.TimestampTz:
                case NpgsqlDbType.Timestamp:
                    return isNullable ? typeof(DateTime?) : typeof(DateTime);

                case NpgsqlDbType.Uuid:
                    return isNullable ? typeof(Guid?) : typeof(Guid);

                default:
                    throw new ArgumentOutOfRangeException(nameof(storeType), $"storetype: {storeType}");
            }
        }

        private static NpgsqlDbType GetNpgsqlDbType(string storeType)
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
            if (SqlTypeAliases.TryGetValue(cleanedTypeName.ToLowerInvariant(), out var alias))
            {
                cleanedTypeName = alias;
            }
#pragma warning restore CA1308 // Normalize strings to uppercase

            if (!Enum.TryParse(cleanedTypeName, true, out NpgsqlDbType result))
            {
                throw new ArgumentOutOfRangeException(nameof(storeType), $"cleanedTypeName: {cleanedTypeName}");
            }

            return result;
        }

        private static string RemoveMatchingBraces(string s)
        {
            var stack = new Stack<char>();
            var count = 0;
            foreach (var ch in s)
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
