// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace System
{
    [DebuggerStepThrough]
    internal static class SharedTypeExtensions
    {
        private static readonly Dictionary<Type, string> _builtInTypeNames = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(object), "object" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(string), "string" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(ushort), "ushort" },
            { typeof(void), "void" }
        };

        public static Type UnwrapNullableType(this Type type) => Nullable.GetUnderlyingType(type) ?? type;

        public static bool IsNullableValueType(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static bool IsNullableType(this Type type)
            => !type.IsValueType || type.IsNullableValueType();

        public static Type TryGetSequenceType(this Type type)
            => type.TryGetElementType(typeof(IEnumerable<>))
                ?? type.TryGetElementType(typeof(IAsyncEnumerable<>));

        public static Type TryGetElementType(this Type type, Type interfaceOrBaseType)
        {
            if (type.IsGenericTypeDefinition)
            {
                return null;
            }

            var types = GetGenericTypeImplementations(type, interfaceOrBaseType);

            Type singleImplementation = null;
            foreach (var implementation in types)
            {
                if (singleImplementation == null)
                {
                    singleImplementation = implementation;
                }
                else
                {
                    singleImplementation = null;
                    break;
                }
            }

            return singleImplementation?.GenericTypeArguments.FirstOrDefault();
        }

        public static bool IsCompatibleWith(this Type propertyType, Type fieldType)
        {
            if (propertyType.IsAssignableFrom(fieldType)
                || fieldType.IsAssignableFrom(propertyType))
            {
                return true;
            }

            var propertyElementType = propertyType.TryGetSequenceType();
            var fieldElementType = fieldType.TryGetSequenceType();

            return propertyElementType != null
                && fieldElementType != null
                && IsCompatibleWith(propertyElementType, fieldElementType);
        }

        public static IEnumerable<Type> GetGenericTypeImplementations(this Type type, Type interfaceOrBaseType)
        {
            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsGenericTypeDefinition)
            {
                var baseTypes = interfaceOrBaseType.GetTypeInfo().IsInterface
                    ? typeInfo.ImplementedInterfaces
                    : type.GetBaseTypes();
                foreach (var baseType in baseTypes)
                {
                    if (baseType.IsGenericType
                        && baseType.GetGenericTypeDefinition() == interfaceOrBaseType)
                    {
                        yield return baseType;
                    }
                }

                if (type.IsGenericType
                    && type.GetGenericTypeDefinition() == interfaceOrBaseType)
                {
                    yield return type;
                }
            }
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            type = type.BaseType;

            while (type != null)
            {
                yield return type;

                type = type.BaseType;
            }
        }

        public static IEnumerable<string> GetNamespacesEx([NotNull] this Type type)
        {
            if (_builtInTypeNames.ContainsKey(type))
            {
                yield break;
            }

            yield return type.Namespace;

            if (type.IsGenericType)
            {
                foreach (var typeArgument in type.GenericTypeArguments)
                {
                    foreach (var ns in typeArgument.GetNamespacesEx())
                    {
                        yield return ns;
                    }
                }
            }
        }
    }
}