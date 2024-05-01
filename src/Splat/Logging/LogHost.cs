// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// Contains helper methods to get access to the Default <see cref="IFullLogger"/>.
/// </summary>
public static class LogHost
{
    /// <summary>
    /// Gets the default <see cref="IFullLogger"/> registered within the <see cref="Locator"/>.
    /// </summary>
    [SuppressMessage("Design", "CA1065: Do not raise exceptions in properties", Justification = "Very rare scenario")]
    public static IStaticFullLogger Default
    {
        get
        {
            /*
             * DV - this will need an extension of the dependency resolver.
             * RegisterLazy(Func<IReadOnlyDependencyResolver, object> factory);
             * which will allow a lazy instance of the static logger.
             *
             * return Locator.Current.GetService<IStaticFullLogger>();
             */

            var factory = Locator.Current.GetService<ILogManager>() ?? throw new LoggingException("ILogManager is null. This should never happen, your dependency resolver is broken");
            var fullLogger = factory.GetLogger(typeof(LogHost));

            return new StaticFullLogger(fullLogger);
        }
    }

    /// <summary>
    /// Call this method to write log entries on behalf of the current class.
    /// </summary>
    /// <typeparam name="T">The type to get the <see cref="IFullLogger"/> for.</typeparam>
    /// <param name="logClassInstance">The class we are getting the logger for.</param>
    /// <returns>The <see cref="IFullLogger"/> for the class type.</returns>
#pragma warning disable RCS1175 // Unused 'this' parameter.
    public static IFullLogger Log<T>(this T logClassInstance)
#pragma warning restore RCS1175 // Unused 'this' parameter.
        where T : IEnableLogger
    {
        var factory = Locator.Current.GetService<ILogManager>();
        return factory switch
        {
            null => throw new InvalidOperationException("ILogManager is null. This should never happen, your dependency resolver is broken"),
            _ => factory.GetLogger<T>()
        };
    }
}
