// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Splat;

internal static class ReflectionStubs
{
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("Type.GetField() may be trimmed.")]
    [RequiresDynamicCode("Type.GetField() may be trimmed.")]
#endif
    public static FieldInfo? GetField(this [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] Type value, string name, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredField(name);
        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetField(name, flags);
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("Type.GetMethod() may be trimmed.")]
    [RequiresDynamicCode("Type.GetMethod() may be trimmed.")]
#endif
    public static MethodInfo? GetMethod(this [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type value, string name, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredMethod(name);
        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetMethod(name, flags);
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("Type.GetProperty() may be trimmed.")]
    [RequiresDynamicCode("Type.GetProperty() may be trimmed.")]
#endif
    public static PropertyInfo? GetProperty(this [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] Type value, string name, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredProperty(name);
        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetProperty(name, flags);
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("Type.GetEvent() may be trimmed.")]
    [RequiresDynamicCode("Type.GetEvent() may be trimmed.")]
#endif
    public static EventInfo? GetEvent(this [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)] Type value, string name, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredEvent(name);
        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetEvent(name, flags);
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("Type.GetProperties() may be trimmed.")]
    [RequiresDynamicCode("Type.GetProperties() may be trimmed.")]
#endif
    public static IEnumerable<PropertyInfo> GetProperties(this [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] Type value) => value.GetTypeInfo().DeclaredProperties;

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("Type.GetFields() may be trimmed.")]
    [RequiresDynamicCode("Type.GetFields() may be trimmed.")]
#endif
    public static IEnumerable<FieldInfo> GetFields(this [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] Type value) => value.GetTypeInfo().DeclaredFields;

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("Type.GetMethod() may be trimmed.")]
    [RequiresDynamicCode("Type.GetMethod() may be trimmed.")]
#endif
    public static MethodInfo? GetMethod(this [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type value, string methodName, Type[] paramTypes, BindingFlags flags = default)
    {
        var ti = value.GetTypeInfo();
        var ret = ti.GetDeclaredMethods(methodName)
            .FirstOrDefault(x => paramTypes.Zip(x.GetParameters().Select(y => y.ParameterType), (l, r) => l == r).All(y => y));

        return ret is not null || !flags.HasFlag(BindingFlags.FlattenHierarchy) || ti.BaseType is null
            ? ret
            : ti.BaseType.GetMethod(methodName, paramTypes, flags);
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("Type.GetMethods() may be trimmed.")]
    [RequiresDynamicCode("Type.GetMethods() may be trimmed.")]
#endif
    public static IEnumerable<MethodInfo> GetMethods(this [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type value) => value.GetTypeInfo().DeclaredMethods;

    public static IEnumerable<object> GetCustomAttributes(this Type value, Type attributeType, bool inherit) => value.GetTypeInfo().GetCustomAttributes(attributeType, inherit);

    public static bool IsAssignableFrom(this Type value, Type anotherType) => value.GetTypeInfo().IsAssignableFrom(anotherType.GetTypeInfo());
}
