// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.NLog;

/// <summary>
/// Provides methods for resolving NLog logger instances associated with specific types.
/// </summary>
/// <remarks>This class uses an internal cache to efficiently retrieve logger instances for types. It is intended
/// for internal use and is not thread-safe for external callers.</remarks>
internal static class LogResolver
{
    private const int MaxCacheSize = 16;
    private static readonly MemoizingMRUCache<Type, global::NLog.Logger> _loggerCache = new(
        (type, _) => global::NLog.LogManager.GetLogger(type.ToString()),
        MaxCacheSize);

    public static global::NLog.Logger Resolve(Type type) => _loggerCache.Get(type, null);
}
