// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

internal static class ExceptionMixins
{
    /// <summary>
    /// Throws the argument null exception if null.
    /// </summary>
    /// <typeparam name="T">Source Type.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name.</param>
    public static void ThrowArgumentNullExceptionIfNull<T>(this T? value, string name)
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }
    }

    /// <summary>
    /// Throws the argument null exception if null.
    /// </summary>
    /// <typeparam name="T">Source Type.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name.</param>
    /// <param name="message">The message.</param>
    public static void ThrowArgumentNullExceptionIfNull<T>(this T? value, string name, string message)
    {
        if (value is null)
        {
            throw new ArgumentNullException(name, message);
        }
    }
}
