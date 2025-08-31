// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A container for Locator which will host the container for dependency injection based operations.
/// </summary>
public static class AppLocatorCore
{
    /// <summary>
    /// Gets the read only dependency resolver. This class is used throughout
    /// libraries for many internal operations as well as for general use
    /// by applications. If this isn't assigned on startup, a default, highly
    /// capable implementation will be used, and it is advised for most people
    /// to simply use the default implementation.
    /// </summary>
    /// <value>The dependency resolver.</value>
    public static IReadonlyDependencyResolver? Current { get; internal set; }

    /// <summary>
    /// Gets the mutable dependency resolver.
    /// The default resolver is also a mutable resolver, so this will be non-null.
    /// Use this to register new types on startup if you are using the default resolver.
    /// </summary>
    public static IMutableDependencyResolver? CurrentMutable { get; internal set; }
}
