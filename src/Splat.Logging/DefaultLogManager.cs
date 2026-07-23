// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>Provides the default implementation of the <see cref="ILogManager"/> interface for creating and managing loggers by type.</summary>
/// <remarks>This class retrieves loggers using a dependency resolver and caches them for efficient reuse. It is
/// typically used as the standard log manager in applications that require logging support. Thread safety is ensured
/// for logger retrieval operations.</remarks>
public sealed class DefaultLogManager : ILogManager
{
    /// <summary>Maximum number of loggers retained by the most-recently-used cache.</summary>
    private const int LoggerCacheSize = 64;

    /// <summary>Logger returned when registration produces a <see langword="null"/> logger.</summary>
    private static readonly IFullLogger _nullLogger = new WrappingFullLogger(new NullLogger());

    /// <summary>Caches the logger created for each type, bounded by <see cref="LoggerCacheSize"/>.</summary>
    private readonly MemoizingMRUCache<Type, IFullLogger> _loggerCache;

    /// <summary>Initializes a new instance of the <see cref="DefaultLogManager"/> class.</summary>
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
            LoggerCacheSize);
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
