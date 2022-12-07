﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat.NLog;

/// <summary>
/// Resolves a type to a NLog instance.
/// </summary>
internal static class LogResolver
{
    private const int MaxCacheSize = 16;
    private static readonly MemoizingMRUCache<Type, global::NLog.ILogger> _loggerCache = new(
        (type, _) => global::NLog.LogManager.GetLogger(type.ToString()),
        MaxCacheSize);

    public static global::NLog.ILogger Resolve(Type type)
    {
        return _loggerCache.Get(type, null);
    }
}
