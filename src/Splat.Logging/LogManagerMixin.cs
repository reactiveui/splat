// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Splat;

/// <summary>
/// Provides extension methods for obtaining loggers from an ILogManager instance.
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
        ArgumentExceptionHelper.ThrowIfNull(logManager);

        return logManager.GetLogger(typeof(T));
    }
}
