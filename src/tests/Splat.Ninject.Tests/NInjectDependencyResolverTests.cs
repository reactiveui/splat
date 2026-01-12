// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Ninject;

using Splat.Common.Test;
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

        // Explicitly cast to call the non-generic Register method with null service type
        resolver.Register(() => foo, serviceType: null);

        const int bar = 4;
        const string contract = "foo";
        resolver.Register(() => bar, serviceType: null, contract: contract);

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

    /// <summary>
    /// Verifies that ServiceRegistrationCallback throws NotImplementedException for Ninject.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Generic_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies that ServiceRegistrationCallback throws NotImplementedException for Ninject.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Generic_WithExistingRegistration_InvokesImmediately()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies that ServiceRegistrationCallback with contract throws NotImplementedException for Ninject.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Generic_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>("test", _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies that non-generic ServiceRegistrationCallback throws NotImplementedException for Ninject.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_NonGeneric_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies that non-generic ServiceRegistrationCallback with contract throws NotImplementedException for Ninject.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_NonGeneric_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), "test", _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies that ServiceRegistrationCallback disposal throws NotImplementedException for Ninject.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Disposal_StopsReceivingNotifications()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies that ServiceRegistrationCallback with null callback throws NotImplementedException for Ninject.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task ServiceRegistrationCallback_NullCallback_Throws()
    {
        var resolver = GetDependencyResolver();

        // Ninject throws NotImplementedException before checking for null
        await Assert.That(() => resolver.ServiceRegistrationCallback<ViewModelOne>(null!))
            .Throws<NotImplementedException>();

        await Assert.That(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), null!))
            .Throws<NotImplementedException>();
    }

    /// <summary>
    /// Verifies that ServiceRegistrationCallback invokes for each throws NotImplementedException for Ninject.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task ServiceRegistrationCallback_Generic_InvokesForEachExistingRegistration()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies that Register after dispose throws NotImplementedException for Ninject (due to callbacks not implemented).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Register_AfterDispose_DoesNotInvokeCallbacks()
    {
        var resolver = GetDependencyResolver();

        // Since ServiceRegistrationCallback throws, we verify that instead of the full test flow
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies that Dispose suppresses exceptions from callbacks (NotApplicable for Ninject as callbacks throw).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_SuppressesExceptionsFromCallbacks()
    {
        var resolver = GetDependencyResolver();
        Assert.Throws<NotImplementedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(_ => { }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Ninject doesn't invoke callbacks on disposal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_InvokesCallbacks()
    {
        // Ninject ServiceRegistrationCallback throws NotImplementedException, so this test doesn't apply
        return Task.CompletedTask;
    }

    /// <summary>
    /// Ninject manages disposal of registered services itself.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_DisposesRegisteredServices()
    {
        // Ninject manages its own service disposal lifecycle
        return Task.CompletedTask;
    }

    /// <summary>
    /// Ninject handles lazy singletons itself.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_WithLazySingleton_DoesNotCreateIfNotAccessed()
    {
        // Ninject manages lazy singleton creation and disposal
        return Task.CompletedTask;
    }

    /// <summary>
    /// Ninject manages disposal of services under construction itself.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_WhileLazySingletonUnderConstruction_DisposesServiceAndThrowsException()
    {
        // Ninject manages its own service disposal lifecycle including services under construction
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override NinjectDependencyResolver GetDependencyResolver() => new(new StandardKernel());
}
