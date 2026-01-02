// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents a placeholder service type for null service registrations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NullServiceType"/> class.
/// </remarks>
/// <param name="factory">The value factory.</param>
public class NullServiceType(Func<object?> factory)
{
    /// <summary>
    /// Cached Type instance for NullServiceType to avoid repeated typeof() calls.
    /// </summary>
    public static readonly Type CachedType = typeof(NullServiceType);

    /// <summary>
    /// Gets the Factory.
    /// </summary>
    public Func<object?> Factory { get; } = factory;
}
