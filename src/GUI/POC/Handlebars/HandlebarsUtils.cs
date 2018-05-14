using System;
using HandlebarsDotNet.Compiler;
using System.Collections;
using System.Linq;

namespace HandlebarsDotNet
{
    public static class HandlebarsUtils
    {
        public static bool IsTruthy(object value)
        {
            return !IsFalsy(value);
        }

        public static bool IsFalsy(object value)
        {
            if (value is UndefinedBindingResult)
            {
                return true;
            }
            if (value == null)
            {
                return true;
            }
            else if (value is bool)
            {
                return !(bool)value;
            }
            else if (value is string)
            {
                if ((string)value == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (IsNumber(value))
            {
                return !System.Convert.ToBoolean(value);
            }
            return false;
        }

        public static bool IsTruthyOrNonEmpty(object value)
        {
            return !IsFalsyOrEmpty(value);
        }

        public static bool IsFalsyOrEmpty(object value)
        {
            if(IsFalsy(value))
            {
                return true;
            }
            else if (value is IEnumerable && ((IEnumerable)value).OfType<object>().Any() == false)
            {
                return true;
            }
            return false;
        }

        private static bool IsNumber(object value)
        {
            return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
        }
    }
}

