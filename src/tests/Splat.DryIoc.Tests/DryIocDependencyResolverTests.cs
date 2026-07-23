// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using DryIoc;
using Splat.Tests.ServiceLocation;

namespace Splat.DryIoc.Tests;

/// <summary>Unit tests for the DryIocDependencyResolver that verify conformance to the IDependencyResolver contract.</summary>
/// <remarks>
/// Inherits from BaseDependencyResolverTests to ensure consistent behavior across all resolver implementations.
/// </remarks>
[NotInParallel]
[InheritsTests]
public sealed class DryIocDependencyResolverTests : BaseDependencyResolverTests<DryIocDependencyResolver>
{
    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_WithExistingRegistration_InvokesImmediately()
    {
        var resolver = GetDependencyResolver();

        // Register a service first so the callback would normally fire immediately; DryIoc still throws.
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>("test", static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_NonGeneric_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback(typeof(Common.Test.ViewModelOne), static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_NonGeneric_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback(typeof(Common.Test.ViewModelOne), "test", static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Disposal_StopsReceivingNotifications()
    {
        var resolver = GetDependencyResolver();

        // The subscription disposable is never produced because DryIoc throws before returning it.
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(static _ => { }).Dispose());
        return Task.CompletedTask;
    }

    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override async Task ServiceRegistrationCallback_NullCallback_Throws()
    {
        var resolver = GetDependencyResolver();
        await Assert.That(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(null!))
            .Throws<NotSupportedException>();
        await Assert.That(() => resolver.ServiceRegistrationCallback(typeof(Common.Test.ViewModelOne), null!))
            .Throws<NotSupportedException>();
    }

    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task ServiceRegistrationCallback_Generic_InvokesForEachExistingRegistration()
    {
        var resolver = GetDependencyResolver();

        // Register multiple services so the callback would normally fire for each; DryIoc still throws.
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task Register_AfterDispose_DoesNotInvokeCallbacks()
    {
        var resolver = GetDependencyResolver();
        resolver.Dispose();

        // After disposal, ServiceRegistrationCallback still throws NotSupportedException for DryIoc.
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(static _ => { }));
        return Task.CompletedTask;
    }

    /// <summary>DryIoc does not support ServiceRegistrationCallback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [ExcludeFromCodeCoverage]
    public override Task Dispose_SuppressesExceptionsFromCallbacks()
    {
        var resolver = GetDependencyResolver();

        // Callbacks can never be registered (DryIoc throws), so disposal has nothing to suppress.
        _ = Assert.Throws<NotSupportedException>(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(static _ => { }));
        resolver.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>DryIoc does not invoke callbacks on disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task Dispose_InvokesCallbacks()
    {
        var resolver = GetDependencyResolver();

        // DryIoc does not support registration callbacks, so there is nothing for disposal to invoke.
        await Assert.That(() => resolver.ServiceRegistrationCallback<Common.Test.ViewModelOne>(static _ => { }))
            .Throws<NotSupportedException>();
        await Assert.That(() =>
        {
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();
    }

    /// <summary>DryIoc manages disposal of services itself.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task Dispose_WhileLazySingletonUnderConstruction_DisposesServiceAndThrowsException()
    {
        var resolver = GetDependencyResolver();
        var service = new DisposableTestService();
        resolver.RegisterLazySingleton(() => service);

        // Force construction so DryIoc tracks the singleton for disposal.
        _ = resolver.GetService<DisposableTestService>();
        await Assert.That(service.IsDisposed).IsFalse();

        resolver.Dispose();

        // DryIoc manages its own disposal lifecycle: constructed singletons are disposed with the container.
        await Assert.That(service.IsDisposed).IsTrue();
    }

    /// <summary>DryIoc does not support null service types.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task GetService_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();

        // DryIoc has no null service-type registration, so resolution returns null rather than an instance.
        await Assert.That(resolver.GetService(null)).IsNull();
    }

    /// <summary>DryIoc does not support null service types.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task GetServices_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();

        // DryIoc has no null service-type registration, so the resolved sequence is empty.
        await Assert.That(resolver.GetServices(null)).IsEmpty();
    }

    /// <summary>DryIoc does not support null service types.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task HasRegistration_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();

        // DryIoc rejects a null service type: HasRegistration guards its argument.
        await Assert.That(() => resolver.HasRegistration(null)).Throws<ArgumentNullException>();
    }

    /// <summary>DryIoc does not support null service types.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterCurrent_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();

        // DryIoc has nothing registered under a null service type, so unregistering is a no-op that does not throw.
        await Assert.That(() => resolver.UnregisterCurrent(null)).ThrowsNothing();
    }

    /// <summary>DryIoc does not support null service types.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterAll_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();

        // DryIoc has nothing registered under a null service type, so unregistering all is a no-op that does not throw.
        await Assert.That(() => resolver.UnregisterAll(null)).ThrowsNothing();
    }

    /// <summary>DryIoc's Unregister implementation has issues with DefaultKey-based registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterCurrent_Remove_Last()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));

        await Assert.That(resolver.HasRegistration(typeof(Common.Test.ViewModelOne))).IsTrue();

        // UnregisterCurrent removes the last (here, only) default registration for the type.
        resolver.UnregisterCurrent(typeof(Common.Test.ViewModelOne));

        await Assert.That(resolver.HasRegistration(typeof(Common.Test.ViewModelOne))).IsFalse();
    }

    /// <summary>DryIoc's Unregister implementation has issues with DefaultKey-based registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterCurrent_Generic_RemovesLastRegistration()
    {
        var resolver = GetDependencyResolver();
        resolver.RegisterConstant(new Common.Test.ViewModelOne());

        await Assert.That(resolver.HasRegistration<Common.Test.ViewModelOne>()).IsTrue();

        resolver.UnregisterCurrent<Common.Test.ViewModelOne>();

        // UnregisterCurrent removes the last (here, only) default registration for the type.
        await Assert.That(resolver.HasRegistration<Common.Test.ViewModelOne>()).IsFalse();
    }

    /// <summary>DryIoc's Unregister implementation has issues with DefaultKey-based registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterAll_Generic_RemovesAllRegistrations()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(static () => new Common.Test.ViewModelOne());

        await Assert.That(resolver.HasRegistration<Common.Test.ViewModelOne>()).IsTrue();

        resolver.UnregisterAll<Common.Test.ViewModelOne>();

        // DryIoc removes the default registration for the service type.
        await Assert.That(resolver.HasRegistration<Common.Test.ViewModelOne>()).IsFalse();
    }

    /// <summary>DryIoc's Unregister implementation has issues with DefaultKey-based registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterCurrent_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.RegisterConstant(new Common.Test.ViewModelOne());

        // A null contract delegates to the default (contract-less) unregister path.
        resolver.UnregisterCurrent<Common.Test.ViewModelOne>(null);

        await Assert.That(resolver.HasRegistration<Common.Test.ViewModelOne>()).IsFalse();
    }

    /// <summary>DryIoc's Unregister implementation has issues with DefaultKey-based registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterCurrent_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));

        // A null contract delegates to the default (contract-less) unregister path, removing only the last.
        resolver.UnregisterCurrent(typeof(Common.Test.ViewModelOne), null);

        await Assert.That(resolver.HasRegistration(typeof(Common.Test.ViewModelOne))).IsTrue();
    }

    /// <summary>DryIoc does not support multiple registrations with the same contract (Replace mode only).</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task GetServices_Generic_WithContract_CombinesGenericAndNonGenericResults()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.Register(static () => new Common.Test.ViewModelOne(), contract);
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne), contract);

        // DryIoc uses Replace mode for keyed (contract) registrations, so only the last survives.
        await Assert.That(resolver.GetServices<Common.Test.ViewModelOne>(contract).Count()).IsEqualTo(1);
    }

    /// <summary>DryIoc does not support multiple registrations with the same contract (Replace mode only).</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterAll_Generic_WithContract_RemovesAllContractRegistrations()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.RegisterConstant(new Common.Test.ViewModelOne(), contract);
        resolver.Register(static () => new Common.Test.ViewModelOne(), contract);

        await Assert.That(resolver.HasRegistration<Common.Test.ViewModelOne>(contract)).IsTrue();

        resolver.UnregisterAll<Common.Test.ViewModelOne>(contract);

        // DryIoc removes the keyed registration for the contract.
        await Assert.That(resolver.HasRegistration<Common.Test.ViewModelOne>(contract)).IsFalse();
    }

    /// <summary>DryIoc's UnregisterAll implementation has issues with DefaultKey-based registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterAll_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(static () => new Common.Test.ViewModelOne());

        // A null contract delegates to the default (contract-less) unregister-all path.
        resolver.UnregisterAll<Common.Test.ViewModelOne>(null);

        await Assert.That(resolver.HasRegistration<Common.Test.ViewModelOne>()).IsFalse();
    }

    /// <summary>DryIoc's UnregisterAll implementation has issues with DefaultKey-based registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task UnregisterAll_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));

        // A null contract delegates to the default (contract-less) unregister-all path.
        resolver.UnregisterAll(typeof(Common.Test.ViewModelOne), null);

        await Assert.That(resolver.HasRegistration(typeof(Common.Test.ViewModelOne))).IsFalse();
    }

    /// <summary>Verifies that querying an unregistered type skips registrations of other types and reports no registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_ForUnregisteredType_WithOtherRegistrationsPresent_ReturnsFalse()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));

        // The only registration is for a different service type, so the predicate skips it and reports no match.
        await Assert.That(resolver.HasRegistration(typeof(Common.Test.ViewModelTwo))).IsFalse();
    }

    /// <summary>Verifies that unregistering the current registration for one type leaves registrations of other types intact.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_ForType_WithOtherRegistrationsPresent_LeavesOthersRegistered()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));
        resolver.Register(static () => new Common.Test.ViewModelTwo(), typeof(Common.Test.ViewModelTwo));

        resolver.UnregisterCurrent(typeof(Common.Test.ViewModelTwo));

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(Common.Test.ViewModelTwo))).IsFalse();
            await Assert.That(resolver.HasRegistration(typeof(Common.Test.ViewModelOne))).IsTrue();
        }
    }

    /// <summary>Verifies that unregistering all registrations for one type leaves registrations of other types intact.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterAll_ForType_WithOtherRegistrationsPresent_LeavesOthersRegistered()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));
        resolver.Register(static () => new Common.Test.ViewModelTwo(), typeof(Common.Test.ViewModelTwo));

        resolver.UnregisterAll(typeof(Common.Test.ViewModelTwo));

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(Common.Test.ViewModelTwo))).IsFalse();
            await Assert.That(resolver.HasRegistration(typeof(Common.Test.ViewModelOne))).IsTrue();
        }
    }

    /// <summary>Verifies the finalizer disposal path leaves the underlying container untouched.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_WhenNotDisposing_LeavesContainerUsable()
    {
        using var resolver = new FinalizableDryIocDependencyResolver(new Container());
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));

        // The non-disposing (finalizer) path must not dispose the container.
        resolver.InvokeDispose(false);

        await Assert.That(resolver.GetService(typeof(Common.Test.ViewModelOne))).IsNotNull();
    }

    /// <summary>Verifies a resolver created without an explicit container provisions its own usable container.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithoutContainer_CreatesUsableResolver()
    {
        using var resolver = new DryIocDependencyResolver();
        resolver.Register(static () => new Common.Test.ViewModelOne(), typeof(Common.Test.ViewModelOne));

        await Assert.That(resolver.GetService(typeof(Common.Test.ViewModelOne))).IsNotNull();
    }

    /// <summary>Verifies that a factory producing <see langword="null"/> resolves to no service.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_WhenFactoryReturnsNull_ReturnsNull()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(static () => null, typeof(Common.Test.ViewModelOne));

        await Assert.That(resolver.GetService(typeof(Common.Test.ViewModelOne))).IsNull();
    }

    /// <inheritdoc />
    protected override DryIocDependencyResolver GetDependencyResolver() => new(new Container());

    /// <summary>A resolver that exposes the protected disposal method so the finalizer path can be exercised.</summary>
    /// <param name="container">The DryIoc container to resolve against.</param>
    private sealed class FinalizableDryIocDependencyResolver(IContainer container) : DryIocDependencyResolver(container)
    {
        /// <summary>Invokes the protected <see cref="DryIocDependencyResolver.Dispose(bool)"/> overload.</summary>
        /// <param name="disposing">Whether the call represents a deterministic dispose (<see langword="true"/>) or a finalizer (<see langword="false"/>).</param>
        public void InvokeDispose(bool disposing) => Dispose(disposing);
    }
}
