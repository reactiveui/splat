// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Splat.Common.Test;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests that verify the Microsoft.Extensions.DependencyInjection dependency resolver conforms to the base resolver contract.</summary>
[NotInParallel]
[InheritsTests]
public sealed class MicrosoftDependencyResolverTests : BaseDependencyResolverTests<MicrosoftDependencyResolver>
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
        resolver.Register(static () => foo, serviceType: null);

        const int bar = 4;
        const string contract = "foo";
        resolver.Register(static () => bar, serviceType: null, contract: contract);

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

    /// <summary>Verifies that ServiceRegistrationCallback throws NotSupportedException for MS.DI.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that ServiceRegistrationCallback throws NotSupportedException for MS.DI.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_WithExistingRegistration_InvokesImmediately()
    {
        var resolver = GetDependencyResolver();

        // Register a service first so the callback would normally fire immediately; MS.DI still throws.
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that ServiceRegistrationCallback with contract throws NotSupportedException for MS.DI.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>("test", static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that non-generic ServiceRegistrationCallback throws NotSupportedException for MS.DI.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_NonGeneric_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that non-generic ServiceRegistrationCallback with contract throws NotSupportedException for MS.DI.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_NonGeneric_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), "test", static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that ServiceRegistrationCallback disposal throws NotSupportedException for MS.DI.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Disposal_StopsReceivingNotifications()
    {
        var resolver = GetDependencyResolver();

        // The subscription disposable is never produced because MS.DI throws before returning it.
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(static _ => { }).Dispose());
        return Task.CompletedTask;
    }

    /// <summary>Verifies that ServiceRegistrationCallback with null callback throws NotSupportedException for MS.DI.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override async Task ServiceRegistrationCallback_NullCallback_Throws()
    {
        var resolver = GetDependencyResolver();

        // MS.DI throws NotSupportedException before checking for null
        await Assert.That(() => resolver.ServiceRegistrationCallback<ViewModelOne>(null!))
            .Throws<NotSupportedException>();

        await Assert.That(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), null!))
            .Throws<NotSupportedException>();
    }

    /// <summary>Verifies that ServiceRegistrationCallback invokes for each throws NotSupportedException for MS.DI.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_InvokesForEachExistingRegistration()
    {
        var resolver = GetDependencyResolver();

        // Register multiple services so the callback would normally fire for each; MS.DI still throws.
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>Verifies that Register after dispose throws NotSupportedException for MS.DI (due to callbacks not implemented).</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override async Task Register_AfterDispose_DoesNotInvokeCallbacks()
    {
        var resolver = GetDependencyResolver();
        await resolver.DisposeAsync();

        // After disposal, ServiceRegistrationCallback still throws NotSupportedException for MS.DI.
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(static _ => { }));
    }

    /// <summary>Verifies that Dispose suppresses exceptions from callbacks (NotApplicable for MS.DI as callbacks throw).</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override async Task Dispose_SuppressesExceptionsFromCallbacks()
    {
        var resolver = GetDependencyResolver();

        // Callbacks can never be registered (MS.DI throws), so disposal has nothing to suppress.
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<ViewModelOne>(static _ => { }));
        await resolver.DisposeAsync();
    }

    /// <summary>MS.DI doesn't invoke callbacks on disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task Dispose_InvokesCallbacks()
    {
        var resolver = GetDependencyResolver();

        // MS.DI does not support registration callbacks, so there is nothing for disposal to invoke.
        await Assert.That(() => resolver.ServiceRegistrationCallback<ViewModelOne>(static _ => { }))
            .Throws<NotSupportedException>();
        await resolver.DisposeAsync();
    }

    /// <summary>MS.DI disposes the factory-created services it owns on disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task Dispose_DisposesRegisteredServices()
    {
        var resolver = GetDependencyResolver();
        var disposableService = new DisposableTestService();

        // A factory-backed singleton is created and owned by the provider (unlike a constant instance, which MS.DI leaves alone).
        resolver.RegisterLazySingleton(() => disposableService);

        _ = resolver.GetService<DisposableTestService>();
        await Assert.That(disposableService.IsDisposed).IsFalse();

        await resolver.DisposeAsync();

        // MS.DI disposes the services it creates when the provider is disposed.
        await Assert.That(disposableService.IsDisposed).IsTrue();
    }

    /// <summary>MS.DI does not create an unresolved lazy singleton just to dispose it.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task Dispose_WithLazySingleton_DoesNotCreateIfNotAccessed()
    {
        var resolver = GetDependencyResolver();
        var factoryCalled = false;
        resolver.RegisterLazySingleton<DisposableTestService>(() =>
        {
            factoryCalled = true;
            return new();
        });

        await Assert.That(factoryCalled).IsFalse();

        await resolver.DisposeAsync();

        // MS.DI does not activate an unresolved singleton just to dispose it.
        await Assert.That(factoryCalled).IsFalse();
    }

    /// <inheritdoc />
    protected override MicrosoftDependencyResolver GetDependencyResolver() => new();
}
