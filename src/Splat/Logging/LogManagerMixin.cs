// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Extension methods associated with the logging module.
/// </summary>
public static class LogManagerMixin
{
    /// <summary>
    /// Gets a <see cref="IFullLogger"/> for the specified <see cref="ILogManager"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="ILogManager"/> to use.</typeparam>
    /// <param name="logManager">The log manager to get the logger from.</param>
    /// <returns>A logger for the specified type.</returns>
    public static IFullLogger GetLogger<T>(this ILogManager logManager)
    {
        logManager.ThrowArgumentNullExceptionIfNull(nameof(logManager));

        return logManager.GetLogger(typeof(T));
    }
}
