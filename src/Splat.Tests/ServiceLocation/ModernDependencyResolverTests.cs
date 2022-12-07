﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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
