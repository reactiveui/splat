// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Unit Tests for the Modern Dependency Resolver.
/// </summary>
public sealed class ModernDependencyResolverTests : BaseDependencyResolverTests<ModernDependencyResolver>
{
    /// <inheritdoc />
    protected override ModernDependencyResolver GetDependencyResolver() => new();
}
