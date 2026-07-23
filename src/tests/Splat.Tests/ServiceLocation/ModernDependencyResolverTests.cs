// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for the <see cref="ModernDependencyResolver"/> class.</summary>
[NotInParallel]
[InheritsTests]
public sealed class ModernDependencyResolverTests : BaseDependencyResolverTests<ModernDependencyResolver>
{
    /// <summary>The number of existing registrations expected before the callback runs.</summary>
    private const int ExistingRegistrationCount = 3;

    /// <summary>The number of registrations expected when a duplicate is registered.</summary>
    private const int DuplicatedRegistrationCount = 2;

    /// <summary>The number of non-generic registrations made before subscribing a callback.</summary>
    private const int NonGenericExistingCount = 2;

    /// <summary>The number of times a one-shot callback is expected to fire.</summary>
    private const int SingleInvocation = 1;

    /// <summary>Test ServiceRegistrationCallback with null service type throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_NullServiceType_Throws()
    {
        var resolver = new ModernDependencyResolver();

        await Assert.That(() => resolver.ServiceRegistrationCallback(null!, static _ => { }))
            .Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that a generic registration callback fires only for registrations of its own service type.
    /// Serves as the parity baseline for the <c>InstanceGenericFirstDependencyResolver</c> fix (issue #1589).
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_Generic_OnlyFiresForMatchingServiceType()
    {
        var resolver = new ModernDependencyResolver();
        var totalInvocations = 0;
        var matchingInvocations = 0;

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ =>
        {
            totalInvocations++;
            matchingInvocations += resolver.HasRegistration<ViewModelOne>() ? 1 : 0;
        });

        resolver.RegisterConstant(new ViewModelTwo());
        await Assert.That(totalInvocations).IsEqualTo(0);

        resolver.RegisterConstant(new ViewModelOne());
        using (Assert.Multiple())
        {
            await Assert.That(totalInvocations).IsEqualTo(1);
            await Assert.That(matchingInvocations).IsEqualTo(1);
        }
    }

    /// <summary>Tests that registration callbacks do not fire on unregistration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_Generic_DoesNotFireOnUnregister()
    {
        var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant(new ViewModelOne());

        var invocations = 0;
        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ => invocations++);

        var baseline = invocations;

        resolver.UnregisterCurrent<ViewModelOne>();
        resolver.UnregisterAll<ViewModelOne>();

        await Assert.That(invocations).IsEqualTo(baseline);
    }

    /// <summary>
    /// Tests that a nested generic lookup for a not-yet-registered type during another resolution does not
    /// permanently flag that type as unresolvable.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_Generic_WhenMissingDuringNestedResolution_ResolvesAfterLaterRegistration()
    {
        var resolver = new ModernDependencyResolver();
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

    /// <summary>Test ServiceRegistrationCallback invoked once per existing registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_InvokedOncePerExistingRegistration()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne());
        resolver.Register(static () => new ViewModelOne());
        resolver.Register(static () => new ViewModelOne());

        var callbackCount = 0;

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ => callbackCount++);

        await Assert.That(callbackCount).IsEqualTo(ExistingRegistrationCount); // Called once for each existing registration
    }

    /// <summary>Test ServiceRegistrationCallback after resolver disposal returns empty disposable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_AfterDisposal_ReturnsEmptyDisposable()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Dispose();

        var callbackInvoked = false;

        await Assert.That(() =>
        {
            var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ => callbackInvoked = true);
            subscription.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();

        await Assert.That(callbackInvoked).IsFalse();
    }

    /// <summary>Test Dispose method disposes resolver properly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_DisposesResolver()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne());

        await Assert.That(() =>
        {
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();
    }

    /// <summary>Test Duplicate method creates a copy with same registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Duplicate_CreatesResolverWithSameRegistrations()
    {
        var resolver = new ModernDependencyResolver();
        var instance = new ViewModelOne();
        resolver.RegisterConstant(instance);
        resolver.Register<IViewModelOne>(static () => new ViewModelOne());

        var duplicate = resolver.Duplicate();

        var result1 = duplicate.GetService<ViewModelOne>();
        var result2 = duplicate.GetService<IViewModelOne>();

        await Assert.That(result1).IsSameReferenceAs(instance);
        await Assert.That(result2).IsNotNull();
    }

    /// <summary>Test Duplicate of disposed resolver returns empty resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Duplicate_OfDisposedResolver_ReturnsEmptyResolver()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne());
        resolver.Dispose();

        var duplicate = resolver.Duplicate();

        await Assert.That(duplicate.HasRegistration<ViewModelOne>()).IsFalse();
    }

    /// <summary>Test Duplicate creates independent resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Duplicate_CreatesIndependentResolver()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne());

        var duplicate = resolver.Duplicate();

        duplicate.Register(static () => new ViewModelOne());

        var originalServices = resolver.GetServices<ViewModelOne>().ToList();
        var duplicateServices = duplicate.GetServices<ViewModelOne>().ToList();

        await Assert.That(originalServices).Count().IsEqualTo(1);
        await Assert.That(duplicateServices).Count().IsEqualTo(DuplicatedRegistrationCount);
    }

    /// <summary>Test GetService throws ObjectDisposedException after disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_GetService_ThrowsObjectDisposedException()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne());
        resolver.Dispose();

        await Assert.That(() => resolver.GetService<ViewModelOne>())
            .Throws<ObjectDisposedException>();
    }

    /// <summary>Test HasRegistration returns false after disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_HasRegistration_ReturnsFalse()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne());
        resolver.Dispose();

        var result = resolver.HasRegistration<ViewModelOne>();

        await Assert.That(result).IsFalse();
    }

    /// <summary>Test HasRegistration with a runtime type returns false after disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_HasRegistration_NonGeneric_ReturnsFalse()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));
        resolver.Dispose();

        await Assert.That(resolver.HasRegistration(typeof(ViewModelOne))).IsFalse();
    }

    /// <summary>Test GetService with a runtime type throws after disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_GetService_NonGeneric_ThrowsObjectDisposedException()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));
        resolver.Dispose();

        await Assert.That(() => resolver.GetService(typeof(ViewModelOne)))
            .Throws<ObjectDisposedException>();
    }

    /// <summary>Test GetServices with a runtime type throws after disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_GetServices_NonGeneric_ThrowsObjectDisposedException()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));
        resolver.Dispose();

        await Assert.That(() => resolver.GetServices(typeof(ViewModelOne)))
            .Throws<ObjectDisposedException>();
    }

    /// <summary>Test generic GetServices throws after disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_GetServices_Generic_ThrowsObjectDisposedException()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne());
        resolver.Dispose();

        await Assert.That(() => resolver.GetServices<ViewModelOne>())
            .Throws<ObjectDisposedException>();
    }

    /// <summary>Test ServiceRegistrationCallback with a runtime type after disposal returns an empty disposable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_ServiceRegistrationCallback_NonGeneric_ReturnsEmptyDisposable()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Dispose();

        var callbackInvoked = false;

        await Assert.That(() =>
        {
            var subscription = resolver.ServiceRegistrationCallback(typeof(ViewModelOne), _ => callbackInvoked = true);
            subscription.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();

        await Assert.That(callbackInvoked).IsFalse();
    }

    /// <summary>Test non-generic GetService returns null when the last registration has been removed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_NonGeneric_AfterUnregisterAll_ReturnsNull()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));
        resolver.UnregisterAll(typeof(ViewModelOne));

        await Assert.That(resolver.GetService(typeof(ViewModelOne))).IsNull();
    }

    /// <summary>Test generic GetService returns default when the last registration has been removed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_Generic_AfterUnregisterAll_ReturnsDefault()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne());
        resolver.UnregisterAll<ViewModelOne>();

        await Assert.That(resolver.GetService<ViewModelOne>()).IsNull();
    }

    /// <summary>Test non-generic ServiceRegistrationCallback fires once for each existing registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_NonGeneric_WithExistingRegistrations_InvokesImmediately()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));

        var callbackCount = 0;
        using var subscription = resolver.ServiceRegistrationCallback(typeof(ViewModelOne), _ => callbackCount++);

        await Assert.That(callbackCount).IsEqualTo(NonGenericExistingCount);
    }

    /// <summary>Test generic ServiceRegistrationCallback fires once for each existing registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_Generic_WithExistingRegistrations_InvokesImmediately()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.Register(static () => new ViewModelOne());
        resolver.Register(static () => new ViewModelOne());

        var callbackCount = 0;
        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ => callbackCount++);

        await Assert.That(callbackCount).IsEqualTo(NonGenericExistingCount);
    }

    /// <summary>Test Dispose(false) from a finalizer-style path leaves the resolver usable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_WhenNotDisposingManagedResources_LeavesResolverUsable()
    {
        using var resolver = new FinalizerDisposableResolver();
        resolver.Register(static () => new ViewModelOne());

        resolver.InvokeNonManagedDispose();

        // Dispose(false) short-circuits before disposal, so the resolver remains active.
        await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsTrue();
        await Assert.That(resolver.GetService<ViewModelOne>()).IsNotNull();
    }

    /// <summary>Test that a lazy singleton whose factory disposes the resolver disposes the value and throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_WhenLazyFactoryDisposesResolver_DisposesValueAndThrows()
    {
        var resolver = new ModernDependencyResolver();
        var service = new DisposableTestService();

        resolver.RegisterLazySingleton(() =>
        {
            resolver.Dispose();
            return service;
        });

        await Assert.That(() => resolver.GetService<DisposableTestService>())
            .Throws<ObjectDisposedException>();

        await Assert.That(service.IsDisposed).IsTrue();
    }

    /// <summary>Test that a callback which disposes its token is removed after firing during a registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_WhenCallbackDisposesToken_RemovesCallbackAfterFiring()
    {
        using var resolver = new ModernDependencyResolver();
        var invocations = 0;

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(disposable =>
        {
            invocations++;
            disposable.Dispose();
        });

        resolver.Register(static () => new ViewModelOne());
        resolver.Register(static () => new ViewModelOne());

        // The callback disposed its token on the first registration, so it is removed and does not fire again.
        await Assert.That(invocations).IsEqualTo(SingleInvocation);
    }

    /// <inheritdoc />
    protected override ModernDependencyResolver GetDependencyResolver() => new();

    /// <summary>A resolver subclass that exposes the finalizer-style <see cref="ModernDependencyResolver.Dispose(bool)"/> path.</summary>
    private sealed class FinalizerDisposableResolver : ModernDependencyResolver
    {
        /// <summary>Invokes <see cref="ModernDependencyResolver.Dispose(bool)"/> with <see langword="false"/>, as a finalizer would.</summary>
        public void InvokeNonManagedDispose() => Dispose(false);
    }
}
