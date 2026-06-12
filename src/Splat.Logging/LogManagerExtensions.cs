// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>Provides extension methods for obtaining loggers from an ILogManager instance.</summary>
[SuppressMessage(
    "Minor Code Smell",
    "S4018:All type parameters should be used in the parameter list to enable type inference",
    Justification = "The generic type parameter is the caller-supplied type used to resolve the logger; it cannot be a method parameter without changing the API contract.")]
public static class LogManagerExtensions
{
    /// <summary>Extension methods for the ILogManager.</summary>
    /// <param name="logManager">The log manager to get the logger from.</param>
    extension(ILogManager logManager)
    {
        /// <summary>Gets a <see cref="IFullLogger"/> for the specified <see cref="ILogManager"/>.</summary>
        /// <typeparam name="T">The type of <see cref="ILogManager"/> to use.</typeparam>
        /// <returns>A logger for the specified type.</returns>
        public IFullLogger GetLogger<T>()
        {
            ArgumentExceptionHelper.ThrowIfNull(logManager);

            return logManager.GetLogger(typeof(T));
        }
    }
}
