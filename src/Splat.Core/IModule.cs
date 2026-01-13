// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Defines a contract for a module that can register its services with a dependency resolver.
/// </summary>
/// <remarks>Implement this interface to provide custom service registrations for use with a dependency injection
/// container. Modules are typically used to organize related service registrations and can be composed to build
/// application functionality.</remarks>
public interface IModule
{
    /// <summary>
    /// Configures the specified dependency resolver with required services and components.
    /// </summary>
    /// <param name="resolver">The dependency resolver to configure. Cannot be null.</param>
    void Configure(IMutableDependencyResolver resolver);
}
