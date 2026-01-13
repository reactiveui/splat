// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Provides a dependency injection module that registers a default debug logger for use with Splat if no logger is
/// already registered.
/// </summary>
/// <remarks>This module is intended for use with dependency injection frameworks that support Splat's logging
/// infrastructure. It ensures that an implementation of ILogger is available by registering a DebugLogger if one has
/// not already been registered. This is typically used to enable logging in applications that use Splat without
/// requiring explicit logger configuration.</remarks>
public sealed class SplatLoggerModule() : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        if (!resolver.HasRegistration<ILogger>())
        {
            var debugLogger = new DebugLogger();
            resolver.Register<ILogger>(() => debugLogger);
        }
    }
}
