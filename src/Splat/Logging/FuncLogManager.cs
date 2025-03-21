﻿// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A log manager which will generate the <see cref="IFullLogger"/> by using the specified Func.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FuncLogManager"/> class.
/// </remarks>
/// <param name="getLoggerFunc">The function which will be used to generate the <see cref="IFullLogger"/>.</param>
public class FuncLogManager(Func<Type, IFullLogger> getLoggerFunc) : ILogManager
{
    private readonly Func<Type, IFullLogger> _inner = getLoggerFunc;

    /// <inheritdoc />
    public IFullLogger GetLogger(Type type) => _inner(type);
}
