// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Splat.Microsoft.Extensions.Logging;

/// <summary>
/// Microsoft.Extensions.Logging specific extensions for the Mutable Dependency Resolver.
/// </summary>
public static class MicrosoftExtensionsLoggingExtensions
{
    /// <summary>
    /// Simple helper to initialize Microsoft.Extensions.Logging within Splat with the Wrapping Full Logger.
    /// </summary>
    /// <remarks>
    /// You should configure Microsoft.Extensions.Logging prior to calling this method.
    /// </remarks>
    /// <param name="instance">
    /// An instance of Mutable Dependency Resolver.
    /// </param>
    /// <param name="loggerFactory">
    /// An instance of the Microsoft.Extensions.Logging Logger Factory.
    /// </param>
    /// <example>
    /// <code>
    /// Locator.CurrentMutable.UseMicrosoftExtensionsLoggingWithWrappingFullLogger();
    /// </code>
    /// </example>
    public static void UseMicrosoftExtensionsLoggingWithWrappingFullLogger(
        this IMutableDependencyResolver instance,
        ILoggerFactory loggerFactory)
    {
        var funcLogManager = new FuncLogManager(type =>
        {
            var actualLogger = loggerFactory.CreateLogger(type.ToString());
            var miniLoggingWrapper = new MicrosoftExtensionsLoggingLogger(actualLogger);
            return new WrappingFullLogger(miniLoggingWrapper);
        });

        instance.RegisterConstant(funcLogManager, typeof(ILogManager));
    }

    /// <summary>
    /// Registers a <see cref="MicrosoftExtensionsLogProvider"/> with the service collection.
    /// </summary>
    /// <param name="builder">The logging builder to register.</param>
    /// <returns>The logging builder.</returns>
    public static ILoggingBuilder AddSplat(this ILoggingBuilder builder)
    {
#if NETSTANDARD || NETFRAMEWORK
        if (builder is null)
        {
            throw new System.ArgumentNullException(nameof(builder));
        }
#else
        ArgumentNullException.ThrowIfNull(builder);
#endif

        builder.Services.AddSingleton<ILoggerProvider, MicrosoftExtensionsLogProvider>();

        return builder;
    }

    /// <summary>
    /// Adds a <see cref="MicrosoftExtensionsLogProvider"/> to the logger factory.
    /// </summary>
    /// <param name="loggerFactory">Our logger provider.</param>
    /// <returns>The factory.</returns>
    public static ILoggerFactory AddSplat(this ILoggerFactory loggerFactory)
    {
#if NETSTANDARD || NETFRAMEWORK
        if (loggerFactory is null)
        {
            throw new System.ArgumentNullException(nameof(loggerFactory));
        }
#else
        ArgumentNullException.ThrowIfNull(loggerFactory);
#endif

#pragma warning disable CA2000 // Dispose objects before losing scope
        loggerFactory.AddProvider(new MicrosoftExtensionsLogProvider());
#pragma warning restore CA2000 // Dispose objects before losing scope
        return loggerFactory;
    }
}
