// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Splat;

/// <summary>
/// Provides methods for locating and instantiating types from assemblies at runtime using reflection.
/// </summary>
/// <remarks>This class is intended for internal use and supports dynamic type loading scenarios where types may
/// not be statically referenced. Methods in this class may not be compatible with ahead-of-time (AOT) compilation
/// environments due to their reliance on reflection.</remarks>
internal static class AssemblyFinder
{
    /// <summary>
    /// Attempts to load a type by its fully qualified name and create an instance of it, returning the instance if
    /// successful.
    /// </summary>
    /// <remarks>This method uses reflection to dynamically load types from the current assembly or its
    /// portable variant. It is not compatible with ahead-of-time (AOT) compilation and may not work in environments
    /// where reflection is restricted. The method returns null if the type cannot be found or instantiated.</remarks>
    /// <typeparam name="T">The type to instantiate. Must have a public parameterless constructor.</typeparam>
    /// <param name="fullTypeName">The fully qualified name of the type to load, including its namespace.</param>
    /// <returns>An instance of type T if the type is found and instantiated successfully; otherwise, null.</returns>
    [RequiresUnreferencedCode("This method uses reflection to dynamically load types and cannot be made AOT-compatible.")]
    public static T? AttemptToLoadType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string fullTypeName)
    {
        var thisType = typeof(AssemblyFinder);

        var thisTypeName = thisType.AssemblyQualifiedName;

        if (thisTypeName is null)
        {
            return default;
        }

        AssemblyName[] toSearch =
        [
#if NET6_0_OR_GREATER
            new(thisTypeName.Replace(thisType.FullName + ", ", string.Empty, StringComparison.CurrentCulture)),
            new(thisTypeName.Replace(thisType.FullName + ", ", string.Empty, StringComparison.CurrentCulture).Replace(".Portable", string.Empty, StringComparison.CurrentCulture)),
#else
            new(thisTypeName.Replace(thisType.FullName + ", ", string.Empty)),
            new(thisTypeName.Replace(thisType.FullName + ", ", string.Empty).Replace(".Portable", string.Empty)),
#endif
        ];

        foreach (var assembly in toSearch)
        {
            string fullName = fullTypeName + ", " + assembly.FullName;

            var type = Type.GetType(fullName, false);
            if (type is null)
            {
                continue;
            }

            GC.KeepAlive(type);
            return (T?)Activator.CreateInstance(type);
        }

        return default;
    }
}
