﻿// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;
using Splat.Tests.ServiceLocation;

namespace Splat.Ninject.Tests;

/// <summary>
/// Unit Tests for the Modern Dependency Resolver.
/// </summary>
[TestFixture]
public sealed class NInjectDependencyResolverTests : BaseDependencyResolverTests<NinjectDependencyResolver>
{
    /// <summary>
    /// Test to ensure container allows registration with null service type.
    /// Should really be brought down to the <see cref="BaseDependencyResolverTests{T}"/>,
    /// it fails for some of the DIs.
    /// </summary>
    [Test]
    public void Can_Register_And_Resolve_Null_Types()
    {
        var resolver = GetDependencyResolver();

        const int foo = 5;
        resolver.Register(() => foo, null);

        const int bar = 4;
        const string contract = "foo";
        resolver.Register(() => bar, null, contract);

        Assert.That(resolver.HasRegistration(null), Is.True);

        var value = resolver.GetService(null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(value, Is.EqualTo(foo));

            Assert.That(resolver.HasRegistration(null, contract), Is.True);
        }

        value = resolver.GetService(null, contract);
        Assert.That(value, Is.EqualTo(bar));

        var values = resolver.GetServices(null);
        Assert.That(values.Count(), Is.EqualTo(1));

        resolver.UnregisterCurrent(null);

        var valuesNC = resolver.GetServices(null);
        Assert.That(valuesNC.Count(), Is.Zero);

        var valuesC = resolver.GetServices(null, contract);
        Assert.That(valuesC.Count(), Is.EqualTo(1));

        resolver.UnregisterAll(null);

        valuesNC = resolver.GetServices(null);
        Assert.That(valuesNC.Count(), Is.Zero);

        resolver.UnregisterAll(null, contract);

        valuesC = resolver.GetServices(null, contract);
        Assert.That(valuesC.Count(), Is.Zero);
    }

    /// <inheritdoc />
    protected override NinjectDependencyResolver GetDependencyResolver() => new(new StandardKernel());
}
