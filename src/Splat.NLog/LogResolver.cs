﻿// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.NLog;

/// <summary>
/// Resolves a type to a NLog instance.
/// </summary>
internal static class LogResolver
{
    private const int MaxCacheSize = 16;
    private static readonly MemoizingMRUCache<Type, global::NLog.Logger> _loggerCache = new(
        (type, _) => global::NLog.LogManager.GetLogger(type.ToString()),
        MaxCacheSize);

    public static global::NLog.Logger Resolve(Type type) => _loggerCache.Get(type, null);
}
