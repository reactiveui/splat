// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat;

/// <summary>
/// A log manager which will generate the <see cref="IFullLogger"/> by using the specified Func.
/// </summary>
public class FuncLogManager : ILogManager
{
    private readonly Func<Type, IFullLogger> _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncLogManager"/> class.
    /// </summary>
    /// <param name="getLoggerFunc">The function which will be used to generate the <see cref="IFullLogger"/>.</param>
    public FuncLogManager(Func<Type, IFullLogger> getLoggerFunc)
    {
        _inner = getLoggerFunc;
    }

    /// <inheritdoc />
    public IFullLogger GetLogger(Type type)
    {
        return _inner(type);
    }
}
