// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !NET

using System.Diagnostics.CodeAnalysis;

namespace Splat.Internal;

/// <summary>
/// Polyfill for the <see cref="ObjectDisposedException"/> throw helper on target frameworks (net462-net481)
/// that predate it. On net8.0 and later this type is not compiled; consuming projects alias the
/// <c>ObjectDisposedExceptionHelper</c> identifier directly to <see cref="ObjectDisposedException"/>.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ObjectDisposedExceptionHelper
{
    /// <summary>Throws an <see cref="ObjectDisposedException"/> if <paramref name="condition"/> is <see langword="true"/>.</summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="instance">The object whose type name is included in the exception message.</param>
    internal static void ThrowIf([DoesNotReturnIf(true)] bool condition, object instance)
    {
        if (!condition)
        {
            return;
        }

        throw new ObjectDisposedException(instance?.GetType().FullName);
    }

    /// <summary>Throws an <see cref="ObjectDisposedException"/> if <paramref name="condition"/> is <see langword="true"/>.</summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="type">The type whose name is included in the exception message.</param>
    internal static void ThrowIf([DoesNotReturnIf(true)] bool condition, Type type)
    {
        if (!condition)
        {
            return;
        }

        throw new ObjectDisposedException(type?.FullName);
    }
}

#endif
