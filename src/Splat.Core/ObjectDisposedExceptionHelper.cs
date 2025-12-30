// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Provides helper methods for disposal validation.
/// These methods serve as polyfills for ObjectDisposedException.ThrowIf methods
/// that are only available in .NET 8 and later.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ObjectDisposedExceptionHelper
{
#if NET8_0_OR_GREATER
    /// <summary>
    /// Throws an <see cref="ObjectDisposedException"/> if the specified condition is true.
    /// Uses the built-in .NET 8+ method.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="instance">The object whose type's full name should be included in the exception message.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIf([DoesNotReturnIf(true)] bool condition, object instance) => ObjectDisposedException.ThrowIf(condition, instance);

    /// <summary>
    /// Throws an <see cref="ObjectDisposedException"/> if the specified condition is true.
    /// Uses the built-in .NET 8+ method.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="type">The type whose full name should be included in the exception message.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIf([DoesNotReturnIf(true)] bool condition, Type type) => ObjectDisposedException.ThrowIf(condition, type);
#else
    /// <summary>
    /// Throws an <see cref="ObjectDisposedException"/> if the specified condition is true.
    /// Polyfill for .NET 8+ ObjectDisposedException.ThrowIf method.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="instance">The object whose type's full name should be included in the exception message.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIf([DoesNotReturnIf(true)] bool condition, object instance)
    {
        if (condition)
        {
            throw new ObjectDisposedException(instance?.GetType().FullName);
        }
    }

    /// <summary>
    /// Throws an <see cref="ObjectDisposedException"/> if the specified condition is true.
    /// Polyfill for .NET 8+ ObjectDisposedException.ThrowIf method.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="type">The type whose full name should be included in the exception message.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIf([DoesNotReturnIf(true)] bool condition, Type type)
    {
        if (condition)
        {
            throw new ObjectDisposedException(type?.FullName);
        }
    }
#endif
}
