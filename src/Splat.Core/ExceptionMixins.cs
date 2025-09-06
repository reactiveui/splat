// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

internal static class ExceptionMixins
{
    /// <summary>
    /// Throws an ArgumentNullException if the value is null.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="name">The parameter name to include in the exception.</param>
    public static void ThrowArgumentNullExceptionIfNull<T>(this T? value, string name)
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }
    }

    /// <summary>
    /// Throws an ArgumentNullException if the value is null.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="name">The parameter name to include in the exception.</param>
    /// <param name="message">The message to include in the exception.</param>
    public static void ThrowArgumentNullExceptionIfNull<T>(this T? value, string name, string message)
    {
        if (value is null)
        {
            throw new ArgumentNullException(name, message);
        }
    }
}
