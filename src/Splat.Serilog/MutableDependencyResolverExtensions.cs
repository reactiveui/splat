// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Serilog;

/// <summary>
/// Serilog specific extensions for the Mutable Dependency Resolver.
/// </summary>
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
    /// Locator.CurrentMutable.UseSerilogWithWrappingFullLogger();
    /// </code>
    /// </example>
    public static void UseSerilogFullLogger(this IMutableDependencyResolver instance)
    {
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
    /// Locator.CurrentMutable.UseSerilogWithWrappingFullLogger();
    /// </code>
    /// </example>
    public static void UseSerilogFullLogger(this IMutableDependencyResolver instance, global::Serilog.ILogger actualLogger)
    {
        var funcLogManager = new FuncLogManager(type => new SerilogFullLogger(actualLogger.ForContext(type)));

        instance.Register<ILogManager>(() => funcLogManager);
    }
}
