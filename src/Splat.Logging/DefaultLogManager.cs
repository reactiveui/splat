// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides the default implementation of the <see cref="ILogManager"/> interface for creating and managing loggers by
/// type.
/// </summary>
/// <remarks>This class retrieves loggers using a dependency resolver and caches them for efficient reuse. It is
/// typically used as the standard log manager in applications that require logging support. Thread safety is ensured
/// for logger retrieval operations.</remarks>
public sealed class DefaultLogManager : ILogManager
{
    private static readonly IFullLogger _nullLogger = new WrappingFullLogger(new NullLogger());
    private readonly MemoizingMRUCache<Type, IFullLogger> _loggerCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultLogManager"/> class.
    /// </summary>
    /// <param name="dependencyResolver">A dependency resolver for testing purposes, will use the default Locator if null.</param>
    public DefaultLogManager(IReadonlyDependencyResolver dependencyResolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(dependencyResolver);

        _loggerCache = new(
            (type, _) =>
            {
                var ret = dependencyResolver.GetService<ILogger>();
                return ret switch
                {
                    null => throw new LoggingException("Couldn't find an ILogger. This should never happen, your dependency resolver is probably broken."),
                    _ => new WrappingFullLogger(new WrappingPrefixLogger(ret, type))
                };
            },
            64);
    }

    /// <inheritdoc />
    public IFullLogger GetLogger(Type type)
    {
        if (type == typeof(MemoizingMRUCache<Type, IFullLogger>))
        {
            return _nullLogger;
        }

        lock (_loggerCache)
        {
            return _loggerCache.Get(type);
        }
    }
}
