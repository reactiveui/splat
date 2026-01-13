// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Serilog;

/// <summary>
/// Provides extension methods for registering Serilog as the logging implementation within Splat using the Wrapping
/// Full Logger.
/// </summary>
/// <remarks>These extension methods allow you to configure Splat to use Serilog for logging by registering the
/// appropriate logger implementation with an IMutableDependencyResolver. Serilog should be configured before invoking
/// these methods. These helpers are intended to simplify integration of Serilog with Splat's logging
/// infrastructure.</remarks>
public static class MutableDependencyResolverExtensions
{
    /// <summary>
    /// Simple helper to initialize Serilog within Splat with the Wrapping Full Logger.
    /// </summary>
    /// <remarks>
    /// You should configure Serilog prior to calling this method.
    /// </remarks>
    /// <param name="instance">An instance of Mutable Dependency Resolver.</param>
    /// <example>
    /// <code>
    /// AppLocator.CurrentMutable.UseSerilogWithWrappingFullLogger();
    /// </code>
    /// </example>
    public static void UseSerilogFullLogger(this IMutableDependencyResolver instance)
    {
        ArgumentExceptionHelper.ThrowIfNull(instance);

        var funcLogManager = new FuncLogManager(type =>
        {
            var actualLogger = global::Serilog.Log.ForContext(type);
            return new SerilogFullLogger(actualLogger);
        });

        instance.Register<ILogManager>(() => funcLogManager);
    }

    /// <summary>
    /// Simple helper to initialize Serilog within Splat with the Wrapping Full Logger.
    /// </summary>
    /// <remarks>
    /// You should configure Serilog prior to calling this method.
    /// </remarks>
    /// <param name="instance">An instance of Mutable Dependency Resolver.</param>
    /// <param name="actualLogger">The serilog logger to register.</param>
    /// <example>
    /// <code>
    /// AppLocator.CurrentMutable.UseSerilogWithWrappingFullLogger();
    /// </code>
    /// </example>
    public static void UseSerilogFullLogger(this IMutableDependencyResolver instance, global::Serilog.ILogger actualLogger)
    {
        ArgumentExceptionHelper.ThrowIfNull(instance);

        var funcLogManager = new FuncLogManager(type => new SerilogFullLogger(actualLogger.ForContext(type)));

        instance.Register<ILogManager>(() => funcLogManager);
    }
}
