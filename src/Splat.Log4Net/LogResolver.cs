// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Log4Net;

/// <summary>
/// Resolves a logger to the specified type.
/// </summary>
internal static class LogResolver
{
    private const int MaxCacheSize = 16;
    private static readonly MemoizingMRUCache<Type, global::log4net.ILog> _loggerCache = new(
        (type, _) => global::log4net.LogManager.GetLogger(type),
        MaxCacheSize);

    public static global::log4net.ILog Resolve(Type type) => _loggerCache.Get(type, null);
}
