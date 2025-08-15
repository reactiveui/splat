// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat;

namespace ReactiveUI.Builder;

/// <summary>
/// Defines a contract for ReactiveUI modules that can configure dependency injection.
/// This provides an AOT-compatible way to register services.
/// </summary>
public interface IReactiveUIModule
{
    /// <summary>
    /// Configures the dependency resolver with the module's services.
    /// </summary>
    /// <param name="resolver">The dependency resolver to configure.</param>
    void Configure(IMutableDependencyResolver resolver);
}
