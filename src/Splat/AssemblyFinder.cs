// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reflection;

namespace Splat;

internal static class AssemblyFinder
{
    /// <summary>
    /// Attempt to find the type based on the specified string.
    /// </summary>
    /// <typeparam name="T">The type to cast the value to if we find it.</typeparam>
    /// <param name="fullTypeName">The name of the full type.</param>
    /// <returns>The created object or the default value.</returns>
    public static T? AttemptToLoadType<T>(string fullTypeName)
    {
        var thisType = typeof(AssemblyFinder);

        var thisTypeName = thisType.AssemblyQualifiedName;

        if (thisTypeName is null)
        {
            return default;
        }

        var toSearch = new[]
        {
#if NET6_0_OR_GREATER
            thisTypeName.Replace(thisType.FullName + ", ", string.Empty, StringComparison.CurrentCulture),
            thisTypeName.Replace(thisType.FullName + ", ", string.Empty, StringComparison.CurrentCulture).Replace(".Portable", string.Empty, StringComparison.CurrentCulture),
#else
            thisTypeName.Replace(thisType.FullName + ", ", string.Empty),
            thisTypeName.Replace(thisType.FullName + ", ", string.Empty).Replace(".Portable", string.Empty),
#endif
        }.Select(x => new AssemblyName(x)).ToArray();

        foreach (var assembly in toSearch)
        {
            var fullName = fullTypeName + ", " + assembly.FullName;
            var type = Type.GetType(fullName, false);
            if (type is null)
            {
                continue;
            }

            return (T?)Activator.CreateInstance(type);
        }

        return default;
    }
}
