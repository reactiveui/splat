// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents a dependency resolver, a service to look up global class
/// instances or types.
/// </summary>
public interface IDependencyResolver : IReadonlyDependencyResolver, IMutableDependencyResolver, IDisposable;
