// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !NET

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat.Internal;

/// <summary>
/// Polyfill for the <see cref="ArgumentOutOfRangeException"/> throw helpers on target frameworks (net462-net481)
/// that predate them. On net8.0 and later this type is not compiled; consuming projects alias the
/// <c>ArgumentOutOfRangeExceptionHelper</c> identifier directly to <see cref="ArgumentOutOfRangeException"/>.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ArgumentOutOfRangeExceptionHelper
{
    /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.</summary>
    /// <param name="value">The value to validate as non-negative.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    internal static void ThrowIfNegative(
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value >= 0)
        {
            return;
        }

        throw new ArgumentOutOfRangeException(paramName, value, "The value cannot be negative.");
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than or equal to <paramref name="other"/>.</summary>
    /// <typeparam name="T">The type of the values to compare.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    internal static void ThrowIfLessThanOrEqual<T>(
        T value,
        T other,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(other) > 0)
        {
            return;
        }

        throw new ArgumentOutOfRangeException(paramName, value, $"The value cannot be less than or equal to {other}.");
    }
}

#endif
