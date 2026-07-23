// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// Provides access to logging facilities and helpers for obtaining loggers associated with specific types or the
/// application as a whole.
/// </summary>
/// <remarks>The LogHost class serves as a central entry point for acquiring loggers in applications that use
/// dependency injection and logging abstractions. It enables consistent logger retrieval for classes implementing
/// IEnableLogger and provides a default static logger for general use. All members are static and
/// thread-safe.</remarks>
public static class LogHost
{
    /// <summary>Gets the default <see cref="IFullLogger"/> registered within the <see cref="IReadonlyDependencyResolver"/>.</summary>
    [SuppressMessage("Design", "CA1065: Do not raise exceptions in properties", Justification = "Very rare scenario")]
    public static IStaticFullLogger Default
    {
        get
        {
            var factory = AppLocator.Current.GetService<ILogManager>() ?? throw new LoggingException("ILogManager is null. This should never happen, your dependency resolver is broken");
            var fullLogger = factory.GetLogger(typeof(LogHost));

            return new StaticFullLogger(fullLogger);
        }
    }
}
