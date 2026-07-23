// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for the InstanceGenericFirstDependencyResolver.
/// Verifies that the instance-scoped generic-first resolver with ConditionalWeakTable
/// properly implements all IDependencyResolver functionality with per-resolver isolation.
/// </summary>
[InheritsTests]
public sealed class InstanceGenericFirstDependencyResolverTests : BaseDependencyResolverTests<InstanceGenericFirstDependencyResolver>
{
    /// <summary>The value produced by a <see cref="NullServiceType"/>-wrapped registration once unwrapped.</summary>
    private const string WrappedValue = "wrapped";

    /// <summary>A plain, non-wrapped service value.</summary>
    private const string PlainValue = "plain";

    /// <summary>The number of services expected from a mixed wrapped/plain non-generic registration.</summary>
    private const int MixedServiceCount = 2;

    /// <summary>The number of times the change callback is expected to fire for a single registration.</summary>
    private const int ExpectedCallbackInvocations = 1;

    /// <summary>Verifies that resolving all non-generic services unwraps <see cref="NullServiceType"/> markers in the registry results.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_NonGeneric_UnwrapsNullServiceTypeMarkers()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.Register(static () => (object?)new NullServiceType(static () => WrappedValue), typeof(object));
        resolver.Register(static () => (object?)PlainValue, typeof(object));

        var services = resolver.GetServices(typeof(object)).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services.Count).IsEqualTo(MixedServiceCount);
            await Assert.That(services).Contains(WrappedValue);
            await Assert.That(services).Contains(PlainValue);
        }
    }

    /// <summary>Verifies that an exception thrown by a registration-change callback during registration is suppressed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_WhenRegistrationCallbackThrows_DoesNotPropagate()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

        var invocations = 0;

        // Subscribe before any matching registration so the callback is not invoked during subscription.
        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ =>
        {
            invocations++;
            throw new InvalidOperationException("callback failure");
        });

        // The throwing callback fires during the registration notification; the exception must be suppressed.
        await Assert.That(() =>
        {
            resolver.RegisterConstant(new ViewModelOne());
            return Task.CompletedTask;
        }).ThrowsNothing();

        using (Assert.Multiple())
        {
            await Assert.That(invocations).IsEqualTo(ExpectedCallbackInvocations);
            await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsTrue();
        }
    }

    /// <summary>Verifies that an exception thrown while disposing a created lazy singleton is suppressed on disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_WhenLazySingletonDisposalThrows_DoesNotPropagate()
    {
        var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterLazySingleton(static () => new ThrowingDisposableService());

        // Force creation so the lazy value exists and is disposed during teardown.
        var service = resolver.GetService<ThrowingDisposableService>();
        await Assert.That(service).IsNotNull();

        await Assert.That(() =>
        {
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();
    }

    /// <summary>
    /// Tests that a generic registration callback fires only for registrations of its own service type,
    /// not for unrelated ones (issue #1589 callback-scoping regression).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_Generic_OnlyFiresForMatchingServiceType()
    {
        var resolver = new InstanceGenericFirstDependencyResolver();
        var totalInvocations = 0;
        var matchingInvocations = 0;

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ =>
        {
            totalInvocations++;
            matchingInvocations += resolver.HasRegistration<ViewModelOne>() ? 1 : 0;
        });

        // An unrelated registration must not trigger the ViewModelOne callback.
        resolver.RegisterConstant(new ViewModelTwo());
        await Assert.That(totalInvocations).IsEqualTo(0);

        // The matching registration must trigger it exactly once, while the service is resolvable.
        resolver.RegisterConstant(new ViewModelOne());
        using (Assert.Multiple())
        {
            await Assert.That(totalInvocations).IsEqualTo(1);
            await Assert.That(matchingInvocations).IsEqualTo(1);
        }
    }

    /// <summary>Tests that registration callbacks do not fire on unregistration, matching ModernDependencyResolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_Generic_DoesNotFireOnUnregister()
    {
        var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant(new ViewModelOne());

        var invocations = 0;
        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ => invocations++);

        // The single existing registration is replayed once on subscribe; nothing else should fire.
        var baseline = invocations;

        resolver.UnregisterCurrent<ViewModelOne>();
        resolver.UnregisterAll<ViewModelOne>();

        await Assert.That(invocations).IsEqualTo(baseline);
    }

    /// <summary>
    /// Tests that a nested generic lookup for a not-yet-registered type during another resolution does not
    /// permanently flag that type as unresolvable (issue #1589 nested-resolution regression).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_Generic_WhenMissingDuringNestedResolution_ResolvesAfterLaterRegistration()
    {
        var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterLazySingleton(() => new NestedResolutionProbe<ViewModelOne>(resolver));

        var probe = resolver.GetService<NestedResolutionProbe<ViewModelOne>>();

        await Assert.That(probe).IsNotNull();
        await Assert.That(probe!.Service).IsNull();

        var service = new ViewModelOne();
        resolver.RegisterConstant(service);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsTrue();
            await Assert.That(resolver.GetService<ViewModelOne>()).IsSameReferenceAs(service);
        }
    }

    /// <summary>Test constructor with configure parameter registers services.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithConfigure_RegistersServices()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver(static r =>
        {
            r.Register<IViewModelOne>(static () => new ViewModelOne());
            r.RegisterConstant(new ViewModelOne());
        });

        var result1 = resolver.GetService<IViewModelOne>();
        var result2 = resolver.GetService<ViewModelOne>();

        await Assert.That(result1).IsNotNull();
        await Assert.That(result2).IsNotNull();
    }

    /// <summary>Test constructor with null configure parameter does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithNullConfigure_DoesNotThrow() =>
        await Assert.That(static () =>
        {
            _ = new InstanceGenericFirstDependencyResolver(null);
            return Task.CompletedTask;
        }).ThrowsNothing();

    /// <summary>Test Dispose method disposes resolver properly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_DisposesResolver()
    {
        var resolver = new InstanceGenericFirstDependencyResolver();
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
        var resolver = new InstanceGenericFirstDependencyResolver();
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

    /// <inheritdoc />
    protected override InstanceGenericFirstDependencyResolver GetDependencyResolver() => new();
}
