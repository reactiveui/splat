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

    /// <summary>Test ServiceRegistrationCallback with null service type throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_NullServiceType_Throws()
    {
        var resolver = new ModernDependencyResolver();

        await Assert.That(() => resolver.ServiceRegistrationCallback(null!, _ => { }))
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
        resolver.Register(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne());

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
        resolver.Register(() => new ViewModelOne());

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
        resolver.Register<IViewModelOne>(() => new ViewModelOne());

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
        resolver.Register(() => new ViewModelOne());
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
        resolver.Register(() => new ViewModelOne());

        var duplicate = resolver.Duplicate();

        duplicate.Register(() => new ViewModelOne());

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
        resolver.Register(() => new ViewModelOne());
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
        resolver.Register(() => new ViewModelOne());
        resolver.Dispose();

        var result = resolver.HasRegistration<ViewModelOne>();

        await Assert.That(result).IsFalse();
    }

    /// <inheritdoc />
    protected override ModernDependencyResolver GetDependencyResolver() => new();
}
