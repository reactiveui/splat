// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;

namespace Splat.Exceptionless;

/// <summary>
/// <summary>
/// Exceptionless specific extensions for the Mutable Dependency Resolver.
/// </summary>
/// </summary>
public static class MutableDependencyResolverExtensions
{
    /// <summary>
    /// Simple helper to initialize Exceptionless within Splat with the Wrapping Full Logger.
    /// </summary>
    /// <remarks>
    /// You should configure Exceptionless prior to calling this method.
    /// </remarks>
    /// <param name="instance">
    /// An instance of Mutable Dependency Resolver.
    /// </param>
    /// <param name="exceptionlessClient">The exceptionless client instance to use.</param>
    /// <example>
    /// <code>
    /// Locator.CurrentMutable.UseExceptionlessWithWrappingFullLogger(exception);
    /// </code>
    /// </example>
    public static void UseExceptionlessWithWrappingFullLogger(this IMutableDependencyResolver instance, ExceptionlessClient exceptionlessClient)
    {
        var funcLogManager = new FuncLogManager(type =>
        {
            var miniLoggingWrapper = new ExceptionlessSplatLogger(type, exceptionlessClient);
            return new WrappingFullLogger(miniLoggingWrapper);
        });

        instance.RegisterConstant<ILogManager>(funcLogManager);
    }
}
