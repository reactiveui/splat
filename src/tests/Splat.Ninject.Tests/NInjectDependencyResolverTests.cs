// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;

using Splat.Tests.ServiceLocation;

namespace Splat.Ninject.Tests;

[NotInParallel]
[InheritsTests]
public sealed class NInjectDependencyResolverTests : BaseDependencyResolverTests<NinjectDependencyResolver>
{
    /// <summary>
    /// Test to ensure container allows registration with null service type.
    /// Should really be brought down to the <see cref="BaseDependencyResolverTests{T}"/>,
    /// it fails for some of the DIs.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Can_Register_And_Resolve_Null_Types()
    {
        var resolver = GetDependencyResolver();

        const int foo = 5;
        resolver.Register(() => foo, null);

        const int bar = 4;
        const string contract = "foo";
        resolver.Register(() => bar, null, contract);

        await Assert.That(resolver.HasRegistration(null)).IsTrue();

        var value = resolver.GetService(null);
        using (Assert.Multiple())
        {
            await Assert.That(value).IsEqualTo(foo);

            await Assert.That(resolver.HasRegistration(null, contract)).IsTrue();
        }

        value = resolver.GetService(null, contract);
        await Assert.That(value).IsEqualTo(bar);

        var values = resolver.GetServices(null);
        await Assert.That(values.Count()).IsEqualTo(1);

        resolver.UnregisterCurrent(null);

        var valuesNC = resolver.GetServices(null);
        await Assert.That(valuesNC.Count()).IsEqualTo(0);

        var valuesC = resolver.GetServices(null, contract);
        await Assert.That(valuesC.Count()).IsEqualTo(1);

        resolver.UnregisterAll(null);

        valuesNC = resolver.GetServices(null);
        await Assert.That(valuesNC.Count()).IsEqualTo(0);

        resolver.UnregisterAll(null, contract);

        valuesC = resolver.GetServices(null, contract);
        await Assert.That(valuesC.Count()).IsEqualTo(0);
    }

    /// <inheritdoc />
    protected override NinjectDependencyResolver GetDependencyResolver() => new(new StandardKernel());
}
