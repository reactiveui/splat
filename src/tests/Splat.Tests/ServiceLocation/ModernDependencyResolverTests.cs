// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

[NotInParallel]
[InheritsTests]
public sealed class ModernDependencyResolverTests : BaseDependencyResolverTests<ModernDependencyResolver>
{
    /// <summary>
    /// Test ServiceRegistrationCallback with null service type throws.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_NullServiceType_Throws()
    {
        var resolver = new ModernDependencyResolver();

        await Assert.That(() => resolver.ServiceRegistrationCallback(null!, _ => { }))
            .Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Test ServiceRegistrationCallback invoked once per existing registration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_InvokedOncePerExistingRegistration()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne());

        var callbackCount = 0;

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ =>
        {
            callbackCount++;
        });

        await Assert.That(callbackCount).IsEqualTo(3); // Called once for each existing registration
    }

    /// <summary>
    /// Test ServiceRegistrationCallback after resolver disposal returns empty disposable.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_AfterDisposal_ReturnsEmptyDisposable()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Dispose();

        var callbackInvoked = false;

        await Assert.That(() =>
        {
            var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ =>
            {
                callbackInvoked = true;
            });
            subscription.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();

        await Assert.That(callbackInvoked).IsFalse();
    }

    /// <summary>
    /// Test Dispose method disposes resolver properly.
    /// </summary>
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

    /// <summary>
    /// Test Duplicate method creates a copy with same registrations.
    /// </summary>
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

    /// <summary>
    /// Test Duplicate of disposed resolver returns empty resolver.
    /// </summary>
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

    /// <summary>
    /// Test Duplicate creates independent resolver.
    /// </summary>
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
        await Assert.That(duplicateServices).Count().IsEqualTo(2);
    }

    /// <summary>
    /// Test GetService returns null after disposal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_GetService_ReturnsNull()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register(() => new ViewModelOne());
        resolver.Dispose();

        var result = resolver.GetService<ViewModelOne>();

        await Assert.That(result).IsNull();
    }

    /// <summary>
    /// Test HasRegistration returns false after disposal.
    /// </summary>
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
