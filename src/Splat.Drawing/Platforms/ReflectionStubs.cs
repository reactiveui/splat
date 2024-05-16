// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reflection;

namespace Splat;

internal static class ReflectionStubs
{
    public static FieldInfo? GetField(this Type value, string name, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredField(name);
        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetField(name, flags);
    }

    public static MethodInfo? GetMethod(this Type value, string name, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredMethod(name);
        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetMethod(name, flags);
    }

    public static PropertyInfo? GetProperty(this Type value, string name, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredProperty(name);
        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetProperty(name, flags);
    }

    public static EventInfo? GetEvent(this Type value, string name, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredEvent(name);
        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetEvent(name, flags);
    }

    public static IEnumerable<PropertyInfo> GetProperties(this Type value) => value.GetTypeInfo().DeclaredProperties;

    public static IEnumerable<FieldInfo> GetFields(this Type value) => value.GetTypeInfo().DeclaredFields;

    public static MethodInfo? GetMethod(this Type value, string methodName, Type[] paramTypes, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredMethods(methodName)
            .FirstOrDefault(x => paramTypes.Zip(x.GetParameters().Select(y => y.ParameterType), (l, r) => l == r).All(y => y));

        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetMethod(methodName, paramTypes, flags);
    }

    public static IEnumerable<MethodInfo> GetMethods(this Type value) => value.GetTypeInfo().DeclaredMethods;

    public static IEnumerable<object> GetCustomAttributes(this Type value, Type attributeType, bool inherit) => value.GetTypeInfo().GetCustomAttributes(attributeType, inherit);

    public static bool IsAssignableFrom(this Type value, Type anotherType) => value.GetTypeInfo().IsAssignableFrom(anotherType.GetTypeInfo());
}
