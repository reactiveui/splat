// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.NLog;

/// <summary>Provides methods for resolving NLog logger instances associated with specific types.</summary>
/// <remarks>This class uses an internal cache to efficiently retrieve logger instances for types. It is intended
/// for internal use and is not thread-safe for external callers.</remarks>
internal static class LogResolver
{
    /// <summary>Maximum number of resolved loggers retained by the cache.</summary>
    private const int MaxCacheSize = 16;

    /// <summary>Caches the NLog logger resolved for each type, bounded by <see cref="MaxCacheSize"/>.</summary>
    private static readonly MemoizingMRUCache<Type, global::NLog.Logger> _loggerCache = new(
        static (type, _) => global::NLog.LogManager.GetLogger(type.ToString()),
        MaxCacheSize);

    /// <summary>Resolves the NLog logger associated with the specified type, using an internal cache.</summary>
    /// <param name="type">The type for which to resolve a logger. The type's full name is used as the logger name.</param>
    /// <returns>The cached or newly created <see cref="global::NLog.Logger"/> for the specified type.</returns>
    internal static global::NLog.Logger Resolve(Type type) => _loggerCache.Get(type, null);
}
