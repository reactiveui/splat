// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !NET

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat.Internal;

/// <summary>
/// Polyfill for the <see cref="ArgumentNullException"/>/<see cref="ArgumentException"/> throw helpers on target
/// frameworks (net462-net481) that predate them. On net8.0 and later this type is not compiled; consuming projects
/// alias the <c>ArgumentExceptionHelper</c> identifier directly to <see cref="ArgumentNullException"/> so the call
/// sites bind to the BCL methods (<c>ThrowIfNullOrWhiteSpace</c> resolving via the <see cref="ArgumentException"/> base).
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ArgumentExceptionHelper
{
    /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/>.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    public static void ThrowIfNull(
        [NotNull] object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is not null)
        {
            return;
        }

        throw new ArgumentNullException(paramName);
    }

    /// <summary>Throws if <paramref name="argument"/> is <see langword="null"/>, empty, or all white-space.</summary>
    /// <param name="argument">The string argument to validate.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    public static void ThrowIfNullOrWhiteSpace(
        [NotNull] string? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }

        if (!string.IsNullOrWhiteSpace(argument))
        {
            return;
        }

        throw new ArgumentException("The value cannot be an empty string or composed entirely of whitespace.", paramName);
    }
}

#endif
