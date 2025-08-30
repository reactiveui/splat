// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Log4Net;

/// <summary>
/// Resolves a logger to the specified type.
/// </summary>
internal static class LogResolver
{
    private const int MaxCacheSize = 16;
    private static readonly MemoizingMRUCache<Type, log4net.ILog> _loggerCache = new(
        (type, _) => log4net.LogManager.GetLogger(type),
        MaxCacheSize);

    public static log4net.ILog Resolve(Type type) => _loggerCache.Get(type, null);
}
