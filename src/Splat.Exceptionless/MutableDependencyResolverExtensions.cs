// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;

namespace Splat.Exceptionless;

/// <summary>
/// Provides extension methods for registering Exceptionless integration with Splat using a wrapping full logger
/// pattern.
/// </summary>
public static class MutableDependencyResolverExtensions
{
    /// <summary>
    /// Initializes Exceptionless integration with Splat using the wrapping full logger pattern.
    /// </summary>
    /// <remarks>
    /// Configure Exceptionless client before calling this method.
    /// </remarks>
    /// <param name="instance">
    /// The mutable dependency resolver to register Exceptionless with.
    /// </param>
    /// <param name="exceptionlessClient">The configured Exceptionless client instance.</param>
    /// <example>
    /// <code>
    /// AppLocator.CurrentMutable.UseExceptionlessWithWrappingFullLogger(exception);
    /// </code>
    /// </example>
    public static void UseExceptionlessWithWrappingFullLogger(this IMutableDependencyResolver instance, ExceptionlessClient exceptionlessClient)
    {
        ArgumentExceptionHelper.ThrowIfNull(instance);

        var funcLogManager = new FuncLogManager(type =>
        {
            var miniLoggingWrapper = new ExceptionlessSplatLogger(type, exceptionlessClient);
            return new WrappingFullLogger(miniLoggingWrapper);
        });

        instance.RegisterConstant<ILogManager>(funcLogManager);
    }
}
