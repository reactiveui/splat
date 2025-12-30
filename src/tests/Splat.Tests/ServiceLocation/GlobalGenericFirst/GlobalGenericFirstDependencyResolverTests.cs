// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
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
    /// <summary>
    /// Teardown method to clear static generic containers after each test.
    /// Ensures no state leaks between test runs.
    /// </summary>
    [After(HookType.Test)]
    public void ClearStaticContainers() => GlobalGenericFirstDependencyResolver.Clear();

    /// <summary>
    /// Test constructor with configure parameter registers services.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithConfigure_RegistersServices()
    {
        var resolver = new GlobalGenericFirstDependencyResolver(r =>
        {
            r.Register<IViewModelOne>(() => new ViewModelOne());
            r.RegisterConstant(new ViewModelOne());
        });

        var result1 = resolver.GetService<IViewModelOne>();
        var result2 = resolver.GetService<ViewModelOne>();

        await Assert.That(result1).IsNotNull();
        await Assert.That(result2).IsNotNull();

        GlobalGenericFirstDependencyResolver.Clear();
    }

    /// <summary>
    /// Test constructor with null configure parameter does not throw.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithNullConfigure_DoesNotThrow()
    {
        await Assert.That(() =>
        {
            var resolver = new GlobalGenericFirstDependencyResolver(null);
            return Task.CompletedTask;
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test Clear method removes all registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Clear_RemovesAllRegistrations()
    {
        var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Register<IViewModelOne>(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne());

        await Assert.That(resolver.HasRegistration<IViewModelOne>()).IsTrue();
        await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsTrue();

        GlobalGenericFirstDependencyResolver.Clear();

        await Assert.That(resolver.HasRegistration<IViewModelOne>()).IsFalse();
        await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsFalse();
    }

    /// <summary>
    /// Test Dispose method disposes resolver properly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_DisposesResolver()
    {
        var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Register(() => new ViewModelOne());

        resolver.Dispose();

        await Assert.That(() =>
        {
            resolver.Register(() => new ViewModelOne());
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();
    }

    /// <summary>
    /// Test operations after disposal throw ObjectDisposedException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AfterDispose_Operations_ThrowObjectDisposedException()
    {
        var resolver = new GlobalGenericFirstDependencyResolver();
        resolver.Dispose();

        await Assert.That(() =>
        {
            resolver.Register(() => new ViewModelOne());
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();

        await Assert.That(() =>
        {
            resolver.Register(() => new ViewModelOne(), "contract");
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();

        await Assert.That(() =>
        {
            resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();

        await Assert.That(() =>
        {
            resolver.Register<IViewModelOne, ViewModelOne>();
            return Task.CompletedTask;
        }).Throws<ObjectDisposedException>();
    }

    /// <summary>
    /// Test UnregisterCurrent after disposal throws ObjectDisposedException.
    /// </summary>
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

    /// <summary>
    /// Test UnregisterAll after disposal throws ObjectDisposedException.
    /// </summary>
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
