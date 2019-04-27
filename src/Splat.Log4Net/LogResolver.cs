// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat.Log4Net
{
    /// <summary>
    /// Resolves a logger to the specified type.
    /// </summary>
    internal static class LogResolver
    {
        private const int MaxCacheSize = 16;
        private static readonly MemoizingMRUCache<Type, global::log4net.ILog> _loggerCache = new MemoizingMRUCache<Type, global::log4net.ILog>(
            (type, _) => global::log4net.LogManager.GetLogger(type),
            MaxCacheSize);

        public static global::log4net.ILog Resolve(Type type)
        {
            return _loggerCache.Get(type, null);
        }
    }
}
