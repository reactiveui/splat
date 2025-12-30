// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents a service registration that can be either an instance or a factory.
/// </summary>
/// <typeparam name="T">The service type.</typeparam>
internal readonly record struct Registration<T>
{
    private readonly T? _instance;
    private readonly Func<T?>? _factory;

    private Registration(T? instance, Func<T?>? factory, bool isFactory)
    {
        _instance = instance;
        _factory = factory;
        IsFactory = isFactory;
    }

    /// <summary>
    /// Gets a value indicating whether this registration is a factory.
    /// </summary>
    public bool IsFactory { get; }

    /// <summary>
    /// Creates a registration from an instance.
    /// </summary>
    /// <param name="instance">The instance to register.</param>
    /// <returns>A registration containing the instance.</returns>
    public static Registration<T> FromInstance(T instance) => new(instance, null, false);

    /// <summary>
    /// Creates a registration from a factory.
    /// </summary>
    /// <param name="factory">The factory to register.</param>
    /// <returns>A registration containing the factory.</returns>
    public static Registration<T> FromFactory(Func<T?> factory) => new(default, factory, true);

    /// <summary>
    /// Gets the instance from this registration.
    /// </summary>
    /// <returns>The instance value.</returns>
    public T GetInstance() => IsFactory ? default! : _instance!;

    /// <summary>
    /// Gets the factory from this registration.
    /// </summary>
    /// <returns>The factory function.</returns>
    public Func<T?> GetFactory() => IsFactory ? _factory! : default!;
}
