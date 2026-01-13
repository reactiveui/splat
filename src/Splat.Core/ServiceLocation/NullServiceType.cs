// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents a service type that always returns null when resolved. Used as a placeholder or marker in dependency
/// injection scenarios where a service is intentionally absent.
/// </summary>
/// <param name="factory">A delegate that produces the service instance. The delegate should return null to indicate the absence of a service.</param>
public class NullServiceType(Func<object?> factory)
{
    /// <summary>
    /// Gets the cached <see cref="Type"/> instance representing the <see cref="NullServiceType"/> type.
    /// </summary>
    /// <remarks>This field can be used to avoid repeated calls to <see cref="Type.GetType"/> or <see
    /// langword="typeof"/> for <see cref="NullServiceType"/>. It is intended for scenarios where a reference to the
    /// <see cref="NullServiceType"/> type is needed multiple times.</remarks>
    public static readonly Type CachedType = typeof(NullServiceType);

    /// <summary>
    /// Gets the function used to create an instance of the associated object.
    /// </summary>
    public Func<object?> Factory { get; } = factory;
}
