// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides an implementation of <see cref="ILogManager"/> that retrieves loggers using a supplied delegate.
/// </summary>
/// <remarks>Use this class to integrate custom logger retrieval logic, such as dependency injection or factory
/// patterns, by supplying an appropriate delegate.</remarks>
/// <param name="getLoggerFunc">A function that returns an <see cref="IFullLogger"/> instance for a given <see cref="Type"/>. Cannot be null.</param>
public class FuncLogManager(Func<Type, IFullLogger> getLoggerFunc) : ILogManager
{
    private readonly Func<Type, IFullLogger> _inner = getLoggerFunc;

    /// <inheritdoc />
    public IFullLogger GetLogger(Type type) => _inner(type);
}
