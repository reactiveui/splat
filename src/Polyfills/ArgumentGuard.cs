// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat.Internal;

/// <summary>
/// Argument guards that have no single BCL throw-helper equivalent (a conditional <see cref="ArgumentException"/>
/// and a null check carrying a custom message). Unlike <c>ArgumentExceptionHelper</c>/<c>ArgumentOutOfRangeExceptionHelper</c>
/// these are not aliased to the BCL; they are compiled into each assembly on every target framework.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ArgumentGuard
{
    /// <summary>Throws an <see cref="ArgumentException"/> when <paramref name="condition"/> is <see langword="true"/>.</summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <param name="paramName">The name of the offending parameter.</param>
    public static void ThrowIf(
        [DoesNotReturnIf(true)] bool condition,
        string message,
        [CallerArgumentExpression(nameof(condition))] string? paramName = null)
    {
        if (!condition)
        {
            return;
        }

        throw new ArgumentException(message, paramName);
    }

    /// <summary>Throws an <see cref="ArgumentNullException"/> with <paramref name="message"/> when <paramref name="argument"/> is <see langword="null"/>.</summary>
    /// <param name="argument">The argument to validate as non-null.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    public static void ThrowIfNullWithMessage(
        [NotNull] object? argument,
        string message,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is not null)
        {
            return;
        }

        throw new ArgumentNullException(paramName, message);
    }
}
