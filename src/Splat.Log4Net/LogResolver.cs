// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Log4Net;

/// <summary>Provides methods for resolving log4net logger instances for specified types.</summary>
/// <remarks>This class uses an internal cache to improve performance when retrieving logger instances. It is
/// intended for internal use and is not thread-safe for external modification.</remarks>
internal static class LogResolver
{
    /// <summary>Maximum number of resolved loggers retained by the cache.</summary>
    private const int MaxCacheSize = 16;

    /// <summary>Caches the log4net logger resolved for each type, bounded by <see cref="MaxCacheSize"/>.</summary>
    private static readonly MemoizingMRUCache<Type, log4net.ILog> _loggerCache = new(
        (type, _) => log4net.LogManager.GetLogger(type),
        MaxCacheSize);

    /// <summary>Resolves the log4net logger associated with the specified type, using an internal cache.</summary>
    /// <param name="type">The type for which to resolve the logger.</param>
    /// <returns>The cached <see cref="log4net.ILog"/> instance for the specified type.</returns>
    public static log4net.ILog Resolve(Type type) => _loggerCache.Get(type, null);
}
