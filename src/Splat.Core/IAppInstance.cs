// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Application instance interface providing access to dependency injection services.
/// </summary>
public interface IAppInstance
{
    /// <summary>
    /// Gets the current services.
    /// </summary>
    /// <value>
    /// The services.
    /// </value>
    IReadonlyDependencyResolver? Current { get; }

    /// <summary>
    /// Gets the mutable service registrar.
    /// </summary>
    /// <value>
    /// The current mutable.
    /// </value>
    IMutableDependencyResolver CurrentMutable { get; }
}
