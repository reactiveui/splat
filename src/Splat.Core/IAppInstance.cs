// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Represents an application instance that provides access to dependency resolution services.
/// </summary>
/// <remarks>Use this interface to access both immutable and mutable service resolvers within the application. The
/// <see cref="Current"/> property exposes the current read-only dependency resolver, while <see cref="CurrentMutable"/>
/// provides access to a mutable service registrar for registering or modifying services at runtime.</remarks>
public interface IAppInstance
{
    /// <summary>
    /// Gets the current dependency resolver in use by the application.
    /// </summary>
    /// <remarks>The returned resolver provides access to registered services and dependencies. If no resolver
    /// is set, this property may return null. Thread safety and resolver lifetime depend on the implementation of the
    /// dependency resolver.</remarks>
    IReadonlyDependencyResolver? Current { get; }

    /// <summary>
    /// Gets the current mutable dependency resolver used for registering and resolving dependencies at runtime.
    /// </summary>
    /// <remarks>The mutable dependency resolver allows for dynamic registration and replacement of services
    /// during application execution. Use this property to add or override service registrations as needed. Thread
    /// safety depends on the implementation of the underlying resolver.</remarks>
    IMutableDependencyResolver CurrentMutable { get; }
}
