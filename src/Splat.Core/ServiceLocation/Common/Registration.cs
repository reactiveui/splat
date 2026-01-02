// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// Represents a service registration that is either a constant instance or a factory delegate.
/// </summary>
/// <remarks>
/// <para>
/// This type is intentionally small and allocation-free:
/// it is used in hot paths of the generic-first resolver containers.
/// </para>
/// <para>
/// A registration is in exactly one of two modes:
/// <list type="bullet">
///   <item><description><b>Instance mode</b>: stores a pre-created instance.</description></item>
///   <item><description><b>Factory mode</b>: stores a delegate that produces instances on demand.</description></item>
/// </list>
/// </para>
/// <para>
/// For maximum performance, <see cref="GetInstance"/> and <see cref="GetFactory"/> do not throw if called in the wrong mode.
/// Callers should branch on <see cref="IsFactory"/> first, or use <see cref="TryGetInstance"/> / <see cref="TryGetFactory"/>.
/// </para>
/// </remarks>
/// <typeparam name="T">The registered service type.</typeparam>
internal readonly record struct Registration<T>
{
    /// <summary>
    /// Stored instance for instance-mode registrations; otherwise <see langword="null"/>.
    /// </summary>
    private readonly T? _instance;

    /// <summary>
    /// Stored delegate for factory-mode registrations; otherwise <see langword="null"/>.
    /// </summary>
    private readonly Func<T?>? _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="Registration{T}"/> struct.
    /// </summary>
    /// <param name="instance">Instance for instance-mode registrations; otherwise <see langword="null"/>.</param>
    /// <param name="factory">Factory for factory-mode registrations; otherwise <see langword="null"/>.</param>
    /// <param name="isFactory"><see langword="true"/> for factory-mode; <see langword="false"/> for instance-mode.</param>
    private Registration(T? instance, Func<T?>? factory, bool isFactory)
    {
        _instance = instance;
        _factory = factory;
        IsFactory = isFactory;
    }

    /// <summary>
    /// Gets a value indicating whether this registration stores a factory delegate.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if this registration is a factory; <see langword="false"/> if it is a constant instance.
    /// </value>
    public bool IsFactory { get; }

    /// <summary>
    /// Creates a registration that stores a constant instance.
    /// </summary>
    /// <param name="instance">The instance to register. May be <see langword="null"/>.</param>
    /// <returns>A registration in instance mode.</returns>
    public static Registration<T> FromInstance(T? instance) => new(instance, null, isFactory: false);

    /// <summary>
    /// Creates a registration that stores a factory delegate.
    /// </summary>
    /// <param name="factory">The factory delegate to register.</param>
    /// <returns>A registration in factory mode.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is <see langword="null"/>.</exception>
    public static Registration<T> FromFactory(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        return new(default, factory, isFactory: true);
    }

    /// <summary>
    /// Gets the stored instance.
    /// </summary>
    /// <returns>The stored instance.</returns>
    /// <remarks>
    /// This method is intended to be used only when <see cref="IsFactory"/> is <see langword="false"/>.
    /// For performance reasons, this method does not throw if the registration is in factory mode; it returns <see langword="null"/>.
    /// </remarks>
    public T? GetInstance() => IsFactory ? default : _instance;

    /// <summary>
    /// Gets the stored factory delegate.
    /// </summary>
    /// <returns>The stored factory delegate.</returns>
    /// <remarks>
    /// This method is intended to be used only when <see cref="IsFactory"/> is <see langword="true"/>.
    /// For performance reasons, this method does not throw if the registration is in instance mode; it returns <see langword="null"/>.
    /// </remarks>
    public Func<T?>? GetFactory() => IsFactory ? _factory! : null;

    /// <summary>
    /// Attempts to retrieve the stored instance when in instance mode.
    /// </summary>
    /// <param name="instance">Receives the instance when available.</param>
    /// <returns><see langword="true"/> if this registration is in instance mode; otherwise <see langword="false"/>.</returns>
    public bool TryGetInstance([MaybeNullWhen(false)] out T instance)
    {
        if (!IsFactory)
        {
            instance = _instance!;
            return true;
        }

        instance = default;
        return false;
    }

    /// <summary>
    /// Attempts to retrieve the stored factory when in factory mode.
    /// </summary>
    /// <param name="factory">Receives the factory delegate when available.</param>
    /// <returns><see langword="true"/> if this registration is in factory mode; otherwise <see langword="false"/>.</returns>
    public bool TryGetFactory([MaybeNullWhen(false)] out Func<T?> factory)
    {
        if (IsFactory)
        {
            factory = _factory!;
            return true;
        }

        factory = default;
        return false;
    }
}
