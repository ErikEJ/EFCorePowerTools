// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Modifications copyright(C) 2017 Tony Sneed.

using System;
using System.Diagnostics;
using System.Reflection;

namespace EntityFrameworkCore.Scaffolding.Handlebars.Internal
{
    [DebuggerStepThrough]
    internal static class SharedTypeExtensions
    {
        public static bool IsNullableType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return !typeInfo.IsValueType
                   || typeInfo.IsGenericType
                   && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
