// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Log4Net;

/// <summary>
/// Log4net specific extensions for the Mutable Dependency Resolver.
/// </summary>
public static class MutableDependencyResolverExtensions
{
    /// <summary>
    /// Simple helper to initialize Log4Net within Splat with the Wrapping Full Logger.
    /// </summary>
    /// <remarks>
    /// You should configure Log4Net prior to calling this method.
    /// </remarks>
    /// <param name="instance">
    /// An instance of Mutable Dependency Resolver.
    /// </param>
    /// <example>
    /// <code>
    /// Locator.CurrentMutable.UseLog4NetWithWrappingFullLogger();
    /// </code>
    /// </example>
    public static void UseLog4NetWithWrappingFullLogger(this IMutableDependencyResolver instance)
    {
        var funcLogManager = new FuncLogManager(type => new WrappingFullLogger(new Log4NetLogger(LogResolver.Resolve(type))));

        instance.RegisterConstant(funcLogManager, typeof(ILogManager));
    }
}
