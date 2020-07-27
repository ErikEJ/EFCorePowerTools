using RevEng.Core.Procedures.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RevEng.Core
{
    public static class StoredProcedureExtensions
    {
        public static Type ClrType(this StoredProcedureParameter storedProcedureParameter)
        {
            return GetClrType(storedProcedureParameter.StoreType, storedProcedureParameter.Nullable);

            //var type = clrType.UnwrapNullableType();
            //var nullable = clrType.IsNullableValueType() ? "?" : string.Empty;

            //return $"{type.Name}{nullable}";
        }

        public static SqlDbType DbType(this StoredProcedureParameter storedProcedureParameter)
        {
            return GetSqlDbType(storedProcedureParameter.StoreType);
        }

        public static Type ClrType(this StoredProcedureResultElement storedProcedureResultElement)
        {
            return GetClrType(storedProcedureResultElement.StoreType, storedProcedureResultElement.Nullable);

            //var type = clrType.UnwrapNullableType();
            //var nullable = clrType.IsNullableValueType() ? "?" : string.Empty;

            //return $"{type.Name}{nullable}";
        }

        public static SqlDbType DbType(this StoredProcedureResultElement storedProcedureResultElement)
        {
            return GetSqlDbType(storedProcedureResultElement.StoreType);
        }

        private static SqlDbType GetSqlDbType(string storeType)
        {
            return (SqlDbType)Enum.Parse(typeof(SqlDbType), storeType, true);
        }

        private static Type GetClrType(string storeType, bool isNullable)
        {
            var cleanedTypeName = RemoveMatchingBraces(storeType);

            var sqlType = GetSqlDbType(cleanedTypeName);

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
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    return isNullable ? typeof(DateTime?) : typeof(DateTime);

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
                case SqlDbType.Udt:
                    return typeof(object);

                case SqlDbType.Structured:
                    return typeof(DataTable);

                case SqlDbType.DateTimeOffset:
                    return isNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
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
                            stack.Push(ch);
                        else
                        {
                            char popped;
                            do
                            {
                                popped = stack.Pop();
                            } while (popped != '(');
                            count -= 1;
                        }
                        break;
                    default:
                        stack.Push(ch);
                        break;
                }
            }
            return string.Join("", stack.Reverse());
        }
    }
}
