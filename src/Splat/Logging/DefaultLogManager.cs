// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// The default log manager provided by splat.
/// This log manager will cache the loggers for each type,
/// This will use the default registered <see cref="ILogger"/> inside the <see cref="Locator"/>.
/// </summary>
public sealed class DefaultLogManager : ILogManager
{
    private static readonly IFullLogger _nullLogger = new WrappingFullLogger(new NullLogger());
    private readonly MemoizingMRUCache<Type, IFullLogger> _loggerCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultLogManager"/> class.
    /// </summary>
    /// <param name="dependencyResolver">A dependency resolver for testing purposes, will use the default Locator if null.</param>
    public DefaultLogManager(IReadonlyDependencyResolver? dependencyResolver = null)
    {
        dependencyResolver ??= Locator.Current;

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
