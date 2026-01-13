// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.NLog;

/// <summary>
/// Provides extension methods for registering NLog integration with Splat using a mutable dependency resolver.
/// </summary>
public static class MutableDependencyResolverExtensions
{
    /// <summary>
    /// Initializes NLog integration with Splat using the wrapping full logger pattern.
    /// </summary>
    /// <remarks>
    /// Configure NLog targets and rules before calling this method.
    /// </remarks>
    /// <param name="instance">
    /// The mutable dependency resolver to register NLog with.
    /// </param>
    /// <example>
    /// <code>
    /// AppLocator.CurrentMutable.UseNLogWithWrappingFullLogger();
    /// </code>
    /// </example>
    public static void UseNLogWithWrappingFullLogger(this IMutableDependencyResolver instance)
    {
        ArgumentExceptionHelper.ThrowIfNull(instance);

        var funcLogManager = new FuncLogManager(type => new NLogLogger(LogResolver.Resolve(type)));

        instance.Register<ILogManager>(() => funcLogManager);
    }
}
