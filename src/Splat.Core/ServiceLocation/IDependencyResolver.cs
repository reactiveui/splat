// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents a dependency resolver that supports both read-only and mutable operations, as well as resource cleanup.
/// </summary>
/// <remarks>IDependencyResolver combines the capabilities of IReadonlyDependencyResolver and
/// IMutableDependencyResolver, allowing for both retrieval and registration of dependencies. Implementations should
/// ensure proper disposal of resources when no longer needed by implementing IDisposable.</remarks>
public interface IDependencyResolver : IReadonlyDependencyResolver, IMutableDependencyResolver, IDisposable;
