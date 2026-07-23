// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for the GlobalGenericFirstDependencyResolver.
/// Verifies that the AOT-friendly generic-first resolver properly implements all IDependencyResolver functionality.
/// </summary>
[NotInParallel]
[InheritsTests]
public sealed class GlobalGenericFirstDependencyResolverTests : BaseDependencyResolverTests<GlobalGenericFirstDependencyResolver>
{
    /// <summary>The contract used when probing for a service that was never registered under that contract.</summary>
    private const string MissingContract = "missing";

    /// <summary>The value produced by a <see cref="NullServiceType"/>-wrapped registration once unwrapped.</summary>
    private const string WrappedValue = "wrapped";

    /// <summary>A plain, non-wrapped service value.</summary>
    private const string PlainValue = "plain";

    /// <summary>The number of services expected from a mixed wrapped/plain non-generic registration.</summary>
    private const int MixedServiceCount = 2;

    /// <summary>The number of times the change callback is expected to fire for a single registration.</summary>
    private const int ExpectedCallbackInvocations = 1;

    /// <summary>Teardown method to clear static generic containers after each test. Ensures no state leaks between test runs.</summary>
    [After(Test)]
    public void ClearStaticContainers() => GlobalGenericFirstDependencyResolver.Clear();

    /// <summary>
    /// Verifies that a generic resolve for a type with no generic or non-generic registration returns the default
    /// value, even when unrelated registrations exist (so the never-registered fast path is not taken).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_Generic_WhenOnlyUnrelatedTypeRegistered_ReturnsDefault()
    {
        using var resolver = new GlobalGenericFirstDependencyResolver();

        // A non-generic registration for a different type marks the process as registered, so the generic
        // GetService evaluates its no-non-generic-registration branch for the unregistered type.
        resolver.Register(static () => new ViewModelTwo(), typeof(ViewModelTwo));

        var result = resolver.GetService<IViewModelOne>();

        await Assert.That(result).IsNull();
        GlobalGenericFirstDependencyResolver.Clear();
    }

    /// <summary>
    /// Verifies that a contract-scoped generic resolve for a type with no matching registration returns the
    /// default value, even when unrelated registrations exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_GenericWithContract_WhenNoContractRegistration_ReturnsDefault()
    {
        using var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Register(static () => new ViewModelTwo(), typeof(ViewModelTwo));

        var result = resolver.GetService<IViewModelOne>(MissingContract);

        await Assert.That(result).IsNull();
        GlobalGenericFirstDependencyResolver.Clear();
    }

    /// <summary>Verifies that resolving all non-generic services unwraps <see cref="NullServiceType"/> markers in the registry results.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_NonGeneric_UnwrapsNullServiceTypeMarkers()
    {
        using var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Register(static () => (object?)new NullServiceType(static () => WrappedValue), typeof(object));
        resolver.Register(static () => (object?)PlainValue, typeof(object));

        var services = resolver.GetServices(typeof(object)).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services.Count).IsEqualTo(MixedServiceCount);
            await Assert.That(services).Contains(WrappedValue);
            await Assert.That(services).Contains(PlainValue);
        }

        GlobalGenericFirstDependencyResolver.Clear();
    }

    /// <summary>Verifies that an exception thrown by a registration-change callback during registration is suppressed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_WhenRegistrationCallbackThrows_DoesNotPropagate()
    {
        using var resolver = new GlobalGenericFirstDependencyResolver();

        var invocations = 0;

        // Subscribe before any registration exists so the callback is not invoked during subscription.
        using var subscription = resolver.ServiceRegistrationCallback<IViewModelOne>(_ =>
        {
            invocations++;
            throw new InvalidOperationException("callback failure");
        });

        // The throwing callback fires during the registration notification; the exception must be suppressed.
        await Assert.That(() =>
        {
            resolver.Register<IViewModelOne>(static () => new ViewModelOne());
            return Task.CompletedTask;
        }).ThrowsNothing();

        using (Assert.Multiple())
        {
            await Assert.That(invocations).IsEqualTo(ExpectedCallbackInvocations);
            await Assert.That(resolver.HasRegistration<IViewModelOne>()).IsTrue();
        }

        GlobalGenericFirstDependencyResolver.Clear();
    }

    /// <summary>Verifies that an exception thrown while disposing a created lazy singleton is suppressed on disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_WhenLazySingletonDisposalThrows_DoesNotPropagate()
    {
        var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.RegisterLazySingleton(static () => new ThrowingDisposableService());

        // Force creation so the lazy value exists and is disposed during teardown.
        var service = resolver.GetService<ThrowingDisposableService>();
        await Assert.That(service).IsNotNull();

        await Assert.That(() =>
        {
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();

        GlobalGenericFirstDependencyResolver.Clear();
    }

    /// <summary>Test constructor with configure parameter registers services.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithConfigure_RegistersServices()
    {
        using var resolver = new GlobalGenericFirstDependencyResolver(static r =>
        {
            r.Register<IViewModelOne>(static () => new ViewModelOne());
            r.RegisterConstant(new ViewModelOne());
        });

        var result1 = resolver.GetService<IViewModelOne>();
        var result2 = resolver.GetService<ViewModelOne>();

        await Assert.That(result1).IsNotNull();
        await Assert.That(result2).IsNotNull();

        GlobalGenericFirstDependencyResolver.Clear();
    }

    /// <summary>Test constructor with null configure parameter does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithNullConfigure_DoesNotThrow() =>
        await Assert.That(static () =>
        {
            _ = new GlobalGenericFirstDependencyResolver(null);
            return Task.CompletedTask;
        }).ThrowsNothing();

    /// <summary>Test Clear method removes all registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Clear_RemovesAllRegistrations()
    {
        using var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Register<IViewModelOne>(static () => new ViewModelOne());
        resolver.Register(static () => new ViewModelOne());

        await Assert.That(resolver.HasRegistration<IViewModelOne>()).IsTrue();
        await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsTrue();

        GlobalGenericFirstDependencyResolver.Clear();

        await Assert.That(resolver.HasRegistration<IViewModelOne>()).IsFalse();
        await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsFalse();
    }

    /// <summary>Test Dispose method disposes resolver properly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_DisposesResolver()
    {
        var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Register(static () => new ViewModelOne());

        resolver.Dispose();

        await Assert.That(() =>
        {
            resolver.Register(static () => new ViewModelOne());
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();
    }

    /// <summary>Test operations after disposal throw ObjectDisposedException.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_Operations_ThrowObjectDisposedException()
    {
        var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Dispose();

        await Assert.That(() =>
        {
            resolver.Register(static () => new ViewModelOne());
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();

        await Assert.That(() =>
        {
            resolver.Register(static () => new ViewModelOne(), "contract");
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();

        await Assert.That(() =>
        {
            resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();

        await Assert.That(() =>
        {
            resolver.Register<IViewModelOne, ViewModelOne>();
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();
    }

    /// <summary>Test UnregisterCurrent after disposal throws ObjectDisposedException.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_UnregisterCurrent_ThrowsObjectDisposedException()
    {
        var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Dispose();

        await Assert.That(() =>
        {
            resolver.UnregisterCurrent<ViewModelOne>();
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();

        await Assert.That(() =>
        {
            resolver.UnregisterCurrent(typeof(ViewModelOne));
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();
    }

    /// <summary>Test UnregisterAll after disposal throws ObjectDisposedException.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_UnregisterAll_ThrowsObjectDisposedException()
    {
        var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Dispose();

        await Assert.That(() =>
        {
            resolver.UnregisterAll<ViewModelOne>();
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();

        await Assert.That(() =>
        {
            resolver.UnregisterAll(typeof(ViewModelOne));
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();
    }

    /// <inheritdoc />
    protected override GlobalGenericFirstDependencyResolver GetDependencyResolver() => new();
}
